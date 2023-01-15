using GenericHostDataAccess.ApplicationLogics;

namespace GenericHostDataAccess;

public class WorkerLite : BackgroundService
{
	private readonly ILogger<Worker> _logger;
	private readonly AppLogicLite appLogic;

	public WorkerLite(ILogger<Worker> logger, AppLogicLite app)
	{
		_logger = logger;
		this.appLogic = app;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var character = await this.appLogic.InsertNewCharacter();

		var tcs = new TaskCompletionSource<bool>();
		stoppingToken.Register(s => (s as TaskCompletionSource<bool>)?.SetResult(true), tcs);
		await tcs.Task;
	}
}