using System.Collections.Generic;

namespace DapperSample.Entities
{
	/// <summary>キャラクターの所属を表します。</summary>
	public class Affiliation
	{
		public int Id { get; set; } = 0;

		public string Name { get; set; } = string.Empty;

		public List<Character_Normal> Characters
			=> this._characters;

		private readonly List<Character_Normal> _characters = new List<Character_Normal>();
	}
}
