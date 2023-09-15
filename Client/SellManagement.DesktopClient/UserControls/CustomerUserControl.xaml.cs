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
    /// Interaction logic for CustomerUserControl.xaml
    /// </summary>
    public partial class CustomerUserControl : UserControl
    {
        public CustomerUserControl()
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
                case "CustomerCd":
                    PART_CustomerCd.Focus();
                    break;
                case "Category":
                    PART_Category.Focus();
                    break;
                case "CustomerName":
                    PART_CustomerName.Focus();
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
            PART_CustomerCd.Focus();

        }
    }
}
