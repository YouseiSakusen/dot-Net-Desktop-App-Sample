using System.Data.Common;
using System.Data.SQLite;
using GenericHostJsonConf;
using Microsoft.Extensions.Options;

IHost host = Host.CreateDefaultBuilder(args)
		.ConfigureServices((hostContext, services) =>
		{
			var configRoot = hostContext.Configuration;

			//services.ConfigureWritable<DatabaseSettings>(DatabaseSettings.Sqlite, configRoot.GetSection(DatabaseSettings.SqliteSectionKey))
			//services.ConfigureWritable<DatabaseSettings>(configRoot.GetSection(nameof(DatabaseSettings)))
			services.Configure<DatabaseSettings>(DatabaseSettings.Sqlite, configRoot.GetSection(DatabaseSettings.SqliteSectionKey))
				.PostConfigure<DatabaseSettings>(DatabaseSettings.Sqlite, ds =>
				{
					var builder = new SQLiteConnectionStringBuilder() { DataSource = Path.Combine(AppContext.BaseDirectory, ds.ConnectString) };

					ds.ConnectString = builder.ConnectionString;
				})
				.Configure<DatabaseSettings>(DatabaseSettings.SqlServer, configRoot.GetSection(DatabaseSettings.SqlServerSectionKey))
				.AddHostedService<Worker>()
				.AddTransient<DictionaryOption>()
				.AddTransient<DbConnection, SQLiteConnection>(provider =>
				{
					var option = provider.GetRequiredService<IOptionsMonitor<DatabaseSettings>>();
					var connection = new SQLiteConnection(option.Get(DatabaseSettings.Sqlite).ConnectString);

					//connection.WaitTimeout = 10;
					//connection.Open();

					return connection;
				})
				.AddScoped<ScopeSample>();
		})
		.Build();

await host.RunAsync();
