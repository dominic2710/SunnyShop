using BarcodeScanner.Mobile;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using SunnyShop.MobileClient.Services.Product;

namespace SunnyShop.MobileClient.Pages;

public partial class ScanBarcodePage : ContentPage
{
	public ScanBarcodePage()
	{
		InitializeComponent();

#if !MACCATALYST
		BarcodeScanner.Mobile.Methods.AskForRequiredPermission();
#endif
    }

    Stream mediaFile;
    IAudioPlayer audioPlayer;

    private async void CameraView_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
    {
        List<BarcodeResult> obj = e.BarcodeResults;
        var scannedBarcode = obj.First().DisplayValue;
        audioPlayer.Play();
        activityIndicator.IsRunning = true;

        var result = await Services.ServiceProvider.GetInstance().CallWebApi<string, GetProductByCdResponse>("/Product/GetProductByBarcode", HttpMethod.Post, scannedBarcode);

        if (result.StatusCode == 200)
        {
            if (result.ProductData == null)
            {
                await Shell.Current.DisplayAlert("Lỗi", $"Barcode: {scannedBarcode}\r\nKhông tìm thấy sản phẩm tương ứng!", "OK");
                cameraView.IsScanning = true;
            }
            else
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Shell.Current.GoToAsync($"EditInventoryDetailPage?barCode={scannedBarcode}");
                });
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Lỗi", result.StatusMessage, "OK");
            cameraView.IsScanning = true;
        }
        activityIndicator.IsRunning = false;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        cameraView.IsScanning = true;
        Task.Run(async () =>
        {
            mediaFile = await FileSystem.OpenAppPackageFileAsync("beep.mp3");
            audioPlayer = AudioManager.Current.CreatePlayer(mediaFile);
        });
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        cameraView.IsScanning = true;
    }
}