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
using System.Windows.Shapes;

namespace SellManagement.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : Window
    {
        public ProductView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseable)
            {
                (DataContext as ICloseable).RequestClose += (_, __) => this.Close();
                (DataContext as IFocusable).RequestFocus += (_, __) => SetFocus(__.FocusItem);
            }
            PART_ProductCd.Focus();
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
                case "ProductCd":
                    PART_TabMain.SelectedIndex = 0;
                    PART_ProductCd.Focus();
                    break;
                case "Category":
                    PART_TabMain.SelectedIndex = 0;
                    PART_Category.Focus();
                    break;
                case "ProductName":
                    PART_TabMain.SelectedIndex = 0;
                    PART_ProductName.Focus();
                    break;
                case "TradeMark":
                    PART_TabMain.SelectedIndex = 0;
                    PART_TradeMark.Focus();
                    break;
                case "Origin":
                    PART_TabMain.SelectedIndex = 0;
                    PART_Origin.Focus();
                    break;
                case "ComboTab":
                    PART_TabMain.SelectedIndex = 1;
                    break;
            }
        }
    }
}
