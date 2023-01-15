using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elf.DataAccesses.Repositories;

public static class RepositoryExtensions
{
	public async static ValueTask OpenDataAccess(this IRepository repository, UnitOfWork unitOfWork, DatabaseType databaseType, bool withTransaction = false)
	{
		await unitOfWork.OpenAsync(databaseType, withTransaction);
		repository.Dapper.SetDataAccessObjects(unitOfWork.CurrentConnection, unitOfWork.CurrentTransaction);

	}
}
