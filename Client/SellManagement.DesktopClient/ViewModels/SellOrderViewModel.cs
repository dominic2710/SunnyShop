using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.SellOrder;
using SellManagement.DesktopClient.Services.VoucherNoManagement;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using SellManagement.DesktopClient.Views;
using System.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Media;

namespace SellManagement.DesktopClient.ViewModels
{
    public class SellOrderViewModel : BaseViewModel, IFocusable
    {
        public SellOrderViewModel()
        {
            ShowBackdrop(false);

            GetSellOrderCommand = new AsyncCommand<object>(p => { return true; }, p => GetSellOrder(p.ToString()), ShowBackdrop);

            GetRefferenceSellOrderCommand = new AsyncCommand<object>(p => { return true; }, p => GetRefferenceSellOrder(p.ToString()), ShowBackdrop);

            SaveSellOrderCommand = new AsyncCommand<object>(p => { return true; }, p => SaveSellOrder(), ShowBackdrop);

            ClearAllCommand = new AsyncCommand<object>(p => { return true; }, p => InitForNewVoucher(), ShowBackdrop);

            OpenCustomerSearchCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                SearchCustomerViewModel searchCustomerViewModel = new SearchCustomerViewModel();
                SearchCustomerView searchCustomerView = new SearchCustomerView
                {
                    DataContext = searchCustomerViewModel
                };
                searchCustomerViewModel.LoadListCustomer();
                searchCustomerView.ShowDialog();

                if (searchCustomerViewModel.ListOfSelectdCustomer == null || searchCustomerViewModel.ListOfSelectdCustomer.Count == 0) return;

                CustomerCd = searchCustomerViewModel.ListOfSelectdCustomer[0].CustomerCd;
                CustomerName = searchCustomerViewModel.ListOfSelectdCustomer[0].Name;
            });

            OpenShippingCompanySearchCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                SearchShippingCompanyViewModel searchShippingCompanyViewModel = new SearchShippingCompanyViewModel();
                SearchShippingCompanyView searchShippingCompanyView = new SearchShippingCompanyView
                {
                    DataContext = searchShippingCompanyViewModel
                };
                searchShippingCompanyViewModel.LoadListShippingCompany();
                searchShippingCompanyView.ShowDialog();

                if (searchShippingCompanyViewModel.ListOfSelectdShippingCompany == null || searchShippingCompanyViewModel.ListOfSelectdShippingCompany.Count == 0) return;

