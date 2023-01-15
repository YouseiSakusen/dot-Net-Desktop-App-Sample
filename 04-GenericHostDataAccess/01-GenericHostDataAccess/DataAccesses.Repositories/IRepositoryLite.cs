using System.Data.Common;

namespace elf.DataAccesses.Repositories;

public interface IRepositoryLite : IDisposable
{
	DbConnection Connection { get; init; }

	DbTransaction? Transaction { get; set; }
}
