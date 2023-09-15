using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Models
{
    public class FilterItemModel:BaseModel
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged(); }
        }

        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged(); }
        }


        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; OnPropertyChanged(); }
        }


    }
}
