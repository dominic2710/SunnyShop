using SellManagement.DesktopClient.Models;
using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Services.PurchaseOrder;
using SellManagement.DesktopClient.Services.Supplier;
using SellManagement.DesktopClient.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SellManagement.DesktopClient.ViewModels
{
    public class ListPurchaseOrderViewModel:BaseViewModel, IOpenNewAble
    {
        public ListPurchaseOrderViewModel()
        {
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "-1", DisplayName = "Tất cả" });
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "0", DisplayName = "Đã đặt hàng" });
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "2", DisplayName = "Đã nhập hàng" });
            ListOfStatusForFilter.Add(new FilterItemModel { IsChecked = true, Code = "9", DisplayName = "Đã hủy" });

            ListOfPlannedImportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "-1", DisplayName = "Tất cả" });
            ListOfPlannedImportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "0", DisplayName = "Hôm nay" });
            ListOfPlannedImportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "1", DisplayName = "Ngày mai" });
            ListOfPlannedImportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "2", DisplayName = "Tuần này" });
            ListOfPlannedImportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "3", DisplayName = "Tuần tới" });
            ListOfPlannedImportDateForFilter.Add(new FilterItemModel { IsChecked = true, Code = "99", DisplayName = "Chọn ngày" });

            //ShowBackdrop(true);

            //new Task(async () =>
            //{
                
            //}).Start();
            
            //new Task(async () =>
            //{
            //    await LoadListPurchaseOrderAsync();
            //}).Start();

            ShowBackdrop(false);

            PlannedImportDateFrom = DateTime.Now.Date;
            PlannedImportDateTo = DateTime.Now.Date;
            OnPropertyChanged("PlannedImportDateFrom");
            OnPropertyChanged("PlannedImportDateTo");

            NewPurchaseOrderCommand = new RelayCommand<object>(p => { return true; }, p => AddPurchaseOrder());

            CheckStatusCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfStatusForFilter, p));

            CheckSupplierCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfSupplierForFilter, p));

            CheckPlannedImportDateCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfPlannedImportDateForFilter, p));

            OpenPurchaseOrderInfoCommand = new RelayCommand<object>(p => { return true; }, p => UpdatePurchaseOrder());
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

            OnPropertyChanged("ListOfPurchaseOrderFiltered");
        }

        void AddPurchaseOrder()
        {
            if (RequestOpen == null) return;
            PurchaseOrderViewModel viewModel = new PurchaseOrderViewModel
            {
                PurchaseOrderNo = null,
            };
            PurchaseOrderUserControl purchaseOrderUserControl = new PurchaseOrderUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this, 
                new OpenEventArgs 
                { 
                    OpenUserControl = purchaseOrderUserControl,
                    Header = "Đơn hàng mới"
                });
        }

        void UpdatePurchaseOrder()
        {
            if (RequestOpen == null) return;
            if (SelectedPurchaseOrder == null) return;

            PurchaseOrderViewModel viewModel = new PurchaseOrderViewModel
            {
                Id = SelectedPurchaseOrder.Id,
                PurchaseOrderNo = SelectedPurchaseOrder.PurchaseOrderNo,
                RefPurchaseOrderNo = "",
                PurchaseOrderDate = SelectedPurchaseOrder.PurchaseOrderDate,
                PlannedImportDate = SelectedPurchaseOrder.PlannedImportDate,
                SupplierCd = SelectedPurchaseOrder.SupplierCd,
                SupplierName = SelectedPurchaseOrder.SupplierName,
                Status = SelectedPurchaseOrder.Status,
                SummaryCost = SelectedPurchaseOrder.SummaryCost,
                SaleOffCost = SelectedPurchaseOrder.SaleOffCost,
                PaidCost = SelectedPurchaseOrder.PaidCost,
                PurchaseCost = SelectedPurchaseOrder.PurchaseCost,
                Note = SelectedPurchaseOrder.Note,
                ListOfPurchaseOrderDetail = new ObservableCollection<PurchaseOrderDetail>(SelectedPurchaseOrder.PurchaseOrderDetails)
            };
            PurchaseOrderUserControl purchaseOrderUserControl = new PurchaseOrderUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this,
                new OpenEventArgs
                {
                    OpenUserControl = purchaseOrderUserControl,
                    Header = viewModel.PurchaseOrderNo
                });
        }

        public async Task LoadListPurchaseOrderAsync()
        {
            ShowBackdrop(true);
            ListOfPurchaseOrder = new ObservableCollection<PurchaseOrder>();

            var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListPurchaseOrderResponse>("PurchaseOrder/GetListPurchaseOrder", HttpMethod.Post, null);
            if (result.StatusCode == 200)
            {
                ListOfPurchaseOrder = new ObservableCollection<PurchaseOrder>(result.ListPurchaseOrder);
            }

            int lastSelectedIndex = SelectedIndex;

            OnPropertyChanged("ListOfPurchaseOrderFiltered");

            SelectedIndex = lastSelectedIndex;
            OnPropertyChanged("SelectedIndex");
            ShowBackdrop(false);
        }

        #region "Properties"
        public event EventHandler<OpenEventArgs> RequestOpen;

        private ObservableCollection<PurchaseOrder> listOfPurchaseOrder;
        public ObservableCollection<PurchaseOrder> ListOfPurchaseOrder
        {
            get { return listOfPurchaseOrder; }
            set { listOfPurchaseOrder = value; }
        }
        public ObservableCollection<PurchaseOrder> ListOfPurchaseOrderFiltered
        {
            get
            {
                if (ListOfPurchaseOrder == null) return new ObservableCollection<PurchaseOrder>();

                var listOfFilterDate = new List<DateTime>();

                if (ListOfPlannedImportDateForFilter.Where(t => t.IsChecked).Count() == 0)
                    listOfFilterDate.Add(DateTime.MaxValue);
                else
                {
                    if (ListOfPlannedImportDateForFilter.Where(t => t.Code == "-1").FirstOrDefault().IsChecked == false)
                    {
                        DateTime thisWeekMonday;
                        foreach (var item in ListOfPlannedImportDateForFilter.Where(t => t.IsChecked))
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
                                    if (PlannedImportDateFrom > PlannedImportDateTo)
                                        listOfFilterDate.Add(DateTime.MaxValue);
                                    else
                                    {
                                        var date = PlannedImportDateFrom;
                                        while (date <= PlannedImportDateTo)
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

                var list = ListOfPurchaseOrder
                    .Where(x => ListOfStatusForFilter.Where(t => t.IsChecked).Select(t => int.Parse(t.Code)).Contains(x.Status))
                    .Where(x => ListOfSupplierForFilter.Where(t => t.IsChecked).Select(t => t.Code).Contains(x.SupplierCd))
                    .Where(x => (listOfFilterDate.Count == 0) || (listOfFilterDate.Count > 0 && listOfFilterDate.Contains(x.PlannedImportDate.Date)))
                    .ToList();

                return new ObservableCollection<PurchaseOrder>(list);
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

        private ObservableCollection<FilterItemModel> listOfSupplierForFilter;
        public ObservableCollection<FilterItemModel> ListOfSupplierForFilter
        {
            get
            {
                if (listOfSupplierForFilter == null)
                    listOfSupplierForFilter = new ObservableCollection<FilterItemModel>();

                return listOfSupplierForFilter;
            }
            set
            {
                listOfSupplierForFilter = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FilterItemModel> listOfPlannedInputDateForFilter;
        public ObservableCollection<FilterItemModel> ListOfPlannedImportDateForFilter
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

        private PurchaseOrder selectedPurchaseOrder;

        public PurchaseOrder SelectedPurchaseOrder
        {
            get { return selectedPurchaseOrder; }
            set { selectedPurchaseOrder = value; }
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
                        var listOfSupplier = new ObservableCollection<Supplier>();
                        var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListSupplierResponse>("Supplier/GetListSupplier", HttpMethod.Post, null);
                        if (result.StatusCode == 200)
                        {
                            listOfSupplier = new ObservableCollection<Supplier>(result.ListSupplier);
                        }
                        listOfSupplier.Add(new Supplier { Id = -1, SupplierCd = "-1", Name = "Tất cả" });

                        ListOfSupplierForFilter = new ObservableCollection<FilterItemModel>(listOfSupplier.OrderBy(t => t.Id).Select(t => new FilterItemModel
                        {
                            IsChecked = true,
                            Code = t.SupplierCd,
                            DisplayName = t.Name
                        }));

                        await LoadListPurchaseOrderAsync();
                    }).Start();
                }
            }
        }

        private DateTime plannedImportDateFrom;
        public DateTime PlannedImportDateFrom 
        { 
            get 
            { 
                return plannedImportDateFrom; 
            }
            set
            {
                plannedImportDateFrom = value;
                OnPropertyChanged();
                OnPropertyChanged("ListOfPurchaseOrderFiltered");
            }
        }

        private DateTime plannedImportDateTo;
        public DateTime PlannedImportDateTo
        {
            get
            {
                return plannedImportDateTo;
            }
            set
            {
                plannedImportDateTo = value;
                OnPropertyChanged();
                OnPropertyChanged("ListOfPurchaseOrderFiltered");
            }
        }
        #endregion

        #region "Command"
        public ICommand NewPurchaseOrderCommand { get; set; }
        public ICommand CheckSupplierCommand { get; set; }
        public ICommand CheckStatusCommand { get; set; }
        public ICommand CheckPlannedImportDateCommand { get; set; }
        public ICommand OpenPurchaseOrderInfoCommand { get; set; }
        #endregion

    }
}

