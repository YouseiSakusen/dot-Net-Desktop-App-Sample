using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using elf.DataAccesses;
using elf.DataAccesses.Repositories;
using GenericHostDataAccess.Entities;

namespace GenericHostDataAccess.DataAccesses;

public class CharacterRepository : IRepository
{
	public DapperWrapper Dapper { get; init; }

	public CharacterRepository(DapperWrapper dapper)
		=> this.Dapper = dapper;

	public async Task<Character> GetByIdAsync(int id)
	{
		var sql = new StringBuilder();
		sql.AppendLine(" SELECT ");
		sql.AppendLine("       CHR.ID AS Id ");
		sql.AppendLine("     , CHR.CHARACTER_NAME AS Name ");
		sql.AppendLine("     , CHR.KANA AS Kana ");
		sql.AppendLine("     , CHR.SEX As Sex ");
		sql.AppendLine(" FROM ");
		sql.AppendLine("     CHARACTERS CHR ");
		sql.AppendLine(" WHERE     ");
		sql.AppendLine("     CHR.ID = @id ");

		return await this.Dapper.QuerySingleAsync<Character>(sql.ToString(), new { id = id });
	}

	public async Task<int> GetSameNameOrKanaAsync(Character character)
	{
		var sql = new StringBuilder();
		sql.AppendLine(" SELECT ");
		sql.AppendLine("     COUNT(0) AS RecordCount ");
		sql.AppendLine(" FROM ");
		sql.AppendLine("     CHARACTERS CHR ");
		sql.AppendLine(" WHERE ");
		sql.AppendLine("     	CHR.CHARACTER_NAME = @name ");
		sql.AppendLine("     OR  CHR.KANA = @kana ");

		return await this.Dapper.ExecuteScalarAsync<int>(sql.ToString(), new { name = character.Name, kana = character.Kana });
	}

	public async Task InsertCharacterAsync(Character character)
	{
		var sql = new StringBuilder();
		sql.AppendLine(" INSERT INTO CHARACTERS ( ");
		sql.AppendLine(" 	  CHARACTER_NAME ");
		sql.AppendLine(" 	, KANA ");
		sql.AppendLine(" 	, SEX ");
		sql.AppendLine(" 	, AFFILIATION ");
		sql.AppendLine(" ) VALUES ( ");
		sql.AppendLine(" 	  @name ");
		sql.AppendLine(" 	, @kana ");
		sql.AppendLine(" 	, @sex ");
		sql.AppendLine(" 	, @affiliation ");
		sql.AppendLine(" ) ");

		var param = new { name = character.Name, kana = character.Kana, sex = character.Sex, affiliation = character.Affiliation.id };

		await this.Dapper.ExecuteAsync(sql.ToString(), param);
	}

	public async Task<int> GetInsertedSequenceAsync()
	{
		var sql = new StringBuilder();
		sql.AppendLine(" SELECT ");
		sql.AppendLine("     seq ");
		sql.AppendLine(" FROM ");
		sql.AppendLine("     sqlite_sequence ");
		sql.AppendLine(" WHERE ");
		sql.AppendLine("     name = @tableName ");

		return await this.Dapper.ExecuteScalarAsync<int>(sql.ToString(), new { tableName = "CHARACTERS" });
	}
}
