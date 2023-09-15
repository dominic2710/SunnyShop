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
    /// Interaction logic for SearchShippingCompanyView.xaml
    /// </summary>
    public partial class SearchShippingCompanyView : Window
    {
        public SearchShippingCompanyView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseable)
            {
                (DataContext as ICloseable).RequestClose += (_, __) => this.Close();
            }
        }
    }
}
