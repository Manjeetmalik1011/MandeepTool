using Jci.RetailSurveyTool.TechnicianApp;
using Microsoft.Extensions.Logging;
using Jci.RetailSurveyTool.TechnicianApp.Core.CommonUtility;

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
                fonts.AddFont("fa-brands-400.ttf", "FAB");
                fonts.AddFont("fa-regular-400.ttf", "FAR");
                fonts.AddFont("fa-solid-900.ttf", "FAS");
            });

        HndlerUtility.ModifyEntry();
        HndlerUtility.ModifyPicker();

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            //MessageBox.Show(text: error.ExceptionObject.ToString(), caption: "Error");
        };

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
