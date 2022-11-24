using System.Data.Common;
using Microsoft.Extensions.Options;

namespace GenericHostJsonConf;

public class DictionaryOption
{
	private readonly DbConnection connection;
	//private readonly DatabaseSettings sqliteSettings;
	//private readonly DatabaseSettings sqlServerSettings;

	public string MyName { get; set; } = nameof(DictionaryOption);

	//public DictionaryOption(IOptionsMonitor<DatabaseSettings> options)
	public DictionaryOption(DbConnection dbConnection)
	{
		this.connection= dbConnection;

		Console.WriteLine($"{this.connection.ConnectionString}");

		//this.sqliteSettings = options.Get(DatabaseSettings.Sqlite);
		//this.sqlServerSettings = options.Get(DatabaseSettings.SqlServer);

		//Console.WriteLine(this.sqliteSettings.ConnectString);
		//Console.WriteLine(this.sqlServerSettings.ConnectString);
	}
}
