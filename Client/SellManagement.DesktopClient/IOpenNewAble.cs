using SellManagement.DesktopClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SellManagement.DesktopClient
{
    public interface IOpenNewAble
    {
        event EventHandler<OpenEventArgs> RequestOpen;
    }

    public class OpenEventArgs
    {
        public UserControl OpenUserControl { get; set; }
        public string Header { get; set; }
    }
}
