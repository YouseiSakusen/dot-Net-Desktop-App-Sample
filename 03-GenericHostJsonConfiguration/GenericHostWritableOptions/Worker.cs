using GenericHostJsonConf;
using OptionsWritable;

namespace GenericHostWritableOptions;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;
	private readonly IDisposable optionsWritableDisposable;

	public Worker(ILogger<Worker> logger, IOptionsWritable<DatabaseSettings> options)
	{
		_logger = logger;
		var dbSetting = options.Get(DatabaseSettings.Sqlite);

		Console.WriteLine(dbSetting.ConnectString);

		this.optionsWritableDisposable = options.OnUpdated((settings, name) => Console.WriteLine($"Updated!!"));
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

	public override void Dispose()
	{
		this.optionsWritableDisposable.Dispose();
		base.Dispose();
	}
}