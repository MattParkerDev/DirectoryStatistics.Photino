using System.Text.Json;
using DirectoryStatistics.Models;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Photino.Blazor;

namespace DirectoryStatistics;

public class Program
{
	[STAThread]
	public static void Main(string[] args)
	{
		var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

		appBuilder.Services.AddLogging();
		appBuilder.Services.AddMudServices();
		appBuilder.Services.AddSingleton<AppState>();

		appBuilder.RootComponents.Add<App>("app");

		var app = appBuilder.Build();

		app.MainWindow
			.SetSize(1400, 800)
			.SetDevToolsEnabled(true)
			.SetLogVerbosity(0)
			//.SetIconFile("favicon.ico")
			.SetTitle("DirectoryStatistics");

		AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
		{
			app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
		};

		var configFilePath = GetConfigFilePath();

		using var scope = app.Services.CreateScope();
		var appState = scope.ServiceProvider.GetRequiredService<AppState>();

		LoadAppStateFromConfigFile(appState, configFilePath);

		app.MainWindow.RegisterWindowClosingHandler((sender, eventArgs) =>
		{
			using var stream = File.Create(configFilePath);
			JsonSerializer.Serialize(stream, appState, typeof(AppState));
			stream.Flush();
			return false;
		});

		app.Run();
	}

	private static string GetConfigFilePath()
	{
		var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		var configFolder = Path.Combine(folder, "DirectoryStatisticsConfig");
		Directory.CreateDirectory(configFolder);
		var configFilePath = Path.Combine(configFolder, "config.json");
		return configFilePath;
	}

	private static void LoadAppStateFromConfigFile(AppState appState, string configFilePath)
	{
		if (File.Exists(configFilePath) is false)
		{
			File.WriteAllText(configFilePath, string.Empty);
		}

		using var stream = File.OpenRead(configFilePath);
		if (stream.Length is 0)
		{
			return;
		}
		var deserializedAppState = JsonSerializer.Deserialize<AppState>(stream);
		if (deserializedAppState is not null)
		{
			appState.RootFolder = deserializedAppState.RootFolder;
		}
	}
}

