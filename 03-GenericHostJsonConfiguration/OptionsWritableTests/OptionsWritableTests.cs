using Xunit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using GenericHostJsonConf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace OptionsWritable.Tests;

public class OptionsWritableTests
{
	[Fact(DisplayName = "OptionsWritable更新名前無し")]
	public async Task UpdateNoNameAsyncTest()
	{
		const string jsonFileName = "appsettingsNoName.json";

		using var hostbuilder = Host.CreateDefaultBuilder()
			.ConfigureHostConfiguration(builder =>
			{
				builder.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile(jsonFileName);
			})
			.ConfigureServices((hostContext, services) =>
			{
				var configRoot = hostContext.Configuration;

				services.ConfigureWritable<DatabaseSettings>(configRoot.GetSection(nameof(DatabaseSettings)), jsonFileName);
			})
			.Build();

		var options = hostbuilder.Services.GetService<IOptionsWritable<DatabaseSettings>>();

		Assert.NotNull(options);
		Assert.True(options.CurrentValue.ConnectString == "NoName");

		var updatedValue = $"foo{options.CurrentValue.ConnectString}bar";
		using var disposable = options.OnUpdated((settings, name) =>
		{
			Assert.True(settings.ConnectString == updatedValue);
		});

		await options.UpdateAsync(ds =>
		{
			ds.ConnectString = updatedValue;
		});

		var jsonPath = Path.Combine(AppContext.BaseDirectory, jsonFileName);
		var lines = File.ReadAllText(jsonPath).Split(Environment.NewLine);
		var tempWords = lines[2].Split(":");
		var value = tempWords[1].Trim().Replace(@"""", "").Replace(",", "");

		Assert.True(value == updatedValue);
	}

	[Theory(DisplayName = "IOptionsWritable更新名前付き")]
	[InlineData(DatabaseSettings.Sqlite, DatabaseSettings.SqliteSectionKey, "Sqlite", $"foo{DatabaseSettings.Sqlite}bar", 3)]
	[InlineData(DatabaseSettings.SqlServer, DatabaseSettings.SqlServerSectionKey, "SqlServer", $"foo{DatabaseSettings.SqlServer}bar", 7)]
	public async Task UpdateAsyncNamedTest(string name, string sectionName, string jsonValue, string updatedValue, int valueLine)
	{
		const string jsonFileName = "appsettings-configure.json";

		using var hostbuilder = Host.CreateDefaultBuilder()
			.ConfigureHostConfiguration(builder =>
			{
				builder.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile(jsonFileName);
			})
			.ConfigureServices((hostContext, services) =>
			{
				var configRoot = hostContext.Configuration;

				services.ConfigureWritable<DatabaseSettings>(name, configRoot.GetSection(sectionName), jsonFileName);
			})
			.Build();

		var options = hostbuilder.Services.GetService<IOptionsWritable<DatabaseSettings>>();

		Assert.NotNull(options);
		Assert.True(options.Get(name).ConnectString == jsonValue);

		using var disposable = options.OnUpdated((settings, name) =>
		{
			Assert.True(settings.ConnectString == updatedValue);
		});

		await options.UpdateAsync(name, ds =>
		{
			ds.ConnectString = updatedValue;
		});

		var jsonPath = Path.Combine(AppContext.BaseDirectory, jsonFileName);
		var lines = File.ReadAllText(jsonPath).Split(Environment.NewLine);
		var tempWords = lines[valueLine].Split(":");
		var value = tempWords[1].Trim().Replace(@"""", "").Replace(",", "");

		Assert.True(value == updatedValue);
	}
}