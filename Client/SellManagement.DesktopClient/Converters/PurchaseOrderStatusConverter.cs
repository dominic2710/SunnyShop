using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SellManagement.DesktopClient.Converters
{
    class PurchaseOrderStatusConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int status = (int)value;
            switch (status)
            {
                case 0:
                    return "Đã đặt hàng";
                case 2:
                    return "Đã nhập hàng";
                case 9:
                    return "Đã hủy";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
