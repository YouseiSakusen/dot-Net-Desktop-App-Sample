using System.Data.SQLite;
using GenericHostJsonConf;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		var configRoot = hostContext.Configuration;

		services.Configure<DatabaseSettings>(DatabaseSettings.Sqlite, configRoot.GetSection(DatabaseSettings.SqliteSectionKey))
			.PostConfigure<DatabaseSettings>(DatabaseSettings.Sqlite, ds =>
			{
				var builder = new SQLiteConnectionStringBuilder() { DataSource = Path.Combine(AppContext.BaseDirectory, ds.ConnectString) };

				ds.ConnectString = builder.ConnectionString;
			})
			.Configure<DatabaseSettings>(DatabaseSettings.SqlServer, configRoot.GetSection(DatabaseSettings.SqlServerSectionKey))
			.AddHostedService<Worker>();
	})
	.Build();

await host.RunAsync();
