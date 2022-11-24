namespace GenericHostJsonConf;

/// <summary>DBの設定情報を表します。</summary>
public record DatabaseSettings
{
	public const string Sqlite = nameof(Sqlite);
	public const string SqlServer = nameof(SqlServer);

	public const string SqliteSectionKey = $"{nameof(DatabaseSettings)}:{DatabaseSettings.Sqlite}";
	public const string SqlServerSectionKey = $"{nameof(DatabaseSettings)}:{DatabaseSettings.SqlServer}";

	/// <summary>DBの接続文字列を取得・設定します。</summary>
	public string ConnectString { get; set; } = string.Empty;

	public string DbFilePath { get; set; } = string.Empty;
}
