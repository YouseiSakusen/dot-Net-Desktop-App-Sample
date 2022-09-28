using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows;
using Dapper;
using DapperSample.Entities;

namespace DapperSample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>接続文字列ビルダ</summary>
		private readonly SQLiteConnectionStringBuilder connectionStringBuilder;

		/// <summary>コンストラクタ。</summary>
		public MainWindow()
		{
			InitializeComponent();

			var dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleDb.db");

			this.connectionStringBuilder = new SQLiteConnectionStringBuilder() { DataSource = dbPath };
		}

		/// <summary>dynamic型で取得ボタンのClickイベントハンドラ。</summary>
		/// <param name="sender">イベントのソース。</param>
		/// <param name="e">イベントデータを表すRoutedEventArgs。</param>
		private async void btnDynamic_Click(object sender, RoutedEventArgs e)
		{
			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				var sql = new StringBuilder();
				sql.AppendLine(" SELECT ");
				sql.AppendLine("     * ");
				sql.AppendLine(" FROM ");
				sql.AppendLine("     CHARACTERS CHR ");
				sql.AppendLine(" WHERE ");
				sql.AppendLine("     CHR.ID <= @Id ");

				var characters = await con.QueryAsync(sql.ToString(), new { Id = 5 });

				var buf = new StringBuilder();
				buf.AppendLine(this.getColumnNames<dynamic>(characters, 3, "\t"));

				foreach (var item in characters)
				{
					buf.AppendLine($"{item.ID}\t{item.CHARACTER_NAME}\t{item.KANA}");
				}

				this.txtResult.Text = buf.ToString();
			}
		}

		/// <summary>Dapperのマッピング結果からカラム名の一覧をから取得します。</summary>
		/// <typeparam name="T">Dapperでマッピングしたクラスの型を表します。</typeparam>
		/// <param name="values">>Dapperの取得結果を表すIEnumerable<T>。</param>
		/// <param name="columnCount">取得するカラムの数を表すint。</param>
		/// <param name="separator">各カラム名を区切る文字列。</param>
		/// <returns></returns>
		private string getColumnNames<T>(IEnumerable<T> values, int columnCount, string separator)
		{
			var columns = values.OfType<IDictionary<string, object>>().First().Select(x => x.Key).Take(columnCount);

			return string.Join(separator, columns);
		}

		private async void btnAdo_Click(object sender, RoutedEventArgs e)
		{
			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				var sql = new StringBuilder();
				sql.AppendLine(" SELECT ");
				sql.AppendLine("     * ");
				sql.AppendLine(" FROM ");
				sql.AppendLine("     CHARACTERS CHR ");
				sql.AppendLine(" WHERE ");
				sql.AppendLine("		CHR.KANA LIKE @kana ");
				sql.AppendLine("     AND CHR.ID <= @id ");

				var character = await con.QueryFirstOrDefaultAsync(sql.ToString(), new { kana = "%くろさき%", id = 5 });

				var buf = new StringBuilder();
				buf.AppendLine(character.ID.ToString());
				buf.AppendLine(character.CHARACTER_NAME);
				buf.AppendLine(character.KANA);

				//await con.OpenAsync();

				//await using (var cmd = con.CreateCommand())
				//{
				//	cmd.CommandText = sql.ToString();
				//	cmd.CommandType = CommandType.Text;
				//	cmd.Parameters.AddWithValue("kana", "%くろさき%");
				//	cmd.Parameters.AddWithValue("id", 5);

				//	await using (var reader = await cmd.ExecuteReaderAsync())
				//	{
				//		while (await reader.ReadAsync())
				//		{
				//			buf.AppendLine(reader.GetInt32("ID").ToString());
				//			buf.AppendLine(reader.GetString("CHARACTER_NAME"));
				//			buf.AppendLine(reader.GetString("KANA"));

				//			break;
				//		}
				//	}
				//}

				this.txtResult.Text = buf.ToString();
			}
		}

		private async void btnColName_Click(object sender, RoutedEventArgs e)
		{
			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				var sql = new StringBuilder();
				sql.AppendLine(" SELECT ");
				sql.AppendLine("     * ");
				sql.AppendLine(" FROM ");
				sql.AppendLine("     CHARACTERS CHR ");
				sql.AppendLine(" WHERE ");
				sql.AppendLine("		CHR.KANA LIKE @kana ");
				sql.AppendLine("     AND CHR.ID <= @id ");

				var characters = await con.QueryAsync<Character_ColName>(sql.ToString(), new { kana = "%くろさき%", id = 10 });

				var buf = new StringBuilder();
				buf.AppendLine($"ID\tCHARACTER_NAME\tKANA\tSEX\tBIRTHDAY");

				foreach (var item in characters)
				{
					buf.AppendLine($"{item.ID}\t{item.CHARACTER_NAME}\t{item.KANA}\t{item.Sex.ToString()}\t{item.BIRTHDAY?.ToString("yyyy/MM/dd")}");
				}

				this.txtResult.Text = buf.ToString();
			}
		}

		private async void btnNormal_Click(object sender, RoutedEventArgs e)
		{
			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				var sql = new StringBuilder();
				sql.AppendLine(" SELECT ");
				sql.AppendLine("       CHR.ID AS Id ");
				sql.AppendLine("     , CHR.CHARACTER_NAME AS Name ");
				sql.AppendLine("     , CHR.KANA AS Kana ");
				sql.AppendLine("     , CHR.BIRTHDAY AS Birthday ");
				sql.AppendLine("     , CHR.SEX AS Sex ");
				sql.AppendLine(" FROM ");
				sql.AppendLine("     CHARACTERS CHR ");
				sql.AppendLine(" WHERE ");
				sql.AppendLine("         CHR.ZANPAKUTOU IS NOT NULL ");
				sql.AppendLine("     AND CHR.KANA LIKE @kana ");
				sql.AppendLine("     AND CHR.ID <= @Id ");

				var characters = await con.QueryAsync<Character_Normal>(sql.ToString(), new { kana = "%くろさき%", id = 10 });

				var buf = new StringBuilder();

				buf.AppendLine($"ID\tCHARACTER_NAME\tKANA\tSEX\tBIRTHDAY");

				foreach (var item in characters)
				{
					buf.AppendLine($"{item.Id}\t{item.Name}\t{item.Kana}\t{item.Sex.ToString()}\t{item.Birthday?.ToString("yyyy/MM/dd")}");
				}

				this.txtResult.Text = buf.ToString();
			}
		}

		private async void btnJoinOne_Click(object sender, RoutedEventArgs e)
		{
			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				var sql = new StringBuilder();
				sql.AppendLine(" SELECT ");
				sql.AppendLine("       CHR.ID AS Id ");
				sql.AppendLine("     , CHR.CHARACTER_NAME AS Name ");
				sql.AppendLine("     , CHR.KANA AS Kana ");
				sql.AppendLine("     , CHR.SEX AS Sex ");
				sql.AppendLine("     , AFL.ID AS Id ");
				sql.AppendLine("     , AFL.AFFILIATION_NAME AS Name ");
				sql.AppendLine("     , ZPT.ZANPAKUTOU_NAME AS Name ");
				sql.AppendLine("     , ZPT.BANKAI_NAME AS Bankai ");
				sql.AppendLine(" FROM ");
				sql.AppendLine("     CHARACTERS CHR ");
				sql.AppendLine("     INNER JOIN AFFILIATION AFL ON ");
				sql.AppendLine("         CHR.AFFILIATION = AFL.ID ");
				sql.AppendLine("     LEFT JOIN ZANPAKUTOU ZPT ON ");
				sql.AppendLine("         CHR.ZANPAKUTOU = ZPT.ID ");
				sql.AppendLine(" WHERE ");
				sql.AppendLine("     CHR.AFFILIATION = @id ");
				sql.AppendLine(" ORDER BY ");
				sql.AppendLine("     CHR.KANA; ");

				var characters = await con.QueryAsync<Character_Normal, Affiliation, Zanpakuto, Character_Normal>(sql.ToString(), (chara, aff, zpt) => 
				{
					chara.Affiliation = aff;
					chara.Zanpakuto = zpt;

					return chara;
				}
				, new { id = 15 },
				splitOn:"Id,Id,Name");

				var buf = new StringBuilder();

				buf.AppendLine($"ID\tCHARACTER_NAME\tKANA\tSEX\t所属\t斬魄刀\t卍解");

				foreach (var item in characters)
				{
					buf.AppendLine($"{item.Id}\t{item.Name}\t{item.Kana}\t{item.Sex.ToString()}\t{item.Affiliation.Name}\t{item.Zanpakuto?.Name}\t{item.Zanpakuto?.Bankai}");
				}

				this.txtResult.Text = buf.ToString();
			}
		}

		private async void btnInMapping_Click(object sender, RoutedEventArgs e)
		{
			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				var sql = new StringBuilder();
				sql.AppendLine(" SELECT ");
				sql.AppendLine("     * ");
				sql.AppendLine(" FROM ");
				sql.AppendLine("     CHARACTERS CHR ");
				sql.AppendLine(" WHERE ");
				sql.AppendLine("     CHR.ID IN @id ");

				var characters = await con.QueryAsync(sql.ToString(), new { id = new List<int>() { 1, 2, 3, 6, 9, 34, 36, 37 } });

				var buf = new StringBuilder();
				buf.AppendLine($"ID\tCHARACTER_NAME\tKANA");

				foreach (var item in characters)
				{
					buf.AppendLine(item.ID + "\t{item.CHARACTER_NAME}\t{item.KANA}\t");
				}

				this.txtResult.Text = buf.ToString();
			}
		}

		private async void btnJoinMany_Click(object sender, RoutedEventArgs e)
		{
			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				var sql = new StringBuilder();
				sql.AppendLine(" SELECT ");
				sql.AppendLine("       AFL.ID AS Id ");
				sql.AppendLine("     , AFL.AFFILIATION_NAME AS Name ");
				sql.AppendLine("     , CHR.ID AS Id ");
				sql.AppendLine("     , CHR.CHARACTER_NAME AS Name ");
				sql.AppendLine("     , CHR.SEX AS Sex ");
				sql.AppendLine("     , ZPT.ZANPAKUTOU_NAME AS Name ");
				sql.AppendLine("     , ZPT.BANKAI_NAME AS Bankai ");
				sql.AppendLine(" FROM ");
				sql.AppendLine("     AFFILIATION AFL ");
				sql.AppendLine("     INNER JOIN CHARACTERS CHR ON ");
				sql.AppendLine("         AFL.ID = CHR.AFFILIATION ");
				sql.AppendLine("     LEFT JOIN ZANPAKUTOU ZPT ON ");
				sql.AppendLine("         CHR.ZANPAKUTOU = ZPT.ID ");
				sql.AppendLine(" ORDER BY ");
				sql.AppendLine("       AFL.ID ");
				sql.AppendLine("     , CHR.ID ");
				sql.AppendLine(" LIMIT 10 ");

				var affilicationDic = new Dictionary<int, Affiliation>();
				var characters = await con.QueryAsync<Affiliation, Character_Normal, Zanpakuto, Affiliation>(sql.ToString(), (aff, chara, zpt) =>
				{
					Affiliation? tempAff = null;

					if (!affilicationDic.TryGetValue(aff.Id, out tempAff))
					{
						tempAff = aff;
						affilicationDic.Add(aff.Id, tempAff);
					}

					chara.Zanpakuto = zpt;
					tempAff.Characters.Add(chara);

					return tempAff;
				},
				splitOn: "Id,Id,Name");

				var buf = new StringBuilder();

				buf.AppendLine($"ID\t所属\tキャラクター名\tKANA\tSEX\t斬魄刀\t卍解");

				foreach (var item in affilicationDic.Values)
				{
					buf.AppendLine($"{item.Id}\t{item.Name}");
					item.Characters.ForEach(c => buf.AppendLine($"\t\t{c.Name}\t{c.Kana}\t{c.Sex}\t{c.Zanpakuto?.Name}\t{c.Zanpakuto?.Bankai}"));
				}

				this.txtResult.Text = buf.ToString();
			}
		}

		private async void btnScalar_Click(object sender, RoutedEventArgs e)
		{
			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				var sql = new StringBuilder();
				sql.AppendLine(" SELECT ");
				sql.AppendLine("     COUNT(1) ");
				sql.AppendLine(" FROM ");
				sql.AppendLine("     CHARACTERS CHR ");

				var count = await con.ExecuteScalarAsync<int>(sql.ToString());

				var buf = new StringBuilder();
				buf.AppendLine($"登録済みキャラクター数");
				buf.AppendLine($"{count}人");

				this.txtResult.Text = buf.ToString();
			}
		}

		private async void btnInsert_Click(object sender, RoutedEventArgs e)
		{
			var chara = new Character_Normal()
			{
				Name = "バンビエッタ・バスターバイン",
				Kana = "バンビエッタバスターバイン",
				Sex = Sex.女,
				Affiliation = new Affiliation() { Id = 19 }
			};
			var sql = new StringBuilder();
			sql.AppendLine(" INSERT INTO CHARACTERS(  ");
			sql.AppendLine(" 	  CHARACTER_NAME ");
			sql.AppendLine(" 	, KANA ");
			sql.AppendLine(" 	, SEX ");
			sql.AppendLine(" 	, AFFILIATION ");
			sql.AppendLine(" ) VALUES (  ");
			sql.AppendLine(" 	  @Name ");
			sql.AppendLine(" 	, @Kana ");
			sql.AppendLine(" 	, @Sex ");
			sql.AppendLine(" 	, @AffiliationId ");
			sql.AppendLine(" ) ");

			await using (var con = new SQLiteConnection(this.connectionStringBuilder.ConnectionString))
			{
				await con.OpenAsync();
				var newId = 0;

				await using (var tran = con.BeginTransaction())
				{
					try
					{
						var delCount = await con.ExecuteAsync("DELETE FROM CHARACTERS WHERE KANA = @kana", new { kana = "バンビエッタバスターバイン" });
						//var count = await con.ExecuteAsync(sql.ToString(), chara, tran);
						var count = await con.ExecuteAsync(sql.ToString(), new {Name = chara.Name, Kana = chara.Kana, Sex = chara.Sex, AffiliationId = chara.Affiliation.Id}, tran);
						newId = await con.ExecuteScalarAsync<int>("SELECT seq FROM sqlite_sequence WHERE name = @tableName ", new { tableName = "CHARACTERS" });
						await tran.CommitAsync();
					}
					catch (Exception)
					{
						await tran.RollbackAsync();
						throw;
					}

					var newChara = await con.QueryFirstOrDefaultAsync("SELECT * FROM CHARACTERS WHERE ID = @id", new { id = newId });
					var buf = new StringBuilder();

					buf.AppendLine($"ID\t所属\tキャラクター名\tKANA\tSEX");
					buf.AppendLine($"{newChara.ID}\t{newChara.AFFILIATION}\t{newChara.CHARACTER_NAME}\t{newChara.KANA}\t{newChara.SEX}");
					this.txtResult.Text = buf.ToString();
				}
			}
		}
	}
}
