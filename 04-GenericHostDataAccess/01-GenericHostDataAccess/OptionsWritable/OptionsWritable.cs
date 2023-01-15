using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace OptionsWritable;

/// <summary>appsettings.jsonの更新をサポートしたIOptionsMonitor。</summary>
/// <typeparam name="T">構成情報をマッピングする型を表します。</typeparam>
public class OptionsWritable<T> : IOptionsWritable<T> where T : class, new()
{
	private readonly IOptionsMonitor<T> monitor;
	private readonly IConfigurationRoot configRoot;
	private readonly string sectionName;
	private readonly string jsonFileName;
	internal event Action<T, string>? onUpdated = null;

	/// <summary>コンストラクタ</summary>
	/// <param name="hostEnvironment">アプリケーションが実行されているホスティング環境を表すIHostEnvironment。</param>
	/// <param name="optionsMonitor">Configurationを表すIOptionsMonitor<T>。</param>
	/// <param name="configurationRoot">アプリケーション構成プロパティのセットを表すIConfigurationRoot?。</param>
	/// <param name="sectionName">アプリケーション構成プロパティのキーを表す文字列。</param>
	/// <param name="jsonFileName">Configurationを読み書きするJSONフォーマットのファイル名を表す文字列。</param>
	public OptionsWritable(IOptionsMonitor<T> optionsMonitor, IConfigurationRoot configurationRoot, string sectionName, string jsonFileName)
		=> (this.monitor, this.configRoot, this.sectionName, this.jsonFileName) = (optionsMonitor, configurationRoot, sectionName, jsonFileName);

	/// <summary>オプション情報の現在のインスタンスを取得します。</summary>
	public T CurrentValue
		=> this.monitor.CurrentValue;

	/// <summary>名前を指定して構成済みインスタンスを取得します。</summary>
	/// <param name="name">構成済みインスタンスの名前を表す文字列。</param>
	/// <returns>指定した名前に一致するインスタンスを表すT。</returns>
	public T Get(string? name)
		=> this.monitor.Get(name);

	/// <summary>名前付きオプションが変更された際に呼び出されるリスナーを登録します。</summary>
	/// <param name="listener">構成済みインスタンスが変更された時に呼び出されるAction。</param>
	/// <returns>リスナーを停止する際に破棄する必要があるIDisposable。</returns>
	public IDisposable? OnChange(Action<T, string?> listener)
		=> this.monitor.OnChange(listener);

	/// <summary>オプションを更新した後に発生するイベントを表します。</summary>
	/// <param name="listener">イベント発生時に実行するデリゲートを表すAction。</param>
	/// <returns>イベントの購読を解除するためのIDisposable。</returns>
	public IDisposable OnUpdated(Action<T, string> listener)
	{
		var tracker = new UpdatedTrackerDisposable(this, listener);
		this.onUpdated += tracker.OnUpdated;

		return tracker;
	}

	/// <summary>名前付きオプションを更新して永続化します。</summary>
	/// <param name="name">オプションの名前を表す文字列。</param>
	/// <param name="applyChange">オプションの内容を更新するAction<T>。</param>
	/// <returns>処理結果を表すValueTask。</returns>
	public async ValueTask UpdateAsync(string name, Action<T> applyChange)
	{
		// 構成済みオプションのインスタンスを取得
		var option = this.monitor.Get(name);
		// appsettings.jsonをデシリアライズ
		var appSettingsInfo = await this.getAppSettingsJsonInfoAsync(option, applyChange);

		if (!appSettingsInfo.Settings.TryGetValue(this.sectionName, out var sectionElement))
			throw new ArgumentException("インスタンス生成時に指定したセクション名が存在しません。");

		// Dictionary<string, object>でデシリアライズすると、値はJsonElementとして取得される
		// JsonElementのままでは扱いにくいので、書き換え対象箇所をDictionary<string, T>（T:DatabaseSettings）にデシリアライズ
		var jsonOption = new JsonSerializerOptions() { ReadCommentHandling = JsonCommentHandling.Skip };
		var updateSections = JsonSerializer.Deserialize<Dictionary<string, T>>(((JsonElement)sectionElement).GetRawText(), jsonOption);
		if (updateSections == null)
			throw new InvalidOperationException($"{nameof(JsonElement)}のデシリアライズに失敗しました。");
		if (!updateSections.ContainsKey(name))
			throw new ArgumentException($"パラメータ：{nameof(name)}の要素が見つかりませんでした。");

		// デシリアライズしたDictionary<string, T>の中身を
		// ラムダで編集したオブジェクトに置き換える。
		updateSections[name] = option;

		// 保存するためにはupdateSectionsをJsonElementに置き換える必要があるので
		// シリアライズ⇒デシリアライズしてJsonElementを作る
		var tempJson = JsonSerializer.Serialize<Dictionary<string, T>>(updateSections);
		var savedElement = JsonSerializer.Deserialize<JsonElement>(tempJson);

		await this.saveToAppSettingsJsonAsync(appSettingsInfo, savedElement, option, name);
	}

