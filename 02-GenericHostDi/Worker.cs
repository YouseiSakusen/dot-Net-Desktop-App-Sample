namespace GenericHostDi;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;
	private readonly TransientSample transient;
	private readonly IServiceScopeFactory scopeFactory;

	private int count = 0;

	public Worker(
		ILogger<Worker> logger,
		IHostApplicationLifetime applicationLifetime,
		TransientSample transientSample,
		IServiceScopeFactory serviceScopeFactory)
	{
		Console.WriteLine($"{nameof(Worker)} New!");

		(this._logger, this.transient, this.scopeFactory) = (logger, transientSample, serviceScopeFactory);

		applicationLifetime.ApplicationStarted.Register(this.OnStarted);
		applicationLifetime.ApplicationStopped.Register(this.OnStopped);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
			await Task.Delay(1000, stoppingToken);

			this.count++;
			if (this.count % 3 == 0)
			{
				await using (var scope = this.scopeFactory.CreateAsyncScope())
				{
					var sample = scope.ServiceProvider.GetRequiredService<ScopeSample>();
					sample.Foo();
				}
			}
		}
	}

	private void OnStarted()
		=> Console.WriteLine("Application Started!");

	private void OnStopped()
		=> Console.WriteLine("Application Stopped...");
}