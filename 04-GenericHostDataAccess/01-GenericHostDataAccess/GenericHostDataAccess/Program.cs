using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using elf.DataAccesses;
using elf.DataAccesses.Repositories;
using GenericHostDataAccess;
using GenericHostDataAccess.ApplicationLogics;
using GenericHostDataAccess.DataAccesses;
using GenericHostDataAccess.Entities;
using Microsoft.Extensions.Options;
using OptionsWritable;

//var host = Host.CreateDefaultBuilder(args)
//	.ConfigureServices((hostContext, services) =>
//	{
//		var configRoot = hostContext.Configuration;

//		services.ConfigureWritable<DatabaseSettings>(DatabaseSettings.Sqlite, configRoot.GetSection(DatabaseSettings.SqliteSectionKey))
//			.ConfigureWritable<DatabaseSettings>(DatabaseSettings.SqlServer, configRoot.GetSection(DatabaseSettings.SqlServerSectionKey))
//			.PostConfigure<DatabaseSettings>(DatabaseSettings.Sqlite, settings =>
//			{
//				var builder = new SQLiteConnectionStringBuilder() { DataSource = Path.Combine(AppContext.BaseDirectory, settings.DbFilePath) };

//				settings.ConnectString = builder.ConnectionString;
//			})
//			.AddHostedService<WorkerLite>()
//			.AddTransient<AppLogicLite>()
//			.AddScoped<CharacterRepositoryLite>()
//			.AddTransient<UnitOfWork>()
//			.AddTransient<DapperWrapper>()
//			.AddTransient<DbConnection>(provider =>
//			{
//				var option = provider.GetRequiredService<IOptionsMonitor<DatabaseSettings>>();

//				return new SQLiteConnection(new SQLiteConnection(option.Get(DatabaseSettings.Sqlite).ConnectString));
//			})
//			.AddTransient<DbConnection>(provider =>
//			{
//				var option = provider.GetRequiredService<IOptionsMonitor<DatabaseSettings>>();

//				return new SqlConnection(option.Get(DatabaseSettings.SqlServer).ConnectString);
//			});
//	})
//	.Build();

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		var configRoot = hostContext.Configuration;

		services.ConfigureWritable<DatabaseSettings>(DatabaseSettings.Sqlite, configRoot.GetSection(DatabaseSettings.SqliteSectionKey))
			.ConfigureWritable<DatabaseSettings>(DatabaseSettings.SqlServer, configRoot.GetSection(DatabaseSettings.SqlServerSectionKey))
			.PostConfigure<DatabaseSettings>(DatabaseSettings.Sqlite, settings =>
			{
				var builder = new SQLiteConnectionStringBuilder() { DataSource = Path.Combine(AppContext.BaseDirectory, settings.DbFilePath) };

				settings.ConnectString = builder.ConnectionString;
			})
			.AddHostedService<Worker>()
			.AddTransient<AppLogic>()
			.AddScoped<CharacterRepository>()
			.AddTransient<DapperWrapper>()
			.AddScoped<UnitOfWork>()
			.AddTransient<DbConnection>(provider =>
			{
				var option = provider.GetRequiredService<IOptionsMonitor<DatabaseSettings>>();

				return new SQLiteConnection(new SQLiteConnection(option.Get(DatabaseSettings.Sqlite).ConnectString));
			})
			.AddTransient<DbConnection>(provider =>
			{
				var option = provider.GetRequiredService<IOptionsMonitor<DatabaseSettings>>();

				return new SqlConnection(option.Get(DatabaseSettings.SqlServer).ConnectString);
			});
	})
	.Build();

await host.RunAsync();
