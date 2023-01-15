using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using elf.DataAccesses.Repositories;

namespace GenericHostDataAccesses.DataAccesses.Lite;

public class CharacterRepositoryLite : IRepositoryLite
{
	public DbConnection Connection { get; init; }

	public DbTransaction? Transaction { get; set; } = null;

	public CharacterRepositoryLite(DbConnection dbConnection)
		=> this.Connection = dbConnection;

	private bool disposedValue;

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				// TODO: マネージド状態を破棄します (マネージド オブジェクト)
				this.Transaction?.Dispose();
			}

			// TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
			// TODO: 大きなフィールドを null に設定します
			disposedValue = true;
		}
	}

	// // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
	// ~CharacterRepositoryLite()
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
