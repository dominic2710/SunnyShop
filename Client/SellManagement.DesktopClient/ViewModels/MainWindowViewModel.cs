using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using SellManagement.DesktopClient.Views;

namespace SellManagement.DesktopClient.ViewModels
{
    public class MainWindowViewModel:BaseViewModel, ICloseable
    {
        private const int OVERVIEW_TAB_IDX = 0;
        private const int LISTPRODUCT_TAB_IDX = 1;
        private const int LISTCUSTOMER_TAB_IDX = 2;
        private const int LISTSUPPLIER_TAB_IDX = 3;
        private const int LISTSHIPPINGCOMPANY_TAB_IDX = 4;
        private const int LISTPURCHASEORDER_TAB_IDX = 5;
        private const int LISTSELLORDER_TAB_IDX = 6;
        private const int LISTCLASSIFYNAME_TAB_IDX = 7;

        public MainWindowViewModel() 
        {
            LogoutCommand = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    ServiceProvider.GetInstance().Logout();
                    var handler = RequestClose;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                    if (LoginWindow != null)
                    {
                        LoginWindow.Show();
                    }
                });

            OpenOverviewCommand = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    SelectedIndex = OVERVIEW_TAB_IDX;
                });
            OpenListProductCommand = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    SelectedIndex = LISTPRODUCT_TAB_IDX;
                });
            OpenListCustomerCommand = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    SelectedIndex = LISTCUSTOMER_TAB_IDX;
                });
            OpenListSupplierCommand = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    SelectedIndex = LISTSUPPLIER_TAB_IDX;
                });
            OpenListShippingCompanyCommand = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    SelectedIndex = LISTSHIPPINGCOMPANY_TAB_IDX;
                });
            OpenPurchaseOrderCommand = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    SelectedIndex = LISTPURCHASEORDER_TAB_IDX;
                });
            OpenSellOrderCommand = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    SelectedIndex = LISTSELLORDER_TAB_IDX;
                });
            OpenListOfClassifyName = new RelayCommand<object>(p => { return true; },
                p =>
                {
                    SelectedIndex = LISTCLASSIFYNAME_TAB_IDX;
                });
        }

        public event EventHandler<EventArgs> RequestClose;

        #region "Properties"
        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; OnPropertyChanged(); }
        }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public LoginView LoginWindow { get; set; }
        #endregion

        #region "Command"
        public ICommand LogoutCommand { get; set; }
        public ICommand OpenOverviewCommand { get; set; }
        public ICommand OpenListProductCommand { get; set; }
        public ICommand OpenListCustomerCommand { get; set; }
        public ICommand OpenListSupplierCommand { get; set; }
        public ICommand OpenPurchaseOrderCommand { get; set; }
        public ICommand OpenListShippingCompanyCommand { get; set; }
        public ICommand OpenSellOrderCommand { get; set; }
        public ICommand OpenListOfClassifyName { get; set; }
        #endregion

    }
}
