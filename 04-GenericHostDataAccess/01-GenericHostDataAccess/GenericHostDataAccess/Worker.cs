using GenericHostDataAccess.ApplicationLogics;

namespace GenericHostDataAccess;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;
	private readonly AppLogic appLogic;

	public Worker(ILogger<Worker> logger, AppLogic app)
	{
		_logger = logger;
		this.appLogic = app;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var character = this.appLogic.InsertNewCharacter();
		//await this.appLogic.DoNoTransactionAsync();

		var tcs = new TaskCompletionSource<bool>();
		stoppingToken.Register(s => (s as TaskCompletionSource<bool>)?.SetResult(true), tcs);
		await tcs.Task;
	}
}