using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.PurchaseOrder;
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
    public class PurchaseOrderViewModel : BaseViewModel, IFocusable
    {
        public PurchaseOrderViewModel()
        {
            ShowBackdrop(false);

            GetPurchaseOrderCommand = new AsyncCommand<object>(p => { return true; }, p => GetPurchaseOrder(p.ToString()), ShowBackdrop);

            GetRefferencePurchaseOrderCommand = new AsyncCommand<object>(p => { return true; }, p => GetRefferencePurchaseOrder(p.ToString()), ShowBackdrop);

            SavePurchaseOrderCommand = new AsyncCommand<object>(p => { return true; }, p => SavePurchaseOrder(), ShowBackdrop);

            ClearAllCommand = new AsyncCommand<object>(p => { return true; }, p => InitForNewVoucher(), ShowBackdrop);

            OpenSupplierSearchCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                SearchSupplierViewModel searchSupplierViewModel = new SearchSupplierViewModel();
                SearchSupplierView searchSupplierView = new SearchSupplierView
                {
                    DataContext = searchSupplierViewModel
                };
                searchSupplierViewModel.LoadListSupplier();
                searchSupplierView.ShowDialog();

                if (searchSupplierViewModel.ListOfSelectdSupplier == null || searchSupplierViewModel.ListOfSelectdSupplier.Count == 0) return;

                SupplierCd = searchSupplierViewModel.ListOfSelectdSupplier[0].SupplierCd;
                SupplierName = searchSupplierViewModel.ListOfSelectdSupplier[0].Name;
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

                if (ListOfPurchaseOrderDetail == null) ListOfPurchaseOrderDetail = new ObservableCollection<PurchaseOrderDetail>();
                foreach (var product in searchProductViewModel.ListOfSelectdProduct)
                {
                    if (ListOfPurchaseOrderDetail.Where(x => x.ProductCd == product.ProductCd).Count() > 0) continue;

                    ListOfPurchaseOrderDetail.Add(new PurchaseOrderDetail
                    {
                        ProductCd = product.ProductCd,
                        ProductName = product.Name,
                        Quantity = 1,
                        CostPrice = product.CostPrice,
                        Cost = product.CostPrice,
                        Note = "",
                    });
                }
                OnPropertyChanged("ListOfPurchaseOrderDetail");
                CalcPrice();
            });

            QuantityChangedCommand = new RelayCommand<object>(p => { return ListOfPurchaseOrderDetail != null && ListOfPurchaseOrderDetail.Count() > 0; }, p =>
            {
                CalcPrice();

                if (SelectedPurchaseOrderDetail == null) return;

                SelectedPurchaseOrderDetail.Cost = SelectedPurchaseOrderDetail.CostPrice * SelectedPurchaseOrderDetail.Quantity;
            });

            DeletePurchaseOrderCommand = new AsyncCommand<object>(p => { return Id != 0; }, p => DeletePurchaseOrder(), ShowBackdrop);

            DeletePurchaseOrderDetailCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                if (SelectedPurchaseOrderDetail == null) return;
                ListOfPurchaseOrderDetail.Remove(SelectedPurchaseOrderDetail);
                OnPropertyChanged("ListOfPurchaseOrderDetail");
                CalcPrice();
            });
        }

        async Task GetPurchaseOrder(string purchaseOrderNo)
        {
            purchaseOrderNo = purchaseOrderNo.PadLeft(8, '0');

            if (purchaseOrderNo == PurchaseOrderNo) return;

            var response = await ServiceProvider.GetInstance().CallWebApi<string, PurchaseOrderAddResponse>("PurchaseOrder/GetPurchaseOrderByNo", HttpMethod.Post, purchaseOrderNo);
            if (response.StatusCode == 200)
            {
                if (response.PurchaseOrderData != null)
                {
                    Id = response.PurchaseOrderData.Id;
                    PurchaseOrderNo = response.PurchaseOrderData.PurchaseOrderNo;
                    RefPurchaseOrderNo = "";
                    PurchaseOrderDate = response.PurchaseOrderData.PurchaseOrderDate;
                    PlannedImportDate = response.PurchaseOrderData.PlannedImportDate;
                    SupplierCd = response.PurchaseOrderData.SupplierCd;
                    SupplierName = response.PurchaseOrderData.SupplierName;
                    Status = response.PurchaseOrderData.Status;
                    SummaryCost = response.PurchaseOrderData.SummaryCost;
                    SaleOffCost = response.PurchaseOrderData.SaleOffCost;
                    PaidCost = response.PurchaseOrderData.PaidCost;
                    PurchaseCost = response.PurchaseOrderData.PurchaseCost;
                    Note = response.PurchaseOrderData.Note;
                    ListOfPurchaseOrderDetail = new ObservableCollection<PurchaseOrderDetail>(response.PurchaseOrderData.PurchaseOrderDetails);
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

            OnPropertyChanged("ShowNewPurchaseOrderMark");
        }

        async Task GetRefferencePurchaseOrder(string purchaseOrderNo)
        {
            purchaseOrderNo = purchaseOrderNo.PadLeft(8, '0');

            if (purchaseOrderNo == RefPurchaseOrderNo) return;

            var response = await ServiceProvider.GetInstance().CallWebApi<string, PurchaseOrderAddResponse>("PurchaseOrder/GetPurchaseOrderByNo", HttpMethod.Post, purchaseOrderNo);
            if (response.StatusCode == 200)
            {
                if (response.PurchaseOrderData != null)
                {
                    RefPurchaseOrderNo = response.PurchaseOrderData.PurchaseOrderNo;
                    SupplierCd = response.PurchaseOrderData.SupplierCd;
                    SupplierName = response.PurchaseOrderData.SupplierName;
                    Status = -1;
                    SummaryCost = response.PurchaseOrderData.SummaryCost;
                    SaleOffCost = response.PurchaseOrderData.SaleOffCost;
                    PaidCost = response.PurchaseOrderData.PaidCost;
                    PurchaseCost = response.PurchaseOrderData.PurchaseCost;
                    Note = response.PurchaseOrderData.Note;
                    ListOfPurchaseOrderDetail = new ObservableCollection<PurchaseOrderDetail>(response.PurchaseOrderData.PurchaseOrderDetails);
                }
                else
                {
                    RefPurchaseOrderNo = "";
                }
            }
            else
            {
                Message = response.Message;
            }
        }

        void CalcPrice()
        {
            SummaryCost = ListOfPurchaseOrderDetail.Sum(x => x.CostPrice * x.Quantity);
            PurchaseCost = SummaryCost - SaleOffCost - PaidCost;
        }

        async Task InitForNewVoucher()
        { 
            if (RequestFocus != null) RequestFocus(this, new FocusEventArgs { FocusItem = "PurchaseOrderNo" });
            RefPurchaseOrderNo = "";
            PurchaseOrderDate = DateTime.Now.Date;
            PlannedImportDate = DateTime.Now.Date;
            SupplierCd = "";
            SupplierName = "";
            Status = -1;
            ListOfPurchaseOrderDetail = new ObservableCollection<PurchaseOrderDetail>();
            SummaryCost = 0;
            SaleOffCost = 0;
            PaidCost = 0;
            PurchaseCost = 0;
            Note = "";

            ShowBackdrop(true);
            var response = await ServiceProvider.GetInstance().CallWebApi<int, GetVoucherNoResponse>("VoucherNoManagement/GetVoucherNo", HttpMethod.Post, ShareContanst.VOUCHERNO_CATEGORY_PURCHASEORDER);
            if (response.StatusCode == 200)
            {
                PurchaseOrderNo = response.VoucherNo;
            }
            else
            {
                Message = response.Message;
            }
            ShowBackdrop(false);
        }

        async Task SavePurchaseOrder()
        {
            if (MessageBox.Show("Lưu thông tin đơn hàng?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            if (SupplierCd == null || SupplierCd == "")
            {
                MessageBox.Show("Chưa nhập nhà cung cấp", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "SupplierCd" });
                return;
            }

            if (PurchaseOrderDate == null)
            {
                MessageBox.Show("Chưa nhập ngày đặt hàng", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "PurchaseOrderDate" });
                return;
            }

            if (PlannedImportDate == null)
            {
                MessageBox.Show("Chưa nhập ngày dự định nhập hàng", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "PlannedImportDate" });
                return;
            }

            if (ListOfPurchaseOrderDetail == null || ListOfPurchaseOrderDetail.Count == 0)
            {
                MessageBox.Show("Chưa nhập sản phẩm", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "ListOfPurchaseOrderDetail" });
                return;
            }

            var purchaseOrder = new PurchaseOrder
            {
                Id = Id,
                PurchaseOrderNo = PurchaseOrderNo,
                PurchaseOrderDate = PurchaseOrderDate,
                PlannedImportDate = PlannedImportDate,
                SupplierCd = SupplierCd,
                SupplierName = SupplierName,
                Status = Status == -1 ? 0 : Status,
                SummaryCost = SummaryCost,
                SaleOffCost = SaleOffCost,
                PaidCost = PaidCost,
                PurchaseCost = PurchaseCost,
                Note = Note,
                PurchaseOrderDetails = new ObservableCollection<PurchaseOrderDetail>(ListOfPurchaseOrderDetail),
            };
            int saveStatus;
            if (Id == 0)
            {
                var request = new PurchaseOrderAddRequest
                {
                    PurchaseOrderData = purchaseOrder
                };

                var response = await ServiceProvider.GetInstance().CallWebApi<PurchaseOrderAddRequest, PurchaseOrderAddResponse>("PurchaseOrder/AddPurchaseOrder", HttpMethod.Post, request);
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
                var request = new PurchaseOrderUpdateRequest
                {
                    PurchaseOrderData = purchaseOrder
                };

                var response = await ServiceProvider.GetInstance().CallWebApi<PurchaseOrderUpdateRequest, PurchaseOrderUpdateResponse>("PurchaseOrder/UpdatePurchaseOrder", HttpMethod.Post, request);
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

        async Task UpdatePurchaseOrderStatus(int status)
        {
            var request = new PurchaseOrderUpdateStatusRequest
            {
                PurchaseOrderNo = PurchaseOrderNo,
                Status = status
            };

            var response = await ServiceProvider.GetInstance().CallWebApi<PurchaseOrderUpdateStatusRequest, PurchaseOrderUpdateStatusResponse>("PurchaseOrder/UpdatePurchaseOrderStatus", HttpMethod.Post, request);
            if (response.StatusCode == 200)
            {
                Status = status;

                var handler = RequestFocus;
                if (handler != null) handler(this, new FocusEventArgs { FocusItem = "PurchaseOrderNo" });
            }
            else
            {
                Message = response.Message;
            }
        }

        async Task DeletePurchaseOrder()
        {
            if (Id != 0)
            {
                if (MessageBox.Show("Xóa thông tin đơn hàng?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                var response = await ServiceProvider.GetInstance().CallWebApi<string, PurchaseOrderDeleteResponse>("PurchaseOrder/DeletePurchaseOrder", HttpMethod.Post, PurchaseOrderNo);
                if (response.StatusCode == 200)
                {

                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    Id = 0;

                    await InitForNewVoucher();

                    var handler = RequestFocus;
                    if (handler != null) handler(this, new FocusEventArgs { FocusItem = "PurchaseOrderNo" });

                    OnPropertyChanged("ShowNewPurchaseOrderMark");
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

        private string purchaseOrderNo;

        public string PurchaseOrderNo
        {
            get { return purchaseOrderNo; }
            set
            { 
                purchaseOrderNo = value; 
                OnPropertyChanged();

                if (purchaseOrderNo == null || purchaseOrderNo == "")
                {
                    new Task(async () =>
                    {
                        await InitForNewVoucher();
                    }).Start();
                }
            }
        }

        private string refPurchaseOrderNo;

        public string RefPurchaseOrderNo
        {
            get { return refPurchaseOrderNo; }
            set { refPurchaseOrderNo = value; OnPropertyChanged(); }
        }


        private DateTime purchaseOrderDate;

        public DateTime PurchaseOrderDate
        {
            get { return purchaseOrderDate; }
            set { purchaseOrderDate = value; OnPropertyChanged(); }
        }
        private DateTime plannedImportDate;

        public DateTime PlannedImportDate
        {
            get { return plannedImportDate; }
            set { plannedImportDate = value; OnPropertyChanged(); }
        }
        private string supplierCd;

        public string SupplierCd
        {
            get { return supplierCd; }
            set 
            { 
                supplierCd = value; 
                OnPropertyChanged();

                if (supplierCd == "") SupplierName = "";
            }
        }
        private string supplierName;

        public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; OnPropertyChanged(); }
        }

        private int status;

        public int Status
        {
            get { return status; }
            set 
            {
                status = value;

                OnPropertyChanged();
                OnPropertyChanged("StatusContent");
                OnPropertyChanged("StatusColor");
                OnPropertyChanged("IsAllowEdit");
                OnPropertyChanged("ShowDeactiveButton");
                OnPropertyChanged("SelectedStatus");

            }
        }

        private int summaryCost;

        public int SummaryCost
        {
            get { return summaryCost; }
            set { summaryCost = value; OnPropertyChanged(); }
        }
        private int saleOffCost;

        public int SaleOffCost
        {
            get { return saleOffCost; }
            set 
            { 
                saleOffCost = value;
                PurchaseCost = SummaryCost - SaleOffCost - PaidCost;
                OnPropertyChanged(); 
            }
        }
        private int purchaseCost;

        public int PaidCost
        {
            get { return paidCost; }
            set
            {
                paidCost = value;
                PurchaseCost = SummaryCost - SaleOffCost - PaidCost;
                OnPropertyChanged();
            }
        }
        private int paidCost;

        public int PurchaseCost
        {
            get { return purchaseCost; }
            set { purchaseCost = value; OnPropertyChanged(); }
        }
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; OnPropertyChanged(); }
        }
        private ObservableCollection<PurchaseOrderDetail> listOfPurchaseOrderDetail;

        public ObservableCollection<PurchaseOrderDetail> ListOfPurchaseOrderDetail
        {
            get { return listOfPurchaseOrderDetail; }
            set { listOfPurchaseOrderDetail = value; OnPropertyChanged(); OnPropertyChanged("PurchaseCost"); }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }
        public Visibility ShowNewPurchaseOrderMark
        {
            get { return Id == 0 && PurchaseOrderNo != null && PurchaseOrderNo != "" ? Visibility.Visible : Visibility.Hidden; }
        }
        public string StatusContent
        {
            get
            {
                switch (Status)
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
        public SolidColorBrush StatusColor
        {
            get
            {
                switch (Status)
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
                        Status = 0;
                        break;
                    case 1:
                        Status = 2;
                        break;
                    case 2:
                        Status = 9;
                        break;
                    default:
                        Status = 0;
                        break;
                }

                OnPropertyChanged();
            }
        }
        public PurchaseOrderDetail SelectedPurchaseOrderDetail { get; set; }
        #endregion

        #region "Command"
        public ICommand GetPurchaseOrderCommand { get; set; }
        public ICommand GetRefferencePurchaseOrderCommand { get; set; }
        public ICommand OpenSupplierSearchCommand { get; set; }
        public ICommand SavePurchaseOrderCommand { get; set; }
        public ICommand SaveImportOrderCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        public ICommand OpenSearchProductCommand { get; set; }
        public ICommand QuantityChangedCommand { get; set; }
        public ICommand DeletePurchaseOrderDetailCommand { get; set; }
        public ICommand DeletePurchaseOrderCommand { get; set; }
        public ICommand DeactivePurchaseOrderCommand { get; set; }
        public ICommand ReactivePurchaseOrderCommand { get; set; }
        #endregion
    }
}
