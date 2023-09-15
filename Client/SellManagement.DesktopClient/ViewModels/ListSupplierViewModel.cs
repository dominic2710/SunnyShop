using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.Supplier;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using SellManagement.DesktopClient.Views;
using System.Windows.Controls;
using System;
using System.Windows;
using SellManagement.DesktopClient.UserControls;

namespace SellManagement.DesktopClient.ViewModels
{
    public class ListSupplierViewModel:BaseViewModel, IOpenNewAble
    {
        public ListSupplierViewModel()
        {
            new Task(async () =>
            {
                await LoadListSupplierAsync();
            }).Start();

            NewSupplierCommand = new RelayCommand<object>(p => { return true; }, p => AddSupplierAsync());

            CheckCategoryCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfCategoryForFilter, p));

            OpenSupplierInfoCommand = new RelayCommand<object>(p => { return true; }, p => UpdateSupplierAsync());
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

            OnPropertyChanged("ListOfSupplierFiltered");
        }

        void AddSupplierAsync()
        {
            SupplierViewModel viewModel = new SupplierViewModel
            {
                ListOfCategory = ListOfCategory,
            };
            SupplierUserControl supplierUserControl = new SupplierUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this,
                    new OpenEventArgs
                    {
                        OpenUserControl = supplierUserControl,
                        Header = viewModel.SupplierCd
                    });
        }

        void UpdateSupplierAsync()
        {
            if (SelectedSupplier == null) return;

            SupplierViewModel viewModel = new SupplierViewModel
            {
                Id = SelectedSupplier.Id,
                SupplierCd = SelectedSupplier.SupplierCd,
                Name = SelectedSupplier.Name,
                CategoryCd = SelectedSupplier.CategoryCd,
                Address1 = SelectedSupplier.Address1,
                Address2 = SelectedSupplier.Address2,
                PhoneNumber = SelectedSupplier.PhoneNumber,
                Email = SelectedSupplier.Email,
                Facebook = SelectedSupplier.Facebook,
                Note = SelectedSupplier.Note,
                ListOfCategory = ListOfCategory,
            };
            SupplierUserControl supplierUserControl = new SupplierUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this,
                    new OpenEventArgs
                    {
                        OpenUserControl = supplierUserControl,
                        Header = viewModel.SupplierCd
                    });
        }

        async Task LoadListSupplierAsync()
        {
            ShowBackdrop(true);
            ListOfSupplier = new ObservableCollection<Supplier>();

            var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListSupplierResponse>("Supplier/GetListSupplier", HttpMethod.Post, null);
            if (result.StatusCode == 200)
            {
                ListOfSupplier = new ObservableCollection<Supplier>(result.ListSupplier);
                ListOfCategory = new ObservableCollection<ClassifyName>(result.ListCategory);
            }

            if (ListOfSupplier == null) ListOfSupplier = new ObservableCollection<Supplier>();
            if (ListOfCategory == null) ListOfCategory = new ObservableCollection<ClassifyName>();

            ListOfCategoryForFilter = GetFilterList(ListOfCategory, ListOfCategoryForFilter);

            int lastSelectedIndex = SelectedIndex;

            OnPropertyChanged("ListOfSupplierFiltered");

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

        public event EventHandler<OpenEventArgs> RequestOpen;

        #region "Properties"
        private ObservableCollection<Supplier> listOfSupplier;
        public ObservableCollection<Supplier> ListOfSupplier
        {
            get { return listOfSupplier; }
            set { listOfSupplier = value; }
        }
        public ObservableCollection<Supplier> ListOfSupplierFiltered
        {
            get
            {
                if (ListOfSupplier == null) return new ObservableCollection<Supplier>();
                var list = ListOfSupplier
                                    .Where(x => FilterName == null || FilterName == "" || (FilterName != "" && x.Name.ToUpper().Contains(FilterName.ToUpper())))
                                    .Where(x => ListOfCategoryForFilter.Where(t => t.IsChecked).Select(t => int.Parse(t.Code)).Contains(x.CategoryCd))
                                    .ToList();

                return new ObservableCollection<Supplier>(list);
            }
        }
        public ObservableCollection<ClassifyName> ListOfCategory { get; set; }

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

        private string filterName;
        public string FilterName
        {
            get { return filterName; }
            set
            {
                filterName = value;
                OnPropertyChanged();
                OnPropertyChanged("ListOfSupplierFiltered");
            }
        }

        private Supplier selectedSupplier;

        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set { selectedSupplier = value; }
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
                        await LoadListSupplierAsync();
                    }).Start();
                }
            }
        }
        #endregion

        #region "Command"
        public ICommand NewSupplierCommand { get; set; }
        public ICommand CheckCategoryCommand { get; set; }
        public ICommand OpenSupplierInfoCommand { get; set; }

        #endregion
    }
}
