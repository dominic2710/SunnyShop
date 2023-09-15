using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient
{
    interface IFocusable
    {
        event EventHandler<FocusEventArgs> RequestFocus;
    }

    public class FocusEventArgs 
    {
        public string FocusItem { get; set; }
    }
}
