using System;

namespace DapperSample.Entities
{
	public class Character_ColName
	{
		public int ID { get; set; } = 0;

		public string CHARACTER_NAME { get; set; } = string.Empty;

		public string KANA { get; set; } = string.Empty;

		public DateTime? BIRTHDAY { get; set; } = null;

		public Sex Sex { get; set; } = Sex.設定なし;
	}
}
