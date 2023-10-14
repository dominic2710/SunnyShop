using SunnyShop.MobileClient.Pages;

namespace SunnyShop.MobileClient;

public partial class AppShell : Shell
{
	public AppShell(ListWarehouseInboundCheckingPage listWarehouseInboundCheckingPage)
	{
		InitializeComponent();

        Routing.RegisterRoute("ListWarehouseInboundCheckingPage", typeof(ListWarehouseInboundCheckingPage));
        Routing.RegisterRoute("WarehouseInboundCheckingPage", typeof(WarehouseInboundCheckingPage));
        Routing.RegisterRoute("ScanBarcodePage", typeof(ScanBarcodePage));
        Routing.RegisterRoute("EditInventoryDetailPage", typeof(EditInventoryDetailPage));

        this.CurrentItem = listWarehouseInboundCheckingPage;
    }
}

