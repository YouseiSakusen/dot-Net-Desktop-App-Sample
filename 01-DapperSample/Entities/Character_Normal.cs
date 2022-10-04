using System;

namespace DapperSample.Entities;

/// <summary>BLEACHのキャラクターを表します。</summary>
public class Character_Normal
{
	public int Id { get; set; } = 0;

	public string Name { get; set; } = string.Empty;

	public string Kana { get; set; } = string.Empty;

	public DateTime? Birthday { get; set; } = null;

	public Sex Sex { get; set; } = Sex.設定なし;

	public Affiliation Affiliation { get; set; } = new Affiliation();

	public Zanpakuto? Zanpakuto { get; set; } = null;

	public int AffiliationId
		=> this.Affiliation == null ? 0 : this.Affiliation.Id;
}
