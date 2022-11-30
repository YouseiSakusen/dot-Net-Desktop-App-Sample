using GenericHostJsonConf;
using GenericHostWritableOptions;
using OptionsWritable;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		var configRoot = hostContext.Configuration;

		services.AddHostedService<Worker>()
				.ConfigureWritable<DatabaseSettings>(DatabaseSettings.Sqlite, configRoot.GetSection(DatabaseSettings.SqliteSectionKey));
	})
	.Build();

await host.RunAsync();
