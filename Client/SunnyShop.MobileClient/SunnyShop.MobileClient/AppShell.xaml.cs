using SunnyShop.MobileClient.Pages;

namespace SunnyShop.MobileClient;

public partial class AppShell : Shell
{
	public AppShell(WarehouseInboundCheckingPage warehouseInboundCheckingPage)
	{
		InitializeComponent();

        Routing.RegisterRoute("WarehouseInboundCheckingPage", typeof(WarehouseInboundCheckingPage));
        Routing.RegisterRoute("ScanBarcodePage", typeof(ScanBarcodePage));
        Routing.RegisterRoute("EditInventoryDetailPage", typeof(EditInventoryDetailPage));

        this.CurrentItem = warehouseInboundCheckingPage;
    }
}

