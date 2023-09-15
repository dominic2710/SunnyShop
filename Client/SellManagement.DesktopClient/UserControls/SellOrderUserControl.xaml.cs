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
    /// Interaction logic for SellOrderUserControl.xaml
    /// </summary>
    public partial class SellOrderUserControl : UserControl
    {
        public SellOrderUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IFocusable)
            {
                (DataContext as IFocusable).RequestFocus += (_, __) => SetFocus(__.FocusItem);
            }
            PART_SellOrderNo.Focus();
        }

        private void SetFocus(string focusItem)
        {
            switch (focusItem)
            {
                case "SellOrderNo":
                    PART_SellOrderNo.Focus();
                    break;
                case "RefSellOrderNo":
                    PART_RefSellOrderNo.Focus();
                    break;
                case "SellOrderDate":
                    PART_SellOrderDate.Focus();
                    break;
                case "PlannedExportDate":
                    PART_PlannedExportDate.Focus();
                    break;
                case "CustomerCd":
                    PART_CustomerCd.Focus();
                    break;
                case "ShippingCompanyCd":
                    PART_ShippingCompanyCd.Focus();
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
