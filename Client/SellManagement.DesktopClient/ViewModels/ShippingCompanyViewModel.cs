using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using SellManagement.DesktopClient.Services.ShippingCompany;
using System;
using System.Windows;

namespace SellManagement.DesktopClient.ViewModels
{
    public class ShippingCompanyViewModel : BaseViewModel,  IFocusable
    {
        public ShippingCompanyViewModel()
        {
            ShowBackdrop(false);

            SaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveShippingCompany(true), ShowBackdrop);

            ContinuitySaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveShippingCompany(false), ShowBackdrop);

            GetShippingCompanyCommand = new AsyncCommand<object>(p => { return true; }, p => GetShippingCompany(p.ToString()), ShowBackdrop);

            DeleteShippingCompanyCommand = new AsyncCommand<object>(p => { return Id != 0; }, p => DeleteShippingCompany(), ShowBackdrop);

            ClearAllCommand = new RelayCommand<object>(p => { return true; }, p => ClearAll());
        }

        async Task SaveShippingCompany(bool IsClearAllAfter)
        {
            if (MessageBox.Show("Lưu thông tin đơn vị vận chuyển?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            //Check input
            if (ShippingCompanyCd == null || ShippingCompanyCd == "")
            {
                MessageBox.Show("Chưa nhập mã đơn vị vẫn chuyển", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "ShippingCompanyCd" });
                return;
            }
            if (Name == null || Name == "")
            {
                MessageBox.Show("Chưa nhập tên đơn vị vận chuyển", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "ShippingCompanyName" });
                return;
            }

            var ShippingCompany = new ShippingCompany
            {
                Id = Id,
                ShippingCompanyCd = ShippingCompanyCd,
                Name = Name,
                Address1 = Address1,
                Address2 =Address2,
                PhoneNumber =PhoneNumber,
                Email = Email,
                Note = Note,
            };
            int saveStatus;
            if (Id == 0)
            {
                var request = new ShippingCompanyAddRequest
                {
                    ShippingCompanyData = ShippingCompany
                };

                var response = await ServiceProvider.GetInstance().CallWebApi<ShippingCompanyAddRequest, ShippingCompanyAddResponse>("ShippingCompany/AddShippingCompany", HttpMethod.Post, request);
                saveStatus = response.StatusCode;
                if (response.StatusCode == 200) { }
                else
                {
                    Message = response.Message;
                }
            }
            else
            {
                var request = new ShippingCompanyUpdateRequest
                {
                    ShippingCompanyData = ShippingCompany
                };
                var response = await ServiceProvider.GetInstance().CallWebApi<ShippingCompanyUpdateRequest, ShippingCompanyUpdateResponse>("ShippingCompany/UpdateShippingCompany", HttpMethod.Post, request);
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
                if (IsClearAllAfter)
                {
                    ClearAll();
                }
                else
                {
                    Id = 0;
                    ShippingCompanyCd = "";

                    var handler = RequestFocus;
                    if (handler != null) handler(this, new FocusEventArgs { FocusItem = "ShippingCompanyCd" });

                    OnPropertyChanged("Id");
                    OnPropertyChanged("ShowNewShippingCompanyMark");
                    OnPropertyChanged("IsShowTabOrderHistory");
                }
            }
            else
                MessageBox.Show($"Lỗi! \n {Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        async Task DeleteShippingCompany()
        {
            if (Id != 0)
            {
                if (MessageBox.Show("Xóa thông tin đơn vị vận chuyển?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                var response = await ServiceProvider.GetInstance().CallWebApi<string, ShippingCompanyDeleteResponse>("ShippingCompany/DeleteShippingCompany", HttpMethod.Post, ShippingCompanyCd);

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
        async Task GetShippingCompany(string shippingCompanyCd)
        {
            if (shippingCompanyCd == ShippingCompanyCd) return;

            var response = await ServiceProvider.GetInstance().CallWebApi<string, ShippingCompanyAddResponse>("ShippingCompany/GetShippingCompanyByCd", HttpMethod.Post, shippingCompanyCd);
            if (response.StatusCode == 200)
            {
                if (response.ShippingCompanyData != null)
                {
                    Id = response.ShippingCompanyData.Id;
                    ShippingCompanyCd = response.ShippingCompanyData.ShippingCompanyCd;
                    Name = response.ShippingCompanyData.Name;
                    Address1 = response.ShippingCompanyData.Address1;
                    Address2 = response.ShippingCompanyData.Address2;
                    PhoneNumber = response.ShippingCompanyData.PhoneNumber;
                    Email = response.ShippingCompanyData.Email;
                    Note = response.ShippingCompanyData.Note;
                }
                else
                {
                    ShippingCompanyCd = shippingCompanyCd;
                    Id = 0;
                }
            }
            else
            {
                Message = response.Message;
            }

            OnPropertyChanged("Id");
            OnPropertyChanged("ShowNewShippingCompanyMark");
            OnPropertyChanged("IsShowTabOrderHistory");
        }

        void ClearAll()
        {
            Id = 0;
            ShippingCompanyCd = "";
            Name = "";
            Address1 = "";
            Address2 = "";
            PhoneNumber = "";
            Email = "";
            Note = "";

            var handler = RequestFocus;
            if (handler != null) handler(this, new FocusEventArgs { FocusItem = "ShippingCompanyCd" });

            OnPropertyChanged("Id");
            OnPropertyChanged("ShowNewShippingCompanyMark");
            OnPropertyChanged("IsShowTabOrderHistory");
        }

        public event EventHandler<FocusEventArgs> RequestFocus;

        #region "Variable"
        int id;
        string shippingCompanyCd;
        string name;
        string address1;
        string address2;
        string phoneNumber;
        string email;
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
        public string ShippingCompanyCd
        {
            get { return shippingCompanyCd; }
            set
            {
                shippingCompanyCd = value;
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
        public Visibility ShowNewShippingCompanyMark
        {
            get { return Id == 0 && ShippingCompanyCd != null && ShippingCompanyCd != "" ? Visibility.Visible : Visibility.Hidden; }
        }
        public Visibility IsShowTabOrderHistory { get { return Id == 0 ? Visibility.Collapsed : Visibility.Visible; } }

        #endregion

        #region "Command"
        public ICommand SaveCommand { get; set; }
        public ICommand ContinuitySaveCommand { get; set; }
        public ICommand GetShippingCompanyCommand { get; set; }
        public ICommand DeleteShippingCompanyCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        #endregion
    }
}
