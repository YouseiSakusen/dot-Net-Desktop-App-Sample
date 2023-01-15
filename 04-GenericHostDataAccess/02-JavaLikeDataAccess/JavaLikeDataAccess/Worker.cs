using JavaLikeDataAccess.DataAccessObjects;
using JavaLikeDataAccess.Models;

namespace JavaLikeDataAccess;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;
	private readonly IDao<Character> characterDao;

	public Worker(ILogger<Worker> logger, IDao<Character> dao)
	{
		_logger = logger;
		this.characterDao = dao;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

			var character = await this.characterDao.GetByIdAsync(1);
			Console.WriteLine(character?.Name);

			await Task.Delay(1000, stoppingToken);
		}
	}
}