namespace GenericHostDataAccess.Entities;

public record Affiliation(string id, string name = "")
{
	public string Id { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;
}
