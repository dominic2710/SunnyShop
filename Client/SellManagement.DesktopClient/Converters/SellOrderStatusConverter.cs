using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SellManagement.DesktopClient.Converters
{
    class SellOrderStatusConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int status = (int)values[0];
            int waybillStatus = (int)values[1];

            string returnStatusStr = "";

            switch (status)
            {
                case 0:
                    returnStatusStr = "Đã đặt hàng";
                    break;
                case 1:
                    returnStatusStr =  "Đã đóng gói";
                    break;
                case 2:
                    switch (waybillStatus)
                    {
                        case 0:
                            returnStatusStr = "Chưa lấy hàng";
                            break;
                        case 2:
                            returnStatusStr = "Đã lấy hàng";
                            break;
                        case 9:
                            returnStatusStr = "Đã giao hàng";
                            break;
                        default:
                            returnStatusStr = "Đã xuất hàng";
                            break;
                    }
                    break;
                case 9:
                    returnStatusStr = "Đã hủy";
                    break;
                default:
                    returnStatusStr = "";
                    break;
            }

            return returnStatusStr;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
