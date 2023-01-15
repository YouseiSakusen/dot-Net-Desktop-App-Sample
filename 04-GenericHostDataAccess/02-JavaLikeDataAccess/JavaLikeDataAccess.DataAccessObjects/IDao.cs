namespace JavaLikeDataAccess.DataAccessObjects;

/// <summary>
/// Data Access Objectパターン用のインタフェースを定義。
/// </summary>
/// <typeparam name="T">実際に読み書きする型を指定。</typeparam>
public interface IDao<T> where T : class
{
	public ValueTask<List<T>> GetAllAsync();

	public ValueTask<T?> GetByIdAsync(int id);

	public ValueTask SaveAsync(T item);

	public ValueTask DeleteByIdAsync(int id);
}
