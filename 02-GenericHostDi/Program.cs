using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using GenericHostDi;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices(services =>
	{
		services.AddHostedService<Worker>()
			.AddTransient<TransientSample>()
			.AddScoped<ScopeSample>()
			.AddTransient<DbConnection, SQLiteConnection>()
			.AddTransient<DbConnection>(_ => new SqlConnection())
			.AddTransient<SQLiteConnection>(_ => new SQLiteConnection())
			.AddTransient<SqlConnection>(_ => new SqlConnection());
	})
	.Build();

Console.WriteLine($"RunAsync íºëOÅI");

await host.RunAsync();

Console.WriteLine($"RunAsync íºå„...");
