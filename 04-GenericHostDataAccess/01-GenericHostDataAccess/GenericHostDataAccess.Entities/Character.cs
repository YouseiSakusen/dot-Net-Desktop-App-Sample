namespace GenericHostDataAccess.Entities;

public enum Sex
{
	None,
	Male,
	Femail,
	Other
}

public class Character
{
	public int Id { get; set; } = 0;

	public string Name { get; set; } = string.Empty;

	public string Kana { get; set; } = string.Empty;

	public Sex Sex { get; set; }

	public Affiliation Affiliation { get; set; } = new Affiliation("15");
}
