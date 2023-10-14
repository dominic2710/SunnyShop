using SunnyShop.MobileClient.ViewModels;

namespace SunnyShop.MobileClient.Pages;

public partial class ListWarehouseInboundCheckingPage : ContentPage
{
	public ListWarehouseInboundCheckingPage(ListWarehouseInboundCheckingViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
    }
}