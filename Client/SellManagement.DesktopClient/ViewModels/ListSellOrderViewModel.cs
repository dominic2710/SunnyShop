using SellManagement.DesktopClient.Models;
using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Services.SellOrder;
using SellManagement.DesktopClient.Services.Customer;
using SellManagement.DesktopClient.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using SellManagement.DesktopClient.Services.ShippingCompany;

namespace SellManagement.DesktopClient.ViewModels
{
    public class ListSellOrderViewModel:BaseViewModel, IOpenNewAble
    {
        public ListSellOrderViewModel()
        {
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "-1", DisplayName = "Tất cả" });
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "0", DisplayName = "Đã đặt hàng" });
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "1", DisplayName = "Đã đóng gói" });
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "2", DisplayName = "Đã nhập hàng" });
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "9", DisplayName = "Đã hủy" });

            ListOfPlannedExportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "-1", DisplayName = "Tất cả" });
            ListOfPlannedExportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "0", DisplayName = "Hôm nay" });
            ListOfPlannedExportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "1", DisplayName = "Ngày mai" });
            ListOfPlannedExportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "2", DisplayName = "Tuần này" });
            ListOfPlannedExportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "3", DisplayName = "Tuần tới" });
            ListOfPlannedExportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "99", DisplayName = "Chọn ngày" });

            ShowBackdrop(false);

            PlannedExportDateFrom = DateTime.Now.Date;
            PlannedExportDateTo = DateTime.Now.Date;
            OnPropertyChanged("PlannedExportDateFrom");
            OnPropertyChanged("PlannedExportDateTo");

            NewSellOrderCommand = new RelayCommand<object>(p => { return true; }, p => AddSellOrder());

            CheckStatusCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfStatusForFilter, p));

            CheckCustomerCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfCustomerForFilter, p));

            CheckShippingCompanyCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfShippingCompanyForFilter, p));

            CheckPlannedExportDateCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfPlannedExportDateForFilter, p));

            OpenSellOrderInfoCommand = new RelayCommand<object>(p => { return true; }, p => UpdateSellOrder());
        }

        void UpdateFilterList(ObservableCollection<FilterItemModel> targetList, object targetCheckbox)
        {
            CheckBox checkBox = targetCheckbox as CheckBox;

            if (checkBox.Tag != null)
            {
                //if check into SelectAll
                if (checkBox.Tag.ToString() == ShareContanst.SELECT_ALL)
                {
                    if ((bool)checkBox.IsChecked)
                    {
                        foreach (var item in targetList)
                            item.IsChecked = true;
                    }
                    else
                    {
                        foreach (var item in targetList)
                            item.IsChecked = false;
                    }
                }
                else
                {
                    var item = targetList
                                    .Where(x => x.IsChecked == false)
                                    .Where(x => x.DisplayName != ShareContanst.SELECT_ALL)
                                    .FirstOrDefault();
                    //If all of item is checked
                    if (item == null)
                    {
                        targetList.Where(x => x.DisplayName == ShareContanst.SELECT_ALL).SingleOrDefault().IsChecked = true;
                    }
                    else
                    {
                        targetList.Where(x => x.DisplayName == ShareContanst.SELECT_ALL).SingleOrDefault().IsChecked = false;
                    }
                }

            }

            OnPropertyChanged("ListOfSellOrderFiltered");
        }

        void AddSellOrder()
        {
            if (RequestOpen == null) return;
            SellOrderViewModel viewModel = new SellOrderViewModel
            {
                SellOrderNo = null,
            };
            SellOrderUserControl SellOrderUserControl = new SellOrderUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this, 
                new OpenEventArgs 
                { 
                    OpenUserControl = SellOrderUserControl,
                    Header = "Đơn hàng mới"
                });
        }

        void UpdateSellOrder()
        {
            if (RequestOpen == null) return;
            if (SelectedSellOrder == null) return;

            SellOrderViewModel viewModel = new SellOrderViewModel
            {
                Id = SelectedSellOrder.Id,
                SellOrderNo = SelectedSellOrder.SellOrderNo,
                RefSellOrderNo = "",
                SellOrderDate = SelectedSellOrder.SellOrderDate,
                PlannedExportDate = SelectedSellOrder.PlannedExportDate,
                CustomerCd = SelectedSellOrder.CustomerCd,
                CustomerName = SelectedSellOrder.CustomerName,
                ShippingCompanyCd = SelectedSellOrder.ShippingCompanyCd,
                ShippingCompanyName = SelectedSellOrder.ShippingCompanyName,
                Status = SelectedSellOrder.Status,
                ForControlStatus = SelectedSellOrder.ForControlStatus,
                SummaryCost = SelectedSellOrder.SummaryCost,
                ShippingCost = SelectedSellOrder.ShippingCost,
                SaleOffCost = SelectedSellOrder.SaleOffCost,
                PaidCost = SelectedSellOrder.PaidCost,
                SellCost = SelectedSellOrder.SellCost,
                Note = SelectedSellOrder.Note,
                WaybillVoucherNo = SelectedSellOrder.WaybillVoucherNo,
                PlannedPickingDate = SelectedSellOrder.PlannedPickingDate,
                PlannedDeliveryDate = SelectedSellOrder.PlannedDeliveryDate,
                DeliveryDate = SelectedSellOrder.DeliveryDate,
                CollectOnDeliveryCash = SelectedSellOrder.CollectOnDeliveryCash,
                WaybillCost = SelectedSellOrder.WaybillCost,
                WaybillStatus = SelectedSellOrder.WaybillStatus,
                ListOfSellOrderDetail = new ObservableCollection<SellOrderDetail>(SelectedSellOrder.SellOrderDetails)
            };
            SellOrderUserControl SellOrderUserControl = new SellOrderUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this,
                new OpenEventArgs
                {
                    OpenUserControl = SellOrderUserControl,
                    Header = viewModel.SellOrderNo
                });
        }

        public async Task LoadListSellOrderAsync()
        {
            ShowBackdrop(true);
            ListOfSellOrder = new ObservableCollection<SellOrder>();

            var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListSellOrderResponse>("SellOrder/GetListSellOrder", HttpMethod.Post, null);
            if (result.StatusCode == 200)
            {
                ListOfSellOrder = new ObservableCollection<SellOrder>(result.ListSellOrder);
            }

            int lastSelectedIndex = SelectedIndex;

            OnPropertyChanged("ListOfSellOrderFiltered");

            SelectedIndex = lastSelectedIndex;
            OnPropertyChanged("SelectedIndex");
            ShowBackdrop(false);
        }

        #region "Properties"
        public event EventHandler<OpenEventArgs> RequestOpen;

        private ObservableCollection<SellOrder> listOfSellOrder;
        public ObservableCollection<SellOrder> ListOfSellOrder
        {
            get { return listOfSellOrder; }
            set { listOfSellOrder = value; }
        }
        public ObservableCollection<SellOrder> ListOfSellOrderFiltered
        {
            get
            {
                if (ListOfSellOrder == null) return new ObservableCollection<SellOrder>();

                var listOfFilterDate = new List<DateTime>();

                if (ListOfPlannedExportDateForFilter.Where(t => t.IsChecked).Count() == 0)
                    listOfFilterDate.Add(DateTime.MaxValue);
                else
                {
                    if (ListOfPlannedExportDateForFilter.Where(t => t.Code == "-1").FirstOrDefault().IsChecked == false)
                    {
                        DateTime thisWeekMonday;
                        foreach (var item in ListOfPlannedExportDateForFilter.Where(t => t.IsChecked))
                        {
                            switch (item.Code)
                            {
                                case "0":
                                    listOfFilterDate.Add(DateTime.Now.Date);
                                    break;
                                case "1":
                                    listOfFilterDate.Add(DateTime.Now.AddDays(1).Date);
                                    break;
                                case "2":
                                    thisWeekMonday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                                    for (int i = 0; i < 7; i++)
                                    {
                                        listOfFilterDate.Add(thisWeekMonday.AddDays(i).Date);
                                    }
                                    break;
                                case "3":
                                    thisWeekMonday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                                    for (int i = 7; i < 14; i++)
                                    {
                                        listOfFilterDate.Add(thisWeekMonday.AddDays(i).Date);
                                    }
                                    break;
                                case "99":
                                    if (PlannedExportDateFrom > PlannedExportDateTo)
                                        listOfFilterDate.Add(DateTime.MaxValue);
                                    else
                                    {
                                        var date = PlannedExportDateFrom;
                                        while (date <= PlannedExportDateTo)
                                        {
                                            listOfFilterDate.Add(date);
                                            date = date.AddDays(1);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                var list = ListOfSellOrder
                    .Where(x => ListOfStatusForFilter.Where(t => t.IsChecked).Select(t => int.Parse(t.Code)).Contains(x.Status))
                    .Where(x => ListOfCustomerForFilter.Where(t => t.IsChecked).Select(t => t.Code).Contains(x.CustomerCd))
                    .Where(x => ListOfShippingCompanyForFilter.Where(t => t.IsChecked).Select(t => t.Code).Contains(x.ShippingCompanyCd))
                    .Where(x => (listOfFilterDate.Count == 0) || (listOfFilterDate.Count > 0 && listOfFilterDate.Contains(x.PlannedExportDate.Date)))
                    .ToList();

                return new ObservableCollection<SellOrder>(list);
            }
        }

        private ObservableCollection<FilterItemModel> listOfStatusForFilter;
        public ObservableCollection<FilterItemModel> ListOfStatusForFilter
        {
            get
            {
                if (listOfStatusForFilter == null)
                    listOfStatusForFilter = new ObservableCollection<FilterItemModel>();

                return listOfStatusForFilter;
            }
            set
            {
                listOfStatusForFilter = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FilterItemModel> listOfCustomerForFilter;
        public ObservableCollection<FilterItemModel> ListOfCustomerForFilter
        {
            get
            {
                if (listOfCustomerForFilter == null)
                    listOfCustomerForFilter = new ObservableCollection<FilterItemModel>();

                return listOfCustomerForFilter;
            }
            set
            {
                listOfCustomerForFilter = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FilterItemModel> listOfShippingCompanyForFilter;
        public ObservableCollection<FilterItemModel> ListOfShippingCompanyForFilter
        {
            get
            {
                if (listOfShippingCompanyForFilter == null)
                    listOfShippingCompanyForFilter = new ObservableCollection<FilterItemModel>();

                return listOfShippingCompanyForFilter;
            }
            set
            {
                listOfShippingCompanyForFilter = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FilterItemModel> listOfPlannedInputDateForFilter;
        public ObservableCollection<FilterItemModel> ListOfPlannedExportDateForFilter
        {
            get
            {
                if (listOfPlannedInputDateForFilter == null)
                    listOfPlannedInputDateForFilter = new ObservableCollection<FilterItemModel>();

                return listOfPlannedInputDateForFilter;
            }
            set
            {
                listOfPlannedInputDateForFilter = value;
                OnPropertyChanged();
            }
        }

        private SellOrder selectedSellOrder;

        public SellOrder SelectedSellOrder
        {
            get { return selectedSellOrder; }
            set { selectedSellOrder = value; }
        }

        public int SelectedIndex { get; set; }

        public bool IsVisble
        {
            set
            {
                if (value == true)
                {
                    new Task(async () =>
                    {
                        ShowBackdrop(true);
                        //Customer
                        var listOfCustomer = new ObservableCollection<Customer>();
                        var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListCustomerResponse>("Customer/GetListCustomer", HttpMethod.Post, null);
                        if (result.StatusCode == 200)
                        {
                            listOfCustomer = new ObservableCollection<Customer>(result.ListCustomer);
                        }
                        listOfCustomer.Add(new Customer { Id = -1, CustomerCd = "-1", Name = "Tất cả" });

                        ListOfCustomerForFilter = new ObservableCollection<FilterItemModel>(listOfCustomer.OrderBy(t => t.Id).Select(t => new FilterItemModel
                        {
                            IsChecked = true,
                            Code = t.CustomerCd,
                            DisplayName = t.Name
                        }));
                        //Shipping Company
                        var listOfShippingCompany = new ObservableCollection<ShippingCompany>();
                        var result2 = await ServiceProvider.GetInstance().CallWebApi<object, GetListShippingCompanyResponse>("ShippingCompany/GetListShippingCompany", HttpMethod.Post, null);
                        if (result2.StatusCode == 200)
                        {
                            listOfShippingCompany = new ObservableCollection<ShippingCompany>(result2.ListShippingCompany);
                        }
                        listOfShippingCompany.Add(new ShippingCompany { Id = -1, ShippingCompanyCd = "-1", Name = "Tất cả" });
                        listOfShippingCompany.Add(new ShippingCompany { Id = 0, ShippingCompanyCd = "", Name = "Chưa nhập" });

                        ListOfShippingCompanyForFilter = new ObservableCollection<FilterItemModel>(listOfShippingCompany.OrderBy(t => t.Id).Select(t => new FilterItemModel
                        {
                            IsChecked = true,
                            Code = t.ShippingCompanyCd,
                            DisplayName = t.Name
                        }));
                        ShowBackdrop(true);

                        await LoadListSellOrderAsync();
                    }).Start();
                }
            }
        }

        private DateTime plannedImportDateFrom;
        public DateTime PlannedExportDateFrom 
        { 
            get 
            { 
                return plannedImportDateFrom; 
            }
            set
            {
                plannedImportDateFrom = value;
                OnPropertyChanged();
                OnPropertyChanged("ListOfSellOrderFiltered");
            }
        }

        private DateTime plannedImportDateTo;
        public DateTime PlannedExportDateTo
        {
            get
            {
                return plannedImportDateTo;
            }
            set
            {
                plannedImportDateTo = value;
                OnPropertyChanged();
                OnPropertyChanged("ListOfSellOrderFiltered");
            }
        }
        #endregion

        #region "Command"
        public ICommand NewSellOrderCommand { get; set; }
        public ICommand CheckCustomerCommand { get; set; }
        public ICommand CheckShippingCompanyCommand { get; set; }
        public ICommand CheckStatusCommand { get; set; }
        public ICommand CheckPlannedExportDateCommand { get; set; }
        public ICommand OpenSellOrderInfoCommand { get; set; }
        #endregion

    }
}

