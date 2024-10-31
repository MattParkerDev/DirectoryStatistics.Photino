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

		app.Run();
	}
}

