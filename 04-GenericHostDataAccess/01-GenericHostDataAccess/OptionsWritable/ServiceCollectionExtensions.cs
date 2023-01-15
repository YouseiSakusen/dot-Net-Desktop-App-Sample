using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace OptionsWritable;

/// <summary>IOptionsWritableをMS.E.DIに登録する拡張メソッドを表します。</summary>
public static class ServiceCollectionExtensions
{
	/// <summary>IOptionsWritableをMS.E.DIに登録します。</summary>
	/// <typeparam name="T">構成情報をマッピングする型を表します。</typeparam>
	/// <param name="services">IOptionsWritableを登録するDIコンテナを表すIServiceCollection。</param>
	/// <param name="configurationSection">構成情報をマッピングするセクションを表すIConfigurationSection。</param>
	/// <param name="jsonFileName">構成情報を読み書きするJSONフォーマットのファイル名を表す文字列。</param>
	/// <returns>IOptionsWritableを登録するDIコンテナを表すIServiceCollection。</returns>
	public static IServiceCollection ConfigureWritable<T>(this IServiceCollection services, IConfigurationSection configurationSection, string jsonFileName = "appsettings.json") where T : class, new()
	{
		services.Configure<T>(configurationSection);

		return ServiceCollectionExtensions.addWritableOptions<T>(services, configurationSection, jsonFileName);
	}

	/// <summary>IOptionsWritableをMS.E.DIに登録します。</summary>
	/// <typeparam name="T">構成情報をマッピングする型を表します。</typeparam>
	/// <param name="services">IOptionsWritableを登録するDIコンテナを表すIServiceCollection。</param>
	/// <param name="name">構成済みインスタンスの名前を表す文字列。</param>
	/// <param name="configurationSection">構成情報をマッピングするセクションを表すIConfigurationSection。</param>
	/// <param name="jsonFileName">構成情報を読み書きするJSONフォーマットのファイル名を表す文字列。</param>
	/// <returns>IOptionsWritableを登録するDIコンテナを表すIServiceCollection。</returns>
	public static IServiceCollection ConfigureWritable<T>(this IServiceCollection services, string name, IConfigurationSection configurationSection, string jsonFileName = "appsettings.json") where T : class, new()
	{
		services.Configure<T>(name, configurationSection);

		return ServiceCollectionExtensions.addWritableOptions<T>(services, configurationSection, jsonFileName);
	}

	/// <summary>構成済みインスタンスをDIコンテナに登録します。</summary>
	/// <typeparam name="T">構成情報をマッピングする型を表します。</typeparam>
	/// <param name="services">IOptionsWritableを登録するDIコンテナを表すIServiceCollection。</param>
	/// <param name="configurationSection">構成情報をマッピングするセクションを表すIConfigurationSection。</param>
	/// <param name="jsonFileName">構成情報を読み書きするJSONフォーマットのファイル名を表す文字列。</param>
	/// <returns>IOptionsWritableを登録するDIコンテナを表すIServiceCollection。</returns>
	private static IServiceCollection addWritableOptions<T>(IServiceCollection services, IConfigurationSection configurationSection, string jsonFileName) where T : class, new()
	{
		services.AddSingleton<IOptionsWritable<T>>(provider =>
		{
			var configRoot = (IConfigurationRoot) provider.GetRequiredService<IConfiguration>();
			var options = provider.GetRequiredService<IOptionsMonitor<T>>();

			return new OptionsWritable<T>(options, configRoot, configurationSection.Path.Split(':').First(), jsonFileName);
		});

		return services;
	}
}
