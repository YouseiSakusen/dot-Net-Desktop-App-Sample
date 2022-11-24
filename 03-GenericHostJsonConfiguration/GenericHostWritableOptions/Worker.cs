using GenericHostJsonConf;
using OptionsWritable;

namespace GenericHostWritableOptions;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;

	public Worker(ILogger<Worker> logger, IOptionsWritable<DatabaseSettings> options)
	{
		_logger = logger;
		var dbSetting = options.Get(DatabaseSettings.Sqlite);

		Console.WriteLine(dbSetting.ConnectString);

		options.UpdateAsync(DatabaseSettings.Sqlite, ds => ds.ConnectString = "hoge");
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
			await Task.Delay(1000, stoppingToken);
		}
	}
}