using Xunit;
using GenericHostJsonConf;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OptionsWritable.Tests;

public class ServiceCollectionExtensionsTests
{
	[Fact(DisplayName = "IOptionsWritable名前無しオプション登録テスト")]
	public void ConfigureWritableNoNameTest()
	{
		const string jsonFileName = "appsettingsNoName-configure.json";

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
	}

	[Theory(DisplayName = "IOptionsWritable名前付きオプション登録テスト")]
	[InlineData(DatabaseSettings.Sqlite, DatabaseSettings.SqliteSectionKey, "Sqlite")]
	[InlineData(DatabaseSettings.SqlServer, DatabaseSettings.SqlServerSectionKey, "SqlServer")]
	public void ConfigureWritableNamedTest(string name, string sectionName, string jsonValue)
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
	}
}