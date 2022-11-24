using Microsoft.Extensions.Options;

namespace GenericHostJsonConf;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;
	private readonly DictionaryOption dictionaryOption;
	//private readonly DatabaseSettings databaseSettings;
	//private readonly IServiceProvider provider;

	//public Worker(ILogger<Worker> logger, IConfiguration configuration)
	//public Worker(ILogger<Worker> logger, IOptions<DatabaseSettings> options)
	public Worker(ILogger<Worker> logger, DictionaryOption dictionary, IServiceProvider serviceProvider)
	{
		_logger = logger;
		this.dictionaryOption = dictionary;

		//this.databaseSettings = options.Value;
		//this.provider = serviceProvider;

		//var dicOption = this.provider.GetRequiredService<DictionaryOption>();
		//Console.WriteLine(dicOption.MyName);

		//Console.WriteLine(this.databaseSettings.ConnectString);

		//Console.WriteLine(configuration.GetValue<string>("TestConfig"));
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