using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.Customer;
using System.Collections.ObjectModel;
using System;
using System.Windows;

namespace SellManagement.DesktopClient.ViewModels
{
    public class CustomerViewModel : BaseViewModel, ICloseable, IFocusable
    {
        public CustomerViewModel()
        {
            ShowBackdrop(false);

            SaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveCustomer(true), ShowBackdrop);

            ContinuitySaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveCustomer(false), ShowBackdrop);

            GetCustomerCommand = new AsyncCommand<object>(p => { return true; }, p => GetCustomer(p.ToString()), ShowBackdrop);

            DeleteCustomerCommand = new AsyncCommand<object>(p => { return Id != 0; }, p => DeleteCustomer(), ShowBackdrop);

            ClearAllCommand = new RelayCommand<object>(p => { return true; }, p => ClearAll());
        }

        async Task SaveCustomer(bool IsCloseAfter)
        {
            if (MessageBox.Show("Lưu thông tin khách hàng?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            //Check input
            if (CustomerCd == null || CustomerCd == "")
            {
                MessageBox.Show("Chưa nhập mã khách hàng", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "CustomerCd" });
                return;
            }
            if (CategoryCd == 0)
            {
                MessageBox.Show("Chưa chọn phân loại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "Category" });
                return;
            }
            if (Name == null || Name == "")
            {
                MessageBox.Show("Chưa nhập tên khách hàng", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "CustomerName" });
                return;
            }

            var customer = new Customer
            {
                Id = Id,
                CustomerCd = CustomerCd,
                Name = Name,
                CategoryCd = CategoryCd,
                Address1 = Address1,
                Address2 =Address2,
                PhoneNumber =PhoneNumber,
                Email = Email,
                Facebook = Facebook,
                Note = Note,
            };

            int saveStatus;
            if (Id == 0)
            {
                var request = new CustomerAddRequest
                {
                    CustomerData = customer
                };

                var response = await ServiceProvider.GetInstance().CallWebApi<CustomerAddRequest, CustomerAddResponse>("Customer/AddCustomer", HttpMethod.Post, request);
                saveStatus = response.StatusCode;
                if (response.StatusCode == 200) { }
                else
                {
                    Message = response.Message;
                }
            }
            else
            {
                var request = new CustomerUpdateRequest
                {
                    CustomerData = customer
                };
                var response = await ServiceProvider.GetInstance().CallWebApi<CustomerUpdateRequest, CustomerUpdateResponse>("Customer/UpdateCustomer", HttpMethod.Post, request);
                saveStatus = response.StatusCode;
                if (response.StatusCode == 200) { }
                else
                {
                    Message = response.Message;
                }
            }

            if (saveStatus == 200)
            {
                MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                if (IsCloseAfter)
                {
                    ClearAll();
                }
                else
                {
                    Id = 0;
                    CustomerCd = "";

                    var handler = RequestFocus;
                    if (handler != null) handler(this, new FocusEventArgs { FocusItem = "CustomerCd" });

                    OnPropertyChanged("Id");
                    OnPropertyChanged("ShowNewCustomerMark");
                    OnPropertyChanged("IsShowTabOrderHistory");
                }
            }
            else
                MessageBox.Show($"Lỗi! \n {Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        async Task DeleteCustomer()
        {
            if (Id != 0)
            {
                if (MessageBox.Show("Xóa thông tin khách hàng?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                var response = await ServiceProvider.GetInstance().CallWebApi<string, CustomerDeleteResponse>("Customer/DeleteCustomer", HttpMethod.Post, CustomerCd);

                if (response.StatusCode == 200)
                {
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearAll();
                }
                else
                {
                    Message = response.Message;
                    MessageBox.Show($"Lỗi! \n {Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        async Task GetCustomer(string customerCd)
        {
            if (customerCd == CustomerCd) return;

            var response = await ServiceProvider.GetInstance().CallWebApi<string, CustomerAddResponse>("Customer/GetCustomerByCd", HttpMethod.Post, customerCd);
            if (response.StatusCode == 200)
            {
                if (response.CustomerData != null)
                {
                    Id = response.CustomerData.Id;
                    CustomerCd = response.CustomerData.CustomerCd;
                    Name = response.CustomerData.Name;
                    CategoryCd = response.CustomerData.CategoryCd;
                    Address1 = response.CustomerData.Address1;
                    Address2 = response.CustomerData.Address2;
                    PhoneNumber = response.CustomerData.PhoneNumber;
                    Email = response.CustomerData.Email;
                    Facebook = response.CustomerData.Facebook;
                    Note = response.CustomerData.Note;
                }
                else
                {
                    CustomerCd = customerCd;
                    Id = 0;
                }
            }
            else
            {
                Message = response.Message;
            }

            OnPropertyChanged("ShowNewCustomerMark");
            OnPropertyChanged("IsShowTabOrderHistory");
        }

        void ClearAll()
        {
            Id = 0;
            CustomerCd = "";
            Name = "";
            CategoryCd = 0;
            Address1 = "";
            Address2 = "";
            PhoneNumber = "";
            Email = "";
            Facebook = "";
            Note = "";

            var handler = RequestFocus;
            if (handler != null) handler(this, new FocusEventArgs { FocusItem = "CustomerCd" });

            OnPropertyChanged("Id");
            OnPropertyChanged("ShowNewCustomerMark");
            OnPropertyChanged("IsShowTabOrderHistory");
        }

        public event EventHandler<EventArgs> RequestClose;
        public event EventHandler<FocusEventArgs> RequestFocus;

        #region "Variable"
        int id;
        string customerCd;
        string name;
        int categoryCd;
        string address1;
        string address2;
        string phoneNumber;
        string email;
        string facebook;
        string note;
        string message;
        #endregion

        #region "Properties"
        public int Id {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string CustomerCd
        {
            get { return customerCd; }
            set
            {
                customerCd = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public int CategoryCd
        {
            get { return categoryCd; }
            set
            {
                categoryCd = value;
                OnPropertyChanged();
            }
        }
        public string Address1
        {
            get { return address1; }
            set
            {
                address1 = value;
                OnPropertyChanged();
            }
        }
        public string Address2
        {
            get { return address2; }
            set
            {
                address2 = value;
                OnPropertyChanged();
            }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        public string Facebook
        {
            get { return facebook; }
            set
            {
                facebook = value;
                OnPropertyChanged();
            }
        }
        public string Note
        {
            get { return note; }
            set
            {
                note = value;
                OnPropertyChanged();
            }
        }
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassifyName> ListOfCategory { get; set; }
        public ObservableCollection<ClassifyName> ListOfTradeMark { get; set; }
        public ObservableCollection<ClassifyName> ListOfOrigin { get; set; }
        public Visibility ShowNewCustomerMark
        {
            get { return Id == 0 && CustomerCd != null && CustomerCd != "" ? Visibility.Visible : Visibility.Hidden; }
        }
        public Visibility IsShowTabOrderHistory { get { return Id == 0 ? Visibility.Collapsed : Visibility.Visible; } }

        #endregion

        #region "Command"
        public ICommand SaveCommand { get; set; }
        public ICommand ContinuitySaveCommand { get; set; }
        public ICommand GetCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        #endregion
    }
}
