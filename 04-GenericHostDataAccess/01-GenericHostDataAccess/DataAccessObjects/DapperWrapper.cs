using System.Data;
using System.Data.Common;
using Dapper;

namespace elf.DataAccessObjects;

/// <summary>
/// Dapperのラッパークラスを表します。
/// </summary>
public class DapperWrapper
{
	private DbConnection connection = null!;
	private DbTransaction? transaction = null;

	/// <summary>
	/// DBへのコネクションとトランザクションを設定します。
	/// </summary>
	/// <param name="dbConnection">DBへの有効な接続を表すDbConnection。</param>
	/// <param name="dbTransaction">DBのトランザクションを表すDbTransaction?。</param>
	internal void SetDataAccessObjects(DbConnection dbConnection, DbTransaction? dbTransaction = null)
		=> (this.connection, this.transaction) = (dbConnection, dbTransaction);

	public int Execute(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Execute(sql, param, this.transaction, commandTimeout, commandType);
	}

	public T ExecuteScalar<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.ExecuteScalar<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public IEnumerable<dynamic> Query(string sql, object? param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query(sql, param, this.transaction, buffered, commandTimeout, commandType);
	}

	public IEnumerable<object> Query(Type type, string sql, object? param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query(type, sql, param, this.transaction, buffered, commandTimeout, commandType);
	}

	public IEnumerable<T> Query<T>(string sql, object? param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query<T>(sql, param, this.transaction, buffered, commandTimeout, commandType);
	}

	public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query<TFirst, TSecond, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query<TFirst, TSecond, TThird, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public dynamic QueryFirst(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.QueryFirst(sql, param, this.transaction, commandTimeout, commandType);
	}

	public T QueryFirst<T>(string sql, object? param = null, int? commandTimeout = null, CommandType ? commandType = null)
	{
		return this.connection.QueryFirst<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public dynamic QueryFirstOrDefault(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.QueryFirstOrDefault(sql, param, this.transaction, commandTimeout, commandType);
	}

	public T QueryFirstOrDefault<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.QueryFirstOrDefault<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public dynamic QuerySingle(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.QuerySingle(sql, param, this.transaction, commandTimeout, commandType);
	}

	public T QuerySingle<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.QuerySingle<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public dynamic QuerySingleOrDefault(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.QuerySingleOrDefault(sql, param, this.transaction, commandTimeout, commandType);
	}

	public T QuerySingleOrDefault<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return this.connection.QuerySingleOrDefault<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<int> ExecuteAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.ExecuteAsync(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.ExecuteScalarAsync<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<IEnumerable<dynamic>> QueryAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryAsync(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryAsync<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryAsync<TFirst, TSecond, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryAsync<TFirst, TSecond, TThird, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object? param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(sql, map, param, this.transaction, buffered, splitOn, commandTimeout, commandType);
	}

	public async Task<dynamic> QueryFirstAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryFirstAsync(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<T> QueryFirstAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryFirstAsync<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<dynamic> QueryFirstOrDefaultAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryFirstOrDefaultAsync(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QueryFirstOrDefaultAsync<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<dynamic> QuerySingleAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QuerySingleAsync(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QuerySingleAsync<T>(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<dynamic> QuerySingleOrDefaultAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QuerySingleOrDefault(sql, param, this.transaction, commandTimeout, commandType);
	}

	public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
	{
		return await this.connection.QuerySingleOrDefaultAsync<T>(sql, param, this.transaction, commandTimeout, commandType);
	}
}
