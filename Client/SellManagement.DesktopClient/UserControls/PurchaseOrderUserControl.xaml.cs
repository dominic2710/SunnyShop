using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SellManagement.DesktopClient.UserControls
{
    /// <summary>
    /// Interaction logic for PurchaseOrderUserControl.xaml
    /// </summary>
    public partial class PurchaseOrderUserControl : UserControl
    {
        public PurchaseOrderUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IFocusable)
            {
                (DataContext as IFocusable).RequestFocus += (_, __) => SetFocus(__.FocusItem);
            }
            PART_PurchaseOrderNo.Focus();
        }

        private void SetFocus(string focusItem)
        {
            switch (focusItem)
            {
                case "PurchaseOrderNo":
                    PART_PurchaseOrderNo.Focus();
                    break;
                case "RefPurchaseOrderNo":
                    PART_RefPurchaseOrderNo.Focus();
                    break;
                case "PurchaseOrderDate":
                    PART_PurchaseOrderDate.Focus();
                    break;
                case "PlannedImportDate":
                    PART_PlannedImportDate.Focus();
                    break;
                case "SupplierCd":
                    PART_SupplierCd.Focus();
                    break;
            }
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
    }
}