                ShippingCompanyCd = searchShippingCompanyViewModel.ListOfSelectdShippingCompany[0].ShippingCompanyCd;
                ShippingCompanyName = searchShippingCompanyViewModel.ListOfSelectdShippingCompany[0].Name;
            });

            OpenSearchProductCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                SearchProductViewModel searchProductViewModel = new SearchProductViewModel();

                SearchProductView searchProductView = new SearchProductView()
                {
                    DataContext = searchProductViewModel
                };
                searchProductViewModel.LoadListProduct();
                searchProductView.ShowDialog();

                if (ListOfSellOrderDetail == null) ListOfSellOrderDetail = new ObservableCollection<SellOrderDetail>();
                foreach (var product in searchProductViewModel.ListOfSelectdProduct)
                {
                    if (ListOfSellOrderDetail.Where(x => x.ProductCd == product.ProductCd).Count() > 0) continue;

                    ListOfSellOrderDetail.Add(new SellOrderDetail
                    {
                        ProductCd = product.ProductCd,
                        ProductName = product.Name,
                        Quantity = 1,
                        CostPrice = product.CostPrice,
                        Cost = product.CostPrice,
                        Note = "",
                    });
                }
                OnPropertyChanged("ListOfSellOrderDetail");
                CalcPrice();
            });

            QuantityChangedCommand = new RelayCommand<object>(p => { return ListOfSellOrderDetail != null && ListOfSellOrderDetail.Count() > 0; }, p =>
            {
                CalcPrice();

                if (SelectedSellOrderDetail == null) return;

                SelectedSellOrderDetail.Cost = SelectedSellOrderDetail.CostPrice * SelectedSellOrderDetail.Quantity;
            });

            DeleteSellOrderCommand = new AsyncCommand<object>(p => { return Id != 0; }, p => DeleteSellOrder(), ShowBackdrop);

            DeleteSellOrderDetailCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                if (SelectedSellOrderDetail == null) return;
                ListOfSellOrderDetail.Remove(SelectedSellOrderDetail);
                OnPropertyChanged("ListOfSellOrderDetail");
                CalcPrice();
            });
        }

        async Task GetSellOrder(string sellOrderNo)
        {
            sellOrderNo = sellOrderNo.PadLeft(8, '0');

            if (sellOrderNo == SellOrderNo) return;

            var response = await ServiceProvider.GetInstance().CallWebApi<string, SellOrderAddResponse>("SellOrder/GetSellOrderByNo", HttpMethod.Post, sellOrderNo);
            if (response.StatusCode == 200)
            {
                if (response.SellOrderData != null)
                {
                    Id = response.SellOrderData.Id;
                    SellOrderNo = response.SellOrderData.SellOrderNo;
                    RefSellOrderNo = "";
                    SellOrderDate = response.SellOrderData.SellOrderDate;
                    PlannedExportDate = response.SellOrderData.PlannedExportDate;
                    CustomerCd = response.SellOrderData.CustomerCd;
                    CustomerName = response.SellOrderData.CustomerName;
                    ShippingCompanyCd = response.SellOrderData.ShippingCompanyCd;
                    ShippingCompanyName = response.SellOrderData.ShippingCompanyName;
                    Status = response.SellOrderData.Status;
                    ForControlStatus = response.SellOrderData.ForControlStatus;
                    SummaryCost = response.SellOrderData.SummaryCost;
                    ShippingCost = response.SellOrderData.ShippingCost;
                    SaleOffCost = response.SellOrderData.SaleOffCost;
                    PaidCost = response.SellOrderData.PaidCost;
                    SellCost = response.SellOrderData.SellCost;
                    Note = response.SellOrderData.Note;
                    WaybillVoucherNo = response.SellOrderData.WaybillVoucherNo;
                    PlannedPickingDate = response.SellOrderData.PlannedPickingDate;
                    PlannedDeliveryDate = response.SellOrderData.PlannedDeliveryDate;
                    DeliveryDate = response.SellOrderData.DeliveryDate;
                    CollectOnDeliveryCash = response.SellOrderData.CollectOnDeliveryCash;
                    WaybillCost = response.SellOrderData.WaybillCost;
                    WaybillStatus = response.SellOrderData.WaybillStatus;
                    ListOfSellOrderDetail = new ObservableCollection<SellOrderDetail>(response.SellOrderData.SellOrderDetails);
                }
                else
                {
                    await InitForNewVoucher();
                    Id = 0;
                }
            }
            else
            {
                Message = response.Message;
            }

            OnPropertyChanged("ShowNewSellOrderMark");
        }

        async Task GetRefferenceSellOrder(string sellOrderNo)
        {
            sellOrderNo = sellOrderNo.PadLeft(8, '0');

            if (sellOrderNo == RefSellOrderNo) return;

            var response = await ServiceProvider.GetInstance().CallWebApi<string, SellOrderAddResponse>("SellOrder/GetSellOrderByNo", HttpMethod.Post, sellOrderNo);
            if (response.StatusCode == 200)
            {
                if (response.SellOrderData != null)
                {
                    RefSellOrderNo = response.SellOrderData.SellOrderNo;
                    CustomerCd = response.SellOrderData.CustomerCd;
                    CustomerName = response.SellOrderData.CustomerName;
                    ShippingCompanyCd = response.SellOrderData.ShippingCompanyCd;
                    ShippingCompanyName = response.SellOrderData.ShippingCompanyName;
                    Status = -1;
                    ForControlStatus = 9;
                    SummaryCost = response.SellOrderData.SummaryCost;
                    ShippingCost = response.SellOrderData.ShippingCost;
                    SaleOffCost = response.SellOrderData.SaleOffCost;
                    PaidCost = response.SellOrderData.PaidCost;
                    SellCost = response.SellOrderData.SellCost;
                    Note = response.SellOrderData.Note;
                    WaybillVoucherNo = "";
                    PlannedPickingDate = null;
                    PlannedDeliveryDate = null;
                    DeliveryDate = null;
                    CollectOnDeliveryCash = 0;
                    WaybillCost = 0;
                    WaybillStatus = 0;
                    ListOfSellOrderDetail = new ObservableCollection<SellOrderDetail>(response.SellOrderData.SellOrderDetails);
                }
                else
                {
                    RefSellOrderNo = "";
                }
            }
            else
            {
                Message = response.Message;
            }
        }

        void CalcPrice()
        {
            SummaryCost = ListOfSellOrderDetail.Sum(x => x.CostPrice * x.Quantity);
            SellCost = SummaryCost + ShippingCost - SaleOffCost - PaidCost;
        }

        async Task InitForNewVoucher()
        { 
            if (RequestFocus != null) RequestFocus(this, new FocusEventArgs { FocusItem = "SellOrderNo" });
            RefSellOrderNo = "";
            SellOrderDate = DateTime.Now.Date;
            PlannedExportDate = DateTime.Now.Date;
            CustomerCd = "";
            CustomerName = "";
            ShippingCompanyCd = "";
            ShippingCompanyName = "";
            Status = -1;
            ForControlStatus = 9;
            ListOfSellOrderDetail = new ObservableCollection<SellOrderDetail>();
            SummaryCost = 0;
            ShippingCost = 0;
            SaleOffCost = 0;
            PaidCost = 0;
            SellCost = 0;
            Note = "";
            WaybillVoucherNo = "";
            PlannedPickingDate = null;
            PlannedDeliveryDate = null;
            DeliveryDate = null;
            CollectOnDeliveryCash = 0;
            WaybillCost = 0;
            WaybillStatus = 0;

            ShowBackdrop(true);
            var response = await ServiceProvider.GetInstance().CallWebApi<int, GetVoucherNoResponse>("VoucherNoManagement/GetVoucherNo", HttpMethod.Post, ShareContanst.VOUCHERNO_CATEGORY_SELL);
            if (response.StatusCode == 200)
            {
                SellOrderNo = response.VoucherNo;
            }
            else
            {
                Message = response.Message;
            }
            ShowBackdrop(false);
        }

        async Task SaveSellOrder()
        {
            if (MessageBox.Show("Lưu thông tin đơn hàng?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            if (CustomerCd == null || CustomerCd == "")
            {
                MessageBox.Show("Chưa nhập khách hàng", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "CustomerCd" });
                return;
            }

            if (SellOrderDate == null)
            {
                MessageBox.Show("Chưa nhập ngày đặt hàng", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "SellOrderDate" });
                return;
            }

            if (PlannedExportDate == null)
            {
                MessageBox.Show("Chưa nhập ngày dự định nhập hàng", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "PlannedExportDate" });
                return;
            }

            if (ListOfSellOrderDetail == null || ListOfSellOrderDetail.Count == 0)
            {
                MessageBox.Show("Chưa nhập sản phẩm", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "ListOfSellOrderDetail" });
                return;
            }

            var sellOrder = new SellOrder
            {
                Id = Id,
                SellOrderNo = SellOrderNo,
                SellOrderDate = SellOrderDate,
                PlannedExportDate = PlannedExportDate,
                CustomerCd = CustomerCd,
                CustomerName = CustomerName,
                ShippingCompanyCd = ShippingCompanyCd,
                ShippingCompanyName = ShippingCompanyName,
                Status = Status == -1 ? 0 : Status,
                ForControlStatus = ForControlStatus,
                SummaryCost = SummaryCost,
                ShippingCost = ShippingCost,
                SaleOffCost = SaleOffCost,
                PaidCost = PaidCost,
                SellCost = SellCost,
                Note = Note,
                WaybillVoucherNo = WaybillVoucherNo,
                PlannedPickingDate = PlannedPickingDate,
                PlannedDeliveryDate = PlannedDeliveryDate,
                DeliveryDate = DeliveryDate,
                CollectOnDeliveryCash = CollectOnDeliveryCash,
                WaybillCost = WaybillCost,
                WaybillStatus = WaybillStatus,
                SellOrderDetails = new ObservableCollection<SellOrderDetail>(ListOfSellOrderDetail),
            };
            int saveStatus;
            if (Id == 0)
            {
                var request = new SellOrderAddRequest
                {
                    SellOrderData = sellOrder
                };

                var response = await ServiceProvider.GetInstance().CallWebApi<SellOrderAddRequest, SellOrderAddResponse>("SellOrder/AddSellOrder", HttpMethod.Post, request);
                saveStatus = response.StatusCode;
                if (response.StatusCode == 200)
                { }
                else
                {
                    Message = response.Message;
                }
            }
            else
            {
                var request = new SellOrderUpdateRequest
                {
                    SellOrderData = sellOrder
                };

                var response = await ServiceProvider.GetInstance().CallWebApi<SellOrderUpdateRequest, SellOrderUpdateResponse>("SellOrder/UpdateSellOrder", HttpMethod.Post, request);
                saveStatus = response.StatusCode;
                if (response.StatusCode == 200)
                { }
                else
                {
                    Message = response.Message;
                }
            }

            if (saveStatus == 200)
            {
                MessageBox.Show("Lưu thông tin đơn hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Id = 0;

                await InitForNewVoucher();
            }
            else
                MessageBox.Show($"Lỗi! \n {Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        async Task UpdateSellOrderStatus(int status)
        {
            var request = new SellOrderUpdateStatusRequest
            {
                SellOrderNo = SellOrderNo,
                Status = status
            };

            var response = await ServiceProvider.GetInstance().CallWebApi<SellOrderUpdateStatusRequest, SellOrderUpdateStatusResponse>("SellOrder/UpdateSellOrderStatus", HttpMethod.Post, request);
            if (response.StatusCode == 200)
            {
                Status = status;

                var handler = RequestFocus;
                if (handler != null) handler(this, new FocusEventArgs { FocusItem = "SellOrderNo" });
            }
            else
            {
                Message = response.Message;
            }
        }

        async Task DeleteSellOrder()
        {
            if (Id != 0)
            {
                if (MessageBox.Show("Xóa thông tin đơn hàng?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                var response = await ServiceProvider.GetInstance().CallWebApi<string, SellOrderDeleteResponse>("SellOrder/DeleteSellOrder", HttpMethod.Post, SellOrderNo);
                if (response.StatusCode == 200)
                {

                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    Id = 0;

                    await InitForNewVoucher();

                    var handler = RequestFocus;
                    if (handler != null) handler(this, new FocusEventArgs { FocusItem = "SellOrderNo" });

                    OnPropertyChanged("ShowNewSellOrderMark");
                }
                else
                {
                    Message = response.Message;
                    MessageBox.Show($"Lỗi! \n {Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event EventHandler<FocusEventArgs> RequestFocus;

        #region "Propeties"
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }

        private string sellOrderNo;

        public string SellOrderNo
        {
            get { return sellOrderNo; }
            set
            { 
                sellOrderNo = value; 
                OnPropertyChanged();

                if (sellOrderNo == null || sellOrderNo == "")
                {
                    new Task(async () =>
                    {
                        await InitForNewVoucher();
                    }).Start();
                }
            }
        }

        private string refSellOrderNo;

        public string RefSellOrderNo
        {
            get { return refSellOrderNo; }
            set { refSellOrderNo = value; OnPropertyChanged(); }
        }


        private DateTime sellOrderDate;

        public DateTime SellOrderDate
        {
            get { return sellOrderDate; }
            set { sellOrderDate = value; OnPropertyChanged(); }
        }
        private DateTime plannedImportDate;

        public DateTime PlannedExportDate
        {
            get { return plannedImportDate; }
            set { plannedImportDate = value; OnPropertyChanged(); }
        }
        private string customerCd;

        public string CustomerCd
        {
            get { return customerCd; }
            set 
            { 
                customerCd = value; 
                OnPropertyChanged();

                if (customerCd == "") CustomerName = "";
            }
        }
        private string customerName;

        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; OnPropertyChanged(); }
        }
        private string shippingCompanyCd;

        public string ShippingCompanyCd
        {
            get { return shippingCompanyCd; }
            set
            {
                shippingCompanyCd = value;
                OnPropertyChanged();

                if (shippingCompanyCd == "") ShippingCompanyName = "";
            }
        }
        private string shippingCompanyName;

        public string ShippingCompanyName
        {
            get { return shippingCompanyName; }
            set { shippingCompanyName = value; OnPropertyChanged(); }
        }

        private int status;

        public int Status
        {
            get { return status; }
            set 
            {
                status = value;

                OnPropertyChanged();
                OnPropertyChanged("IsAllowEdit");
                OnPropertyChanged("ShowDeactiveButton");
                OnPropertyChanged("SelectedStatus");
            }
        }

        private int forControlStatus;

        public int ForControlStatus
        {
            get { return forControlStatus; }
            set
            {
                forControlStatus = value;

                if (forControlStatus != 9)
                    CollectOnDeliveryCash = SellCost;
                else
                    CollectOnDeliveryCash = 0;

                OnPropertyChanged();
                OnPropertyChanged("SelectedForControlStatus");
                OnPropertyChanged("CollectOnDeliveryCash");
            }
        }

        private int summaryCost;

        public int SummaryCost
        {
            get { return summaryCost; }
            set { summaryCost = value; OnPropertyChanged(); }
        }
        private int shippingCost;

        public int ShippingCost
        {
            get { return shippingCost; }
            set 
            { 
                shippingCost = value;
                SellCost = SummaryCost + ShippingCost - SaleOffCost - PaidCost;
                OnPropertyChanged(); 
            }
        }
        private int saleOffCost;

        public int SaleOffCost
        {
            get { return saleOffCost; }
            set 
            { 
                saleOffCost = value;
                SellCost = SummaryCost + ShippingCost - SaleOffCost - PaidCost;
                OnPropertyChanged(); 
            }
        }
        private int paidCost;
        public int PaidCost
        {
            get { return paidCost; }
            set
            {
                paidCost = value;
                SellCost = SummaryCost + ShippingCost - SaleOffCost - PaidCost;
                if (WaybillStatus != 9)
                {
                    CollectOnDeliveryCash = SellCost;
                }
                OnPropertyChanged();
            }
        }
        private int sellCost;

        public int SellCost
        {
            get { return sellCost; }
            set { sellCost = value; OnPropertyChanged(); }
        }
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; OnPropertyChanged(); }
        }

        string waybillVoucherNo;
        public string WaybillVoucherNo
        {
            get { return waybillVoucherNo; }
            set
            {
                waybillVoucherNo = value;
                OnPropertyChanged();
            }
        }

        DateTime? plannedPickingDate;
        public DateTime? PlannedPickingDate
        {
            get { return plannedPickingDate; }
            set
            {
                plannedPickingDate = value;
                OnPropertyChanged();
            }
        }
        DateTime? plannedDeliveryDate;
        public DateTime? PlannedDeliveryDate
        {
            get { return plannedDeliveryDate; }
            set
            {
                plannedDeliveryDate = value;
                OnPropertyChanged();
            }
        }
        DateTime? deliveryDate;
        public DateTime? DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                OnPropertyChanged();
            }
        }
        int collectOnDeliveryCash;
        public int CollectOnDeliveryCash
        {
            get { return collectOnDeliveryCash; }
            set
            {
                collectOnDeliveryCash = value;
                OnPropertyChanged();
            }
        }
        int waybillCost;
        public int WaybillCost
        {
            get { return waybillCost; }
            set
            {
                waybillCost = value;
                OnPropertyChanged();
            }
        }
        int waybillStatus;
        public int WaybillStatus
        {
            get { return waybillStatus; }
            set
            {
                waybillStatus = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedWaybillStatus");
            }
        }

        private ObservableCollection<SellOrderDetail> listOfSellOrderDetail;

        public ObservableCollection<SellOrderDetail> ListOfSellOrderDetail
        {
            get { return listOfSellOrderDetail; }
            set { listOfSellOrderDetail = value; OnPropertyChanged(); }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }
        public Visibility ShowNewSellOrderMark
        {
            get { return Id == 0 && SellOrderNo != null && SellOrderNo != "" ? Visibility.Visible : Visibility.Hidden; }
        }

        public bool IsAllowEdit
        {
            get
            {
                switch (Status)
                {
                    case 0:
                    case 2:
                        return true;
                    case 9:
                        return false;
                    default:
                        return true;
                }
            }
        }
        public Visibility ShowDeactiveButton
        {
            get { return Id != 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        public int SelectedStatus
        {
            get 
            {
                switch (Status)
                {
                    case 0:
                        return 0;
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                    case 9:
                        return 3;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 0:
                        Status = 0;
                        break;
                    case 1:
                        Status = 1;
                        break;
                    case 2:
                        Status = 2;
                        break;
                    case 3:
                        Status = 9;
                        break;
                    default:
                        Status = 0;
                        break;
                }

                OnPropertyChanged();
            }
        }

        public int SelectedForControlStatus
        {
            get
            {
                switch (ForControlStatus)
                {
                    case 0:
                        return 0;
                    case 2:
                        return 1;
                    case 9:
                        return 2;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 0:
                        ForControlStatus = 0;
                        break;
                    case 1:
                        ForControlStatus = 2;
                        break;
                    case 2:
                        ForControlStatus = 9;
                        break;
                    default:
                        ForControlStatus = 0;
                        break;
                }

                OnPropertyChanged();
                OnPropertyChanged("ForControlStatus");
            }
        }

        public int SelectedWaybillStatus
        {
            get
            {
                switch (WaybillStatus)
                {
                    case 0:
                        return 0;
                    case 2:
                        return 1;
                    case 9:
                        return 2;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 0:
                        WaybillStatus = 0;
                        break;
                    case 1:
                        WaybillStatus = 2;
                        break;
                    case 2:
                        WaybillStatus = 9;
                        break;
                    default:
                        WaybillStatus = 0;
                        break;
                }

                OnPropertyChanged();
            }
        }
        public SellOrderDetail SelectedSellOrderDetail { get; set; }
        #endregion

        #region "Command"
        public ICommand GetSellOrderCommand { get; set; }
        public ICommand GetRefferenceSellOrderCommand { get; set; }
        public ICommand OpenCustomerSearchCommand { get; set; }
        public ICommand OpenShippingCompanySearchCommand { get; set; }
        public ICommand SaveSellOrderCommand { get; set; }
        public ICommand SaveImportOrderCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        public ICommand OpenSearchProductCommand { get; set; }
        public ICommand QuantityChangedCommand { get; set; }
        public ICommand DeleteSellOrderDetailCommand { get; set; }
        public ICommand DeleteSellOrderCommand { get; set; }
        public ICommand DeactiveSellOrderCommand { get; set; }
        public ICommand ReactiveSellOrderCommand { get; set; }
        #endregion
    }
}
