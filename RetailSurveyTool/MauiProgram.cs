using Jci.RetailSurveyTool.TechnicianApp;
using Microsoft.Extensions.Logging;

namespace RetailSurveyTool;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("fa-brands-400.ttf", "fa_brands");
				fonts.AddFont("fa-regular-400.ttf", "FAS");
				fonts.AddFont("fa-solid-900.ttf", "fa_solid");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
