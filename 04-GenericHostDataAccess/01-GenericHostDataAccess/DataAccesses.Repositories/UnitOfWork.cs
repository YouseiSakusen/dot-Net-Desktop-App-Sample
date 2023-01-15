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

public class UnitOfWork : IDisposable
{
	//private readonly IServiceScopeFactory scopeFactory;

	internal DbConnection? CurrentConnection => this.connection;

	internal DbTransaction? CurrentTransaction => this.transaction;

	private readonly IEnumerable<DbConnection> connections;

	public UnitOfWork(IServiceScopeFactory serviceScopeFactory, IEnumerable<DbConnection> dbConnections)
		=> this.connections = dbConnections;

	private DbConnection? connection = null;
	private DbTransaction? transaction = null;
	private bool inTransaction = false;
	private bool disposedValue;

	internal async ValueTask OpenAsync(DatabaseType databaseType, bool withTransaction)
	{
		if (this.connection != null)
			throw new InvalidOperationException("既に接続が開いています。");

		//await using var scope = this.scopeFactory.CreateAsyncScope();
		//var connections = scope.ServiceProvider.GetRequiredService<IEnumerable<DbConnection>>();

		this.connection = this.connections.Where(con => con.GetType().Name == databaseType.ToString()).FirstOrDefault();
		if (this.connection == null)
			return;

		await this.connection.OpenAsync();
		this.inTransaction= withTransaction;

		if (withTransaction)
			this.transaction= await this.connection.BeginTransactionAsync();
	}

	internal async ValueTask CommitAsync()
	{
		if (this.transaction != null)
			await this.transaction.CommitAsync();
	}

	internal async ValueTask RollbackAsync()
	{
		if (this.transaction != null)
			await this.transaction.RollbackAsync();
	}

	//internal async ValueTask CloseAsync()
	//{
	//	//if ((this.inTransaction) && (this.transaction != null))
	//	//{
	//	//	await this.transaction.RollbackAsync();
	//	//	await this.transaction.DisposeAsync();
	//	//}
	//	this.transaction = null;

	//	//if (this.connection != null)
	//	//{
	//	//	await this.connection.DisposeAsync();
	//	//}
	//	this.connection = null;
	//	this.inTransaction = false;
	//}

	//public async ValueTask DisposeAsync()
	//{
	//	if ((this.inTransaction) && (this.transaction != null))
	//	{
	//		await this.transaction.DisposeAsync();
	//		this.inTransaction = false;
	//	}

	//	if (this.connection != null)
	//	{
	//		await this.connection.DisposeAsync();
	//	}
	//}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				// TODO: マネージド状態を破棄します (マネージド オブジェクト)
				if (this.transaction != null)
				{
					this.transaction.Dispose();
				}
			}

			// TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
			// TODO: 大きなフィールドを null に設定します
			disposedValue = true;
		}
	}

	// // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
	// ~UnitOfWork()
	// {
	//     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
	//     Dispose(disposing: false);
	// }

	public void Dispose()
	{
		// このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
