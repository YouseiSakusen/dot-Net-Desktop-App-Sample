using Microsoft.Extensions.Options;

namespace GenericHostJsonConf;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;
	private readonly DatabaseSettings databaseSettings;

	public Worker(ILogger<Worker> logger, IOptionsMonitor<DatabaseSettings> options)
	{
		_logger = logger;
		this.databaseSettings = options.Get(DatabaseSettings.Sqlite);

		Console.WriteLine(this.databaseSettings.ConnectString);
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