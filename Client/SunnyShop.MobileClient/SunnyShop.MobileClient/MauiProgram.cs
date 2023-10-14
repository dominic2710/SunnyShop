using BarcodeScanner.Mobile;
using Camera.MAUI;
using CommunityToolkit.Maui;
using Plugin.Maui.Audio;
using SunnyShop.MobileClient.Pages;
using SunnyShop.MobileClient.ViewModels;

namespace SunnyShop.MobileClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCameraView()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "IconFontTypes");
            })
			.ConfigureMauiHandlers(handlers =>
			{
				handlers.AddBarcodeScannerHandler();
			});
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<WarehouseInboundCheckingPage>();
		builder.Services.AddSingleton<EditInventoryDetailPage>();
        builder.Services.AddSingleton<ListWarehouseInboundCheckingPage>();
        builder.Services.AddSingleton<WarehouseInboundCheckingViewModel>();
		builder.Services.AddSingleton<EditInventoryDetailViewModel>();
        builder.Services.AddSingleton<ListWarehouseInboundCheckingViewModel>();

        builder.Services.AddSingleton(AudioManager.Current);
		return builder.Build();
	}
}
