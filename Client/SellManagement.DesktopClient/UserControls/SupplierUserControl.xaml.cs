using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SellManagement.DesktopClient.UserControls
{
    /// <summary>
    /// Interaction logic for SupplierUserControl.xaml
    /// </summary>
    public partial class SupplierUserControl : UserControl
    {
        public SupplierUserControl()
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
                case "SupplierCd":
                    PART_SupplierCd.Focus();
                    break;
                case "Category":
                    PART_Category.Focus();
                    break;
                case "SupplierName":
                    PART_SupplierName.Focus();
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
            if (DataContext is ICloseable)
            {
                (DataContext as IFocusable).RequestFocus += (_, __) => SetFocus(__.FocusItem);
            }
            PART_SupplierCd.Focus();
        }
    }
}
