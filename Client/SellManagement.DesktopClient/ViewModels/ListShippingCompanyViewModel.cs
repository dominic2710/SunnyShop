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
using SellManagement.DesktopClient.UserControls;

namespace SellManagement.DesktopClient.ViewModels
{
    public class ListShippingCompanyViewModel:BaseViewModel, IOpenNewAble
    {
        public ListShippingCompanyViewModel()
        {
            new Task(async () =>
            {
                await LoadListShippingCompanyAsync();
            }).Start();

            NewShippingCompanyCommand = new RelayCommand<object>(p => { return true; }, p => AddShippingCompanyAsync());

            OpenShippingCompanyInfoCommand = new RelayCommand<object>(p => { return true; }, p => UpdateShippingCompanyAsync());
        }

        void AddShippingCompanyAsync()
        {
            ShippingCompanyViewModel viewModel = new ShippingCompanyViewModel();
            ShippingCompanyUserControl ShippingCompanyUserControl = new ShippingCompanyUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this,
                    new OpenEventArgs
                    {
                        OpenUserControl = ShippingCompanyUserControl,
                        Header = viewModel.ShippingCompanyCd
                    });
        }

        void UpdateShippingCompanyAsync()
        {
            if (SelectedShippingCompany == null) return;

            ShippingCompanyViewModel viewModel = new ShippingCompanyViewModel
            {
                Id = SelectedShippingCompany.Id,
                ShippingCompanyCd = SelectedShippingCompany.ShippingCompanyCd,
                Name = SelectedShippingCompany.Name,
                Address1 = SelectedShippingCompany.Address1,
                Address2 = SelectedShippingCompany.Address2,
                PhoneNumber = SelectedShippingCompany.PhoneNumber,
                Email = SelectedShippingCompany.Email,
                Note = SelectedShippingCompany.Note,
            };
            ShippingCompanyUserControl ShippingCompanyUserControl = new ShippingCompanyUserControl
            {
                DataContext = viewModel
            };
            RequestOpen(this,
                    new OpenEventArgs
                    {
                        OpenUserControl = ShippingCompanyUserControl,
                        Header = viewModel.ShippingCompanyCd
                    });
        }

        async Task LoadListShippingCompanyAsync()
        {
            ShowBackdrop(true);
            ListOfShippingCompany = new ObservableCollection<ShippingCompany>();

            var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListShippingCompanyResponse>("ShippingCompany/GetListShippingCompany", HttpMethod.Post, null);
            if (result.StatusCode == 200)
            {
                ListOfShippingCompany = new ObservableCollection<ShippingCompany>(result.ListShippingCompany);
            }

            if (ListOfShippingCompany == null) ListOfShippingCompany = new ObservableCollection<ShippingCompany>();

            int lastSelectedIndex = SelectedIndex;

            OnPropertyChanged("ListOfShippingCompanyFiltered");

            SelectedIndex = lastSelectedIndex;
            OnPropertyChanged("SelectedIndex");
            ShowBackdrop(false);
        }


        public event EventHandler<OpenEventArgs> RequestOpen;

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

        public int SelectedIndex { get; set; }

        public bool IsVisble
        {
            set
            {
                if (value == true)
                {
                    new Task(async () =>
                    {
                        await LoadListShippingCompanyAsync();
                    }).Start();
                }
            }
        }
        #endregion

        #region "Command"
        public ICommand NewShippingCompanyCommand { get; set; }
        public ICommand CheckCategoryCommand { get; set; }
        public ICommand OpenShippingCompanyInfoCommand { get; set; }

        #endregion
    }
}
