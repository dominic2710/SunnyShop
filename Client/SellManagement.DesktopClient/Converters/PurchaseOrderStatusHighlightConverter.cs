using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SellManagement.DesktopClient.Converters
{
    class PurchaseOrderStatusHighlightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int status = (int)value;
            switch (status)
            {
                case 0:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
                case 2:
                    return new SolidColorBrush(Colors.Green);
                case 9:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ee5253"));
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
