using elf.DataAccesses;
using elf.DataAccesses.Repositories;
using GenericHostDataAccess.DataAccesses;
using GenericHostDataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GenericHostDataAccess.ApplicationLogics;

public class AppLogic
{
	private readonly IServiceScopeFactory scopeFactory;

	public AppLogic(IServiceScopeFactory serviceScopeFactory)
	{
		this.scopeFactory = serviceScopeFactory;
	}

	public async ValueTask DoNoTransactionAsync()
	{
		await using var scope = this.scopeFactory.CreateAsyncScope();
		using var unit = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
		var repository = scope.ServiceProvider.GetRequiredService<CharacterRepository>();

		await repository.OpenDataAccess(unit, DatabaseType.SQLiteConnection);

		var chara = await repository.GetByIdAsync(1);
	}

	public async ValueTask<Character?> InsertNewCharacter()
	{
		var character = new Character()
		{
			Name = "ユーハバッハ",
			Kana = "ユーハバッハ",
			Sex = Sex.Male,
			Affiliation = new Affiliation("19")
		};

		await using var scope = this.scopeFactory.CreateAsyncScope();
		var repository = scope.ServiceProvider.GetRequiredService<CharacterRepositoryLite>();
		await repository.OpenDataAccess(DatabaseType.SQLiteConnection, true);

		var count = await repository.GetSameNameOrKanaAsync(character);
		if (count != 0) return null;

		await repository.InsertCharacterAsync(character);
		var id = await repository.GetInsertedSequenceAsync();
		await repository.CommitAsync();

		return await repository.GetByIdAsync(id);
	}
}
