using Microsoft.Extensions.Options;

namespace OptionsWritable;

/// <summary>設定情報を永続化するIOptionsMonitorを表します。</summary>
/// <typeparam name="T">設定情報をマッピングする型を表します。</typeparam>
public interface IOptionsWritable<out T> : IOptionsMonitor<T> where T : class, new()
{
	/// <summary>名前付きオプションを更新して永続化します。</summary>
	/// <param name="name">オプションの名前を表す文字列。</param>
	/// <param name="applyChange">オプションの内容を更新するAction<T>。</param>
	/// <returns>処理結果を表すValueTask。</returns>
	ValueTask UpdateAsync(string name, Action<T> applyChange);

	/// <summary>オプションを更新して永続化します。</summary>
	/// <param name="applyChange">オプションの内容を更新するAction<T>。</param>
	/// <returns>処理結果を表すValueTask。</returns>
	ValueTask UpdateAsync(Action<T> applyChange);

	/// <summary>更新時に発生するイベントを表します。</summary>
	/// <param name="listener">イベント発生時に実行するAction。</param>
	/// <returns>イベントを破棄するためのIDisposable。</returns>
	IDisposable OnUpdated(Action<T, string> listener);
}