	/// <summary>オプションを更新して永続化します。</summary>
	/// <param name="applyChange">オプションの内容を更新するAction<T>。</param>
	/// <returns>処理結果を表すValueTask。</returns>
	public async ValueTask UpdateAsync(Action<T> applyChange)
	{
		var option = this.monitor.CurrentValue;
		var appSettingsInfo = await this.getAppSettingsJsonInfoAsync(option, applyChange);
		var tempJson = JsonSerializer.Serialize<T>(option);
		var savedElement = JsonSerializer.Deserialize<JsonElement>(tempJson);

		await this.saveToAppSettingsJsonAsync(appSettingsInfo, savedElement, option, string.Empty);
	}

	/// <summary>appsettings.jsonからデシリアライズしたオブジェクトとappsettings.jsonのフルパスを取得します。</summary>
	/// <param name="option">構成済みインスタンスを表すT。</param>
	/// <param name="applyChange">構成済みインスタンスを更新するためのAction。</param>
	/// <returns>appsettings.jsonからデシリアライズしたオブジェクトとappsettings.jsonのフルパスを表すTuple。</returns>
	/// <exception cref="InvalidOperationException">appsettings.jsonからのデシリアライズに失敗した場合にThrowされます。</exception>
	private async ValueTask<(Dictionary<string, object> Settings, string JsonPath)> getAppSettingsJsonInfoAsync(T option, Action<T> applyChange)
	{
		// 保存する値を編集するためのデリゲート
		applyChange(option);

		var jsonPath = Path.Combine(AppContext.BaseDirectory, this.jsonFileName);
		var appSettingText = await File.ReadAllTextAsync(jsonPath);
		// appsettings.json全体をデシリアライズ
		var jsonOption = new JsonSerializerOptions() { ReadCommentHandling = JsonCommentHandling.Skip };
		var appSettings = JsonSerializer.Deserialize<Dictionary<string, object>>(appSettingText, jsonOption);
		if (appSettings == null)
			throw new InvalidOperationException($"{this.jsonFileName}からデシリアライズできませんでした。");

		return (appSettings, jsonPath);
	}

	/// <summary>appsettings.jsonに保存します。</summary>
	/// <param name="appSettingsInfo">シリアライズするオブジェクトと保存先ファイルのフルパスを表すTuple。</param>
	/// <param name="savedElement">保存するJsonElement。</param>
	/// <param name="options">更新後のオプション情報を表すT。</param>
	/// <param name="name">更新対象の名前付きオプションの名前を表す文字列。（名前付きオプションでない場合は空文字）</param>
	/// <returns>処理結果を表すValueTask。</returns>
	private async ValueTask saveToAppSettingsJsonAsync((Dictionary<string, object> Settings, string jsonPath) appSettingsInfo, JsonElement savedElement, T options, string name)
	{
		// appsettings.jsonからデシリアライズしたDictionary<string, object>の
		// セクションを丸ごと上で作ったJsonElementに置き換える
		appSettingsInfo.Settings[this.sectionName] = savedElement;

		var serializeOption = new JsonSerializerOptions()
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			WriteIndented = true
		};

		// 後はappsettings.jsonからデシリアライズしたDictionary<string, object>を
		// appsettings.jsonに書き出す
		await File.WriteAllTextAsync(appSettingsInfo.jsonPath, JsonSerializer.Serialize<Dictionary<string, object>>(appSettingsInfo.Settings, serializeOption));
		
		// 設定情報を再読込
		this.configRoot.Reload();
		// 更新を通知
		this.onUpdated?.Invoke(options, name);
	}

	/// <summary>OnUpdatedの呼出を追跡します。</summary>
	internal sealed class UpdatedTrackerDisposable : IDisposable
	{
		private readonly Action<T, string> listener;
		private readonly OptionsWritable<T> optionsWritable;

		/// <summary>コンストラクタ。</summary>
		/// <param name="options">イベントの発生元を表すOptionsWritable<T>。</param>
		/// <param name="updateListener">イベントの発生時に実行するデリゲートを表すAction。</param>
		public UpdatedTrackerDisposable(OptionsWritable<T> options, Action<T, string> updateListener)
			=> (this.optionsWritable, this.listener) = (options, updateListener);

		/// <summary>OnUpdatedイベントを発生させます。</summary>
		/// <param name="options">更新対象のオプション情報を表すT。</param>
		/// <param name="name">名前付きオプションの名前を表す文字列。（名前無しの場合は空文字）</param>
		public void OnUpdated(T options, string name)
			=> this.listener.Invoke(options, name);

		/// <summary>イベントを破棄します。</summary>
		public void Dispose()
			=> this.optionsWritable.onUpdated -= this.OnUpdated;
	}
}
