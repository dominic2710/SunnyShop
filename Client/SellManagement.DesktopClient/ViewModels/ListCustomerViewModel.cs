using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.Customer;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System;
using SellManagement.DesktopClient.UserControls;

namespace SellManagement.DesktopClient.ViewModels
{
    public class ListCustomerViewModel:BaseViewModel, IOpenNewAble
    {
        public ListCustomerViewModel()
        {
            new Task(async () =>
            {
                await LoadListCustomerAsync();
            }).Start();

            NewCustomerCommand = new RelayCommand<object>(p => { return true; }, p => AddCustomerAsync());

            CheckCategoryCommand = new RelayCommand<object>(p => { return true; }, p => UpdateFilterList(ListOfCategoryForFilter, p));

            OpenCustomerInfoCommand = new RelayCommand<object>(p => { return true; }, p => UpdateCustomerAsync());
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

            OnPropertyChanged("ListOfCustomerFiltered");
        }

        void AddCustomerAsync()
        {
            if (RequestOpen == null) return;
            CustomerViewModel viewModel = new CustomerViewModel
            {
                ListOfCategory = ListOfCategory,
            };
            CustomerUserControl customerUserControl = new CustomerUserControl
            {
                DataContext = viewModel
            };

            RequestOpen(this,
                    new OpenEventArgs
                    {
                        OpenUserControl = customerUserControl,
                        Header = viewModel.CustomerCd
                    });
        }

        void UpdateCustomerAsync()
        {
            if (RequestOpen == null) return;
            if (SelectedCustomer == null) return;

            CustomerViewModel viewModel = new CustomerViewModel
            {
                Id = SelectedCustomer.Id,
                CustomerCd = SelectedCustomer.CustomerCd,
                Name = SelectedCustomer.Name,
                CategoryCd = SelectedCustomer.CategoryCd,
                Address1 = SelectedCustomer.Address1,
                Address2 = SelectedCustomer.Address2,
                PhoneNumber = SelectedCustomer.PhoneNumber,
                Email = SelectedCustomer.Email,
                Facebook = SelectedCustomer.Facebook,
                Note = SelectedCustomer.Note,
                ListOfCategory = ListOfCategory,
            };
            CustomerUserControl customerUserControl = new CustomerUserControl
            {
                DataContext = viewModel
            };

            RequestOpen(this,
                    new OpenEventArgs
                    {
                        OpenUserControl = customerUserControl,
                        Header = viewModel.CustomerCd
                    });
        }

        async Task LoadListCustomerAsync()
        {

            ShowBackdrop(true);
            ListOfCustomer = new ObservableCollection<Customer>();

            var result = await ServiceProvider.GetInstance().CallWebApi<object, GetListCustomerResponse>("Customer/GetListCustomer", HttpMethod.Post, null);
            if (result.StatusCode == 200)
            {
                ListOfCustomer = new ObservableCollection<Customer>(result.ListCustomer);
                ListOfCategory = new ObservableCollection<ClassifyName>(result.ListCategory);

                ListOfCategoryForFilter = GetFilterList(ListOfCategory, ListOfCategoryForFilter);
            }
            int lastSelectedIndex = SelectedIndex;

            OnPropertyChanged("ListOfCustomerFiltered");

            SelectedIndex = lastSelectedIndex;
            OnPropertyChanged("SelectedIndex");
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

        public event EventHandler<OpenEventArgs> RequestOpen;

        #region "Properties"
        private ObservableCollection<Customer> listOfCustomer;
        public ObservableCollection<Customer> ListOfCustomer
        {
            get { return listOfCustomer; }
            set { listOfCustomer = value; }
        }
        public ObservableCollection<Customer> ListOfCustomerFiltered
        {
            get
            {
                if (ListOfCustomer == null) return new ObservableCollection<Customer>();
                var list = ListOfCustomer
                                    .Where(x => FilterName == null || FilterName == "" || (FilterName != "" && x.Name.ToUpper().Contains(FilterName.ToUpper())))
                                    .Where(x => ListOfCategoryForFilter.Where(t => t.IsChecked).Select(t =>int.Parse( t.Code)).Contains(x.CategoryCd))
                                    .ToList();

                return new ObservableCollection<Customer>(list);
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
                OnPropertyChanged("ListOfCustomerFiltered");
            }
        }

        private Customer selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set { selectedCustomer = value; }
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
                        await LoadListCustomerAsync();
                    }).Start();
                }
            }
        }
        #endregion

        #region "Command"
        public ICommand NewCustomerCommand { get; set; }
        public ICommand CheckCategoryCommand { get; set; }
        public ICommand OpenCustomerInfoCommand { get; set; }

        #endregion
    }
}
