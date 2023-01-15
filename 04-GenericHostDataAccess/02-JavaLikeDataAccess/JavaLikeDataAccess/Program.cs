using JavaLikeDataAccess;
using JavaLikeDataAccess.DataAccessObjects;
using JavaLikeDataAccess.Models;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices(services =>
	{
		services.AddHostedService<Worker>()
				.AddTransient<IDao<Character>, CharacterDao>();
	})
	.Build();

host.Run();
