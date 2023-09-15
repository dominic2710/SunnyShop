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
    class SellOrderStatusHighlightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int status = (int)values[0];
            int waybillStatus = (int)values[1];

            SolidColorBrush returnColor;

            switch (status)
            {
                case 0:
                    returnColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
                    break;
                case 1:
                    returnColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1b12c"));
                    break;
                case 2:
                    switch (waybillStatus)
                    {
                        case 0:
                            returnColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e1b12c"));
                            break;
                        case 2:
                            returnColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
                            break;
                        case 9:
                            returnColor = new SolidColorBrush(Colors.Green);
                            break;
                        default:
                            returnColor = new SolidColorBrush(Colors.Green);
                            break;
                    }
                    break;
                case 9:
                    returnColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ee5253"));
                    break;
                default:
                    returnColor = new SolidColorBrush(Colors.Transparent);
                    break;
            }

            return returnColor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
