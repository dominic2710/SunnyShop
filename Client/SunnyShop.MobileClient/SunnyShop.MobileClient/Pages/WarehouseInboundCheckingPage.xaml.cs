using SunnyShop.MobileClient.ViewModels;

namespace SunnyShop.MobileClient.Pages;

public partial class WarehouseInboundCheckingPage : ContentPage
{
	public WarehouseInboundCheckingPage(WarehouseInboundCheckingViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}