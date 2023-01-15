using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elf.DataAccesses.Repositories;

public static class RepositoryLiteExtensions
{
	public async static ValueTask OpenDataAccess(this IRepositoryLite repository, DatabaseType databaseType, bool withTransaction = false)
	{
		await repository.Connection.OpenAsync();
	}

	public async static ValueTask CommitAsync(this IRepositoryLite repository)
	{
		await repository.Unit.CommitAsync();
	}
}
