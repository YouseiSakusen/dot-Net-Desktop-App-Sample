namespace GenericHostDataAccess.Entities;

public record DatabaseSettings
{
	public const string Sqlite = nameof(Sqlite);
	public const string SqlServer = nameof(SqlServer);

	public const string SqliteSectionKey = $"{nameof(DatabaseSettings)}:{DatabaseSettings.Sqlite}";
	public const string SqlServerSectionKey = $"{nameof(DatabaseSettings)}:{DatabaseSettings.SqlServer}";

	public string ConnectString { get; set; } = string.Empty;

	public string DbFilePath { get; set; } = string.Empty;
}
