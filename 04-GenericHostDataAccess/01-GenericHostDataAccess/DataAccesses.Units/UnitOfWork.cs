using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace elf.DataAccesses;

public enum DatabaseType
{
	SQLiteConnection,
	SqlConnection
}

public class UnitOfWork : IAsyncDisposable
{
	private readonly IServiceScopeFactory scopeFactory;

	public UnitOfWork(IServiceScopeFactory serviceScopeFactory)
		=> this.scopeFactory = serviceScopeFactory;

	private DbConnection? connection = null;
	private DbTransaction? transaction = null;
	private bool inTransaction = false;

	public async ValueTask OpenAsync(DatabaseType databaseType, bool withTransaction)
	{
		if (this.connection != null)
			throw new InvalidOperationException("既に接続が開いています。");

		await using var scope = this.scopeFactory.CreateAsyncScope();
		var connections = scope.ServiceProvider.GetRequiredService<IEnumerable<DbConnection>>();

		this.connection = connections.Where(con => con.GetType().Name == databaseType.ToString()).FirstOrDefault();
		if (this.connection == null)
			return;

		await this.connection.OpenAsync();
		this.inTransaction= withTransaction;

		if (withTransaction)
			this.transaction= await this.connection.BeginTransactionAsync();
	}

	public async ValueTask CommitAsync()
	{
		if (this.transaction != null)
			await this.transaction.CommitAsync();
	}

	public async ValueTask RollbackAsync()
	{
		if (this.transaction != null)
			await this.transaction.RollbackAsync();
	}

	public async ValueTask CloseAsync()
	{
		if ((this.inTransaction) && (this.transaction != null))
		{
			await this.transaction.RollbackAsync();
			await this.transaction.DisposeAsync();
		}
		this.transaction = null;

		if (this.connection != null)
		{
			await this.connection.DisposeAsync();
		}
		this.connection = null;
		this.inTransaction = false;
	}

	public async ValueTask DisposeAsync()
	{
		await this.CloseAsync();
	}
}
