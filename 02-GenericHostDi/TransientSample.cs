using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace GenericHostDi;

public class TransientSample : IDisposable
{
	//private readonly DbConnection connection;
	private readonly IEnumerable<DbConnection> connections;
	private readonly SQLiteConnection sqliteConnection;
	private readonly SqlConnection sqlConnection;
	private readonly DbConnection? sqliteConnectionFromEnum;
	private readonly DbConnection? sqlConnectionFromEnum;

	//public TransientSample(DbConnection dbConnection)
	//public TransientSample(IEnumerable<DbConnection> dbConnections)
	public TransientSample(IEnumerable<DbConnection> dbConnections, SQLiteConnection sqliteCon, SqlConnection sqlCon)
	{
		Console.WriteLine($"{nameof(TransientSample)} New!");

		(this.connections, this.sqliteConnection, this.sqlConnection) = (dbConnections, sqliteCon, sqlCon);
		foreach (var item in this.connections)
		{
			Console.WriteLine($"{item.GetType().Name}");

			switch (item.GetType().Name)
			{
				case "SQLiteConnection":
					this.sqliteConnectionFromEnum = item;
					break;
				case "SqlConnection":
					this.sqlConnectionFromEnum = item;
					break;
			}
		}
		//this.connection = dbConnection;
		//Console.WriteLine($"Type: {this.connection.GetType().Name}");
	}

	public void Dispose()
		=> Console.WriteLine($"{nameof(TransientSample)} Dispose...");
}
