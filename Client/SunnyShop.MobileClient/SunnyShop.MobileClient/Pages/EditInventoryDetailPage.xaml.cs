using CommunityToolkit.Maui.Core.Platform;
using SunnyShop.MobileClient.ViewModels;

namespace SunnyShop.MobileClient.Pages;

public partial class EditInventoryDetailPage : ContentPage
{
	public EditInventoryDetailPage(EditInventoryDetailViewModel editInventoryDetailViewModel)
	{
		InitializeComponent();

		this.BindingContext = editInventoryDetailViewModel;
	}

    private void ContentPage_NavigatingFrom(object sender, NavigatingFromEventArgs e)
    {
#if !MACCATALYST
		if (KeyboardExtensions.IsSoftKeyboardShowing(entryAddQuantity))
			KeyboardExtensions.HideKeyboardAsync(entryAddQuantity, default);
#endif
    }

    private void entryAddQuantity_Focused(object sender, FocusEventArgs e)
    {
        entryAddQuantity.CursorPosition = 1;

    }
}