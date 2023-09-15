using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SellManagement.DesktopClient.UserControls
{
    /// <summary>
    /// Interaction logic for ShippingCompanyUserControl.xaml
    /// </summary>
    public partial class ShippingCompanyUserControl : UserControl
    {
        public ShippingCompanyUserControl()
        {
            InitializeComponent();
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void TextBox_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender.GetType() == typeof(TextBox))
            {
                (sender as TextBox).SelectAll();
            }
        }

        private void SetFocus(string focusItem)
        {
            switch (focusItem)
            {
                case "ShippingCompanyCd":
                    PART_ShippingCompanyCd.Focus();
                    break;
                case "ShippingCompanyName":
                    PART_ShippingCompanyName.Focus();
                    break;
                case "PhoneNumber":
                    PART_PhoneNumber.Focus();
                    break;
                case "Address1":
                    PART_Address1.Focus();
                    break;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IFocusable)
            {
                (DataContext as IFocusable).RequestFocus += (_, __) => SetFocus(__.FocusItem);
            }
            PART_ShippingCompanyCd.Focus();
        }
    }
}
