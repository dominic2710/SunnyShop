using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient
{
    interface ICloseable
    {
        event EventHandler<EventArgs> RequestClose;
    }
}
