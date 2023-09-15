using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.Product;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using SellManagement.DesktopClient.Views;
using System.Windows.Controls;
using System;
using System.Windows;

namespace SellManagement.DesktopClient.ViewModels
{
    public class SearchProductViewModel : BaseViewModel, ICloseable
    {

        public SearchProductViewModel()
        {
            ListOfSelectdProduct = new ObservableCollection<Product>();

            CheckCategoryCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfCategoryForFilter, p));

            CheckTradeMarkCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfTradeMarkForFilter, p));

            CheckOriginCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfOriginForFilter, p));

            ConfirmedCommand = new RelayCommand<object>(p => { return true; }, p =>
             {
                 ListOfSelectdProduct = new ObservableCollection<Product>(ListOfProductFiltered.Where(x => x.IsChecked).ToList());

                 var handler = RequestClose;
                 if (handler != null) handler(this, EventArgs.Empty);
             });

            CancelCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ListOfSelectdProduct = new ObservableCollection<Product>();

                var handler = RequestClose;
                if (handler != null) handler(this, EventArgs.Empty);
            });

            SelectAndReturnProductCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ListOfSelectdProduct = new ObservableCollection<Product>();
                ListOfSelectdProduct.Add(SelectedProduct);

                var handler = RequestClose;
                if (handler != null) handler(this, EventArgs.Empty);
            });

            SelectProductCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                if (SelectedProduct == null) return;
                SelectedProduct.IsChecked = !SelectedProduct.IsChecked;

                var lastSelectedProduct = SelectedProduct;
                OnPropertyChanged("ListOfProductFiltered");

                SelectedProduct = lastSelectedProduct;
                OnPropertyChanged("SelectedProduct");
            });
        }

        public event EventHandler<EventArgs> RequestClose;

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
            OnPropertyChanged("ListOfProductFiltered");
        }

        public void LoadListProduct()
        {
            new Task(async () =>
            {
                await LoadListProductAsync();
            }).Start();
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

                //ListOfCategory.Remove(ListOfCategory.Where(x => x.GroupId == ShareContanst.GROUPID_CATEGORY && x.Code == ShareContanst.CATEGORY_COMBO).SingleOrDefault());
                ListOfCategoryForFilter = GetFilterList(ListOfCategory, ListOfCategoryForFilter);
                ListOfTradeMarkForFilter = GetFilterList(ListOfTradeMark, ListOfTradeMarkForFilter);
                ListOfOriginForFilter = GetFilterList(ListOfOrigin, ListOfOriginForFilter);

            }

            OnPropertyChanged("ListOfProductFiltered");
            ShowBackdrop(false);
        }

        ObservableCollection<FilterItemModel> GetFilterList(ObservableCollection<ClassifyName> newList, ObservableCollection<FilterItemModel> targetFilterList)
        {
            var list = newList
                .GroupJoin(targetFilterList,
                            a => new { Code = a.Code },
                            b => new { Code = int.Parse(b.Code) },
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
                .OrderBy(x => x.DisplayName)
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
                                    .Where(x => FilterName == null || FilterName == "" || (FilterName != "" && x.Name.ToUpper().Contains(FilterName.ToUpper())))
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
        public ObservableCollection<Product> ListOfSelectdProduct { get; set; }
        #endregion

        #region "Command"
        public ICommand CheckCategoryCommand { get; set; }
        public ICommand CheckTradeMarkCommand { get; set; }
        public ICommand CheckOriginCommand { get; set; }
        public ICommand ConfirmedCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SelectAndReturnProductCommand { get; set; }
        public ICommand SelectProductCommand { get; set; }
        #endregion
    }
}
