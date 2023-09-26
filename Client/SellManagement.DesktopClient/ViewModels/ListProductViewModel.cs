using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.Product;
using System.Collections.ObjectModel;
using System.Linq;
using SellManagement.DesktopClient.Views;
using System.Windows.Controls;
using System;
using System.Windows;
using SellManagement.DesktopClient.UserControls;

namespace SellManagement.DesktopClient.ViewModels
{
    public class ListProductViewModel:BaseViewModel, IOpenNewAble
    {
        public ListProductViewModel()
        {
            new Task(async () =>
            {
                await LoadListProductAsync();
            }).Start();

            NewProductCommand = new RelayCommand<object>(p => { return !IsUseForSearchWindow; }, p => AddProductAsync());

            CheckCategoryCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfCategoryForFilter, p));

            CheckTradeMarkCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfTradeMarkForFilter, p));

            CheckOriginCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfOriginForFilter, p));

            OpenProductInfoCommand = new RelayCommand<object>(p => { return !IsUseForSearchWindow; }, p => UpdateProductAsync());
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

            OnPropertyChanged("ShowFilterCategoryIndicator");
            OnPropertyChanged("HideFilterCategoryIndicator");
            OnPropertyChanged("ShowFilterTradeMarkIndicator");
            OnPropertyChanged("HideFilterTradeMarkIndicator");
            OnPropertyChanged("ListOfProductFiltered");
        }

        bool IsCheckedAll(ObservableCollection<FilterItemModel> targetList)
        {
            var item = targetList
                        .Where(x => x.IsChecked == false)
                        .Where(x => x.DisplayName != ShareContanst.SELECT_ALL)
                        .FirstOrDefault();
            return item == null;
        }

        void AddProductAsync()
        {
            if (RequestOpen == null) return;
            ProductViewModel viewModel = new ProductViewModel
            {
                ListOfCategory = ListOfCategory,
                ListOfTradeMark = ListOfTradeMark,
                ListOfOrigin = ListOfOrigin
            };
            ProductUserControl productUserControl = new ProductUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this,
                new OpenEventArgs
                {
                    OpenUserControl = productUserControl,
                    Header = "Sản phẩm mới"
                });
        }

        void UpdateProductAsync()
        {
            if (RequestOpen == null) return;
            if (SelectedProduct == null) return;

            ProductViewModel viewModel = new ProductViewModel
            {
                Id = SelectedProduct.Id,
                ProductCd = SelectedProduct.ProductCd,
                Name = SelectedProduct.Name,
                Barcode = SelectedProduct.Barcode,
                CategoryCd = SelectedProduct.CategoryCd,
                TradeMarkCd = SelectedProduct.TradeMarkCd,
                OriginCd = SelectedProduct.OriginCd,
                CostPrice = SelectedProduct.CostPrice,
                SoldPrice = SelectedProduct.SoldPrice,
                Detail = SelectedProduct.Detail,
                ListOfCategory = ListOfCategory,
                ListOfTradeMark = ListOfTradeMark,
                ListOfOrigin = ListOfOrigin
            };
            ProductUserControl productUserControl = new ProductUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this,
                new OpenEventArgs
                {
                    OpenUserControl = productUserControl,
                    Header = "Sản phẩm mới"
                });
        }

        async Task LoadListProductAsync()
        {
            ShowBackdrop(true);
            ListOfProduct = new ObservableCollection<Product>();

            var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListProductResponse>("Product/GetListProduct", HttpMethod.Post, null);
            if (result.StatusCode == 200)
            {
                ListOfProduct = new ObservableCollection<Product>(result.ListProduct);
                ListOfCategory = new ObservableCollection<ClassifyName>(result.ListCategory);
                ListOfTradeMark = new ObservableCollection<ClassifyName>(result.ListTradeMark);
                ListOfOrigin = new ObservableCollection<ClassifyName>(result.ListOrigin);


                ListOfCategoryForFilter = GetFilterList(ListOfCategory, ListOfCategoryForFilter);
                ListOfTradeMarkForFilter = GetFilterList(ListOfTradeMark, ListOfTradeMarkForFilter);
                ListOfOriginForFilter = GetFilterList(ListOfOrigin, ListOfOriginForFilter);
            }

            int lastSelectedIndex = SelectedIndex;

            OnPropertyChanged("ListOfProductFiltered");

            SelectedIndex = lastSelectedIndex;
            OnPropertyChanged("SelectedIndex");
            ShowBackdrop(false);
        }

        ObservableCollection<FilterItemModel> GetFilterList(ObservableCollection<ClassifyName> newList, ObservableCollection<FilterItemModel> targetFilterList)
        {
            var list = newList
                .GroupJoin(targetFilterList,
                            a => new { Code = a.Code },
                            b => new { Code = int.Parse(b.Code )},
                            (a, b) => new { NewList = a, CurrentList = b })
                .SelectMany
                (
                    x => x.CurrentList.DefaultIfEmpty(),
                    (o, i) => new FilterItemModel
                    {
                        IsChecked = i == null ? true : i.IsChecked,
                        Code = o.NewList.Code.ToString(),
                        DisplayName = o.NewList.Name
                    }

                )
                .OrderBy(x => x.Code)
                .ToList();

            var resultList = new ObservableCollection<FilterItemModel>(list);

            var uncheckedItem = resultList.Where(x => x.IsChecked == false).FirstOrDefault();
            //If all of item is checked
            if (uncheckedItem == null)
            {
                resultList.Insert(0, new FilterItemModel
                {
                    IsChecked = true,
                    Code = "0",
                    DisplayName = ShareContanst.SELECT_ALL
                });
            }
            else
            {
                resultList.Insert(0, new FilterItemModel
                {
                    IsChecked = false,
                    Code = "0",
                    DisplayName = ShareContanst.SELECT_ALL
                });
            }
            return resultList;
        }


        #region "Properties"
        public event EventHandler<OpenEventArgs> RequestOpen;
        private ObservableCollection<Product> listOfProduct;
        public ObservableCollection<Product> ListOfProduct
        {
            get { return listOfProduct; }
            set { listOfProduct = value; }
        }
        public ObservableCollection<Product> ListOfProductFiltered
        {
            get 
            {
                if (ListOfProduct == null) return new ObservableCollection<Product>();
                var list = ListOfProduct
                                    .Where(x=> FilterName == null || FilterName == "" || (FilterName != "" && x.Name.ToUpper().Contains(FilterName.ToUpper())))
                                    .Where(x => ListOfCategoryForFilter.Where(t => t.IsChecked).Select(t => int.Parse(t.Code)).Contains(x.CategoryCd))
                                    .Where(x => ListOfTradeMarkForFilter.Where(t => t.IsChecked).Select(t => int.Parse(t.Code)).Contains(x.TradeMarkCd))
                                    .Where(x => ListOfOriginForFilter.Where(t => t.IsChecked).Select(t => int.Parse(t.Code)).Contains(x.OriginCd))
                                    .ToList();

                return new ObservableCollection<Product>(list);
            }
        }
        public ObservableCollection<ClassifyName> ListOfCategory { get; set; }
        public ObservableCollection<ClassifyName> ListOfTradeMark { get; set; }
        public ObservableCollection<ClassifyName> ListOfOrigin { get; set; }

        private ObservableCollection<FilterItemModel> listOfCategoryForFilter;
        public ObservableCollection<FilterItemModel> ListOfCategoryForFilter 
        {
            get
            {
                if (listOfCategoryForFilter == null)
                    listOfCategoryForFilter = new ObservableCollection<FilterItemModel>();

                return listOfCategoryForFilter;
            }
            set
            {
                listOfCategoryForFilter = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FilterItemModel> listOfTradeMarkForFilter;
        public ObservableCollection<FilterItemModel> ListOfTradeMarkForFilter
        {
            get
            {
                if (listOfTradeMarkForFilter == null)
                    return new ObservableCollection<FilterItemModel>();

                return listOfTradeMarkForFilter;
            }
            set
            {
                listOfTradeMarkForFilter = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FilterItemModel> listOfOriginForFilter;
        public ObservableCollection<FilterItemModel> ListOfOriginForFilter
        {
            get
            {
                if (listOfOriginForFilter == null)
                    return new ObservableCollection<FilterItemModel>();

                return listOfOriginForFilter;
            }
            set
            {
                listOfOriginForFilter = value;
                OnPropertyChanged();
            }
        }

        private string filterName;
        public string FilterName
        {
            get { return filterName; }
            set 
            { 
                filterName = value; 
                OnPropertyChanged();
                OnPropertyChanged("ListOfProductFiltered");
            }
        }

        private Product selectedProduct;

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; }
        }

        public int SelectedIndex { get; set; }

        private bool isUseForSearchWindow;

        public bool IsUseForSearchWindow
        {
            get { return isUseForSearchWindow; }
            set { isUseForSearchWindow = value; OnPropertyChanged(); }
        }

        public Visibility IsDisplay 
        {
            get { return IsUseForSearchWindow ? Visibility.Collapsed : Visibility.Visible; }
        }

        public bool IsVisble
        {
            set
            {
                if (value == true)
                {
                    new Task(async () =>
                    {
                        await LoadListProductAsync();
                    }).Start();
                }
            }
        }

        public Visibility ShowFilterCategoryIndicator
        {
            get { return IsCheckedAll(ListOfCategoryForFilter) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility HideFilterCategoryIndicator
        {
            get { return IsCheckedAll(ListOfCategoryForFilter) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility ShowFilterTradeMarkIndicator
        {
            get { return IsCheckedAll(ListOfTradeMarkForFilter) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility HideFilterTradeMarkIndicator
        {
            get { return IsCheckedAll(ListOfTradeMarkForFilter) ? Visibility.Collapsed : Visibility.Visible; }
        }

        #endregion

        #region "Command"
        public ICommand NewProductCommand { get; set; }
        public ICommand CheckCategoryCommand { get; set; }
        public ICommand CheckTradeMarkCommand { get; set; }
        public ICommand CheckOriginCommand { get; set; }
        public ICommand OpenProductInfoCommand { get; set; }

        #endregion

    }
}

