using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.ShippingCompany;
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
    public class SearchShippingCompanyViewModel:BaseViewModel, ICloseable
    {
        public SearchShippingCompanyViewModel()
        {
            SelectAndReturnProductCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ListOfSelectdShippingCompany = new ObservableCollection<ShippingCompany>();
                ListOfSelectdShippingCompany.Add(SelectedShippingCompany);

                var handler = RequestClose;
                if (handler != null) handler(this, EventArgs.Empty);
            });

            CancelCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ListOfSelectdShippingCompany = new ObservableCollection<ShippingCompany>();

                var handler = RequestClose;
                if (handler != null) handler(this, EventArgs.Empty);
            });
            
        }

        public event EventHandler<EventArgs> RequestClose;

        public void LoadListShippingCompany()
        {
            new Task(async () => await LoadListShippingCompanyAsync()).Start();
        }

        public async Task LoadListShippingCompanyAsync()
        {
            ShowBackdrop(true);
            ListOfShippingCompany = new ObservableCollection<ShippingCompany>();

            var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListShippingCompanyResponse>("ShippingCompany/GetListShippingCompany", HttpMethod.Post, null);
            if (result.StatusCode == 200)
            {
                ListOfShippingCompany = new ObservableCollection<ShippingCompany>(result.ListShippingCompany);
            }
            OnPropertyChanged("ListOfShippingCompanyFiltered");
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
        private ObservableCollection<ShippingCompany> listOfShippingCompany;
        public ObservableCollection<ShippingCompany> ListOfShippingCompany
        {
            get { return listOfShippingCompany; }
            set { listOfShippingCompany = value; }
        }
        public ObservableCollection<ShippingCompany> ListOfShippingCompanyFiltered
        {
            get
            {
                if (ListOfShippingCompany == null) return new ObservableCollection<ShippingCompany>();
                var list = ListOfShippingCompany
                                    .Where(x => FilterName == null || FilterName == "" || (FilterName != "" && x.Name.ToUpper().Contains(FilterName.ToUpper())))
                                    .ToList();

                return new ObservableCollection<ShippingCompany>(list);
            }
        }

        public ObservableCollection<ShippingCompany> ListOfSelectdShippingCompany { get; set; }

        private string filterName;
        public string FilterName
        {
            get { return filterName; }
            set
            {
                filterName = value;
                OnPropertyChanged();
                OnPropertyChanged("ListOfShippingCompanyFiltered");
            }
        }

        private ShippingCompany selectedShippingCompany;
        public ShippingCompany SelectedShippingCompany
        {
            get { return selectedShippingCompany; }
            set { selectedShippingCompany = value; }
        }
        #endregion

        #region "Command"
        public ICommand CancelCommand { get; set; }
        public ICommand SelectAndReturnProductCommand { get; set; }

        #endregion
    }
}
