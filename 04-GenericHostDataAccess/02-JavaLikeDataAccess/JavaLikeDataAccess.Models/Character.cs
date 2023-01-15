namespace JavaLikeDataAccess.Models;

/// <summary>
/// DBから取得した結果を格納するクラス。
/// このクラス自体がDTO。
/// </summary>
public class Character
{
	public int Id { get; set; } = 0;

	public string Name { get; set; } = string.Empty;

	public string Kana { get; set; } = string.Empty;

	public DateTime? Birthday { get; set; } = null;
}
