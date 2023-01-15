using System.Data.SQLite;
using JavaLikeDataAccess.Models;

namespace JavaLikeDataAccess.DataAccessObjects;

/// <summary>
/// DAOの実装。
/// </summary>
public class CharacterDao : IDao<Character>
{
	public async ValueTask<Character?> GetByIdAsync(int id)
	{
		var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleDb.db");
		var builder = new SQLiteConnectionStringBuilder() { DataSource = dbPath };
		var sql = " SELECT * FROM CHARACTERS CHR WHERE CHR.ID = @Id ";

		using var connection = new SQLiteConnection(builder.ConnectionString);
		await connection.OpenAsync();

		await using var command = connection.CreateCommand();
		command.CommandText = sql;
		command.Parameters.AddWithValue("Id", id);
		await using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var character = new Character();

			character.Name = reader["CHARACTER_NAME"] == DBNull.Value ? string.Empty : reader["CHARACTER_NAME"].ToString()!;
			character.Kana = reader["KANA"] == DBNull.Value ? string.Empty : reader["KANA"].ToString()!;

			return character;
		}

		return null;
	}

	public ValueTask<List<Character>> GetAllAsync()
	{
		throw new NotImplementedException();
	}

	public ValueTask DeleteByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public ValueTask SaveAsync(Character item)
	{
		throw new NotImplementedException();
	}
}
