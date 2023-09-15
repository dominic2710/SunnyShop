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

namespace SellManagement.DesktopClient.ViewModels
{
    public class SearchSupplierViewModel:BaseViewModel, ICloseable
    {
        public SearchSupplierViewModel()
        {
            CheckCategoryCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfCategoryForFilter, p));

            SelectAndReturnProductCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ListOfSelectdSupplier = new ObservableCollection<Supplier>();
                ListOfSelectdSupplier.Add(SelectedSupplier);

                var handler = RequestClose;
                if (handler != null) handler(this, EventArgs.Empty);
            });

            CancelCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ListOfSelectdSupplier = new ObservableCollection<Supplier>();

                var handler = RequestClose;
                if (handler != null) handler(this, EventArgs.Empty);
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

            OnPropertyChanged("ListOfSupplierFiltered");
        }

        public void LoadListSupplier()
        {
            new Task(async () => await LoadListSupplierAsync()).Start();
        }

        public async Task LoadListSupplierAsync()
        {
            ShowBackdrop(true);
            ListOfSupplier = new ObservableCollection<Supplier>();

            var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListSupplierResponse>("Supplier/GetListSupplier", HttpMethod.Post, null);
            if (result.StatusCode == 200)
            {
                ListOfSupplier = new ObservableCollection<Supplier>(result.ListSupplier);
                ListOfCategory = new ObservableCollection<ClassifyName>(result.ListCategory);

                ListOfCategoryForFilter = GetFilterList(ListOfCategory, ListOfCategoryForFilter);
                OnPropertyChanged("ListOfSupplierFiltered");
            }
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

        public ObservableCollection<Supplier> ListOfSelectdSupplier { get; set; }

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
        #endregion

        #region "Command"
        public ICommand CheckCategoryCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SelectAndReturnProductCommand { get; set; }

        #endregion
    }
}
