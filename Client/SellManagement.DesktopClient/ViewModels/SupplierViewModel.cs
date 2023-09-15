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
    public class SupplierViewModel : BaseViewModel,  IFocusable
    {
        public SupplierViewModel()
        {
            ShowBackdrop(false);

            SaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveSupplier(true), ShowBackdrop);

            ContinuitySaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveSupplier(false), ShowBackdrop);

            GetSupplierCommand = new AsyncCommand<object>(p => { return true; }, p => GetSupplier(p.ToString()), ShowBackdrop);

            DeleteSupplierCommand = new AsyncCommand<object>(p => { return Id != 0; }, p => DeleteSupplier(), ShowBackdrop);

            ClearAllCommand = new RelayCommand<object>(p => { return true; }, p => ClearAll());
        }
        async Task SaveSupplier(bool IsClearAllAfter)
        {
            if (MessageBox.Show("Lưu thông tin nhà cung cấp?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            //Check input
            if (SupplierCd == null || SupplierCd == "")
            {
                MessageBox.Show("Chưa nhập mã nhà cung cấp", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "SupplierCd" });
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
                MessageBox.Show("Chưa nhập tên nhà cung cấp", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "SupplierName" });
                return;
            }

            var supplier = new Supplier
            {
                Id = Id,
                SupplierCd = SupplierCd,
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
                var request = new SupplierAddRequest
                {
                    SupplierData = supplier
                };

                var response = await ServiceProvider.GetInstance().CallWebApi<SupplierAddRequest, SupplierAddResponse>("Supplier/AddSupplier", HttpMethod.Post, request);
                saveStatus = response.StatusCode;
                if (response.StatusCode == 200) { }
                else
                {
                    Message = response.Message;
                }
            }
            else
            {
                var request = new SupplierUpdateRequest
                {
                    SupplierData = supplier
                };
                var response = await ServiceProvider.GetInstance().CallWebApi<SupplierUpdateRequest, SupplierUpdateResponse>("Supplier/UpdateSupplier", HttpMethod.Post, request);
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
                    SupplierCd = "";

                    var handler = RequestFocus;
                    if (handler != null) handler(this, new FocusEventArgs { FocusItem = "SupplierCd" });

                    OnPropertyChanged("Id");
                    OnPropertyChanged("ShowNewSupplierMark");
                }
            }
            else
            {
                MessageBox.Show($"Lỗi! \n {Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        async Task DeleteSupplier()
        {
            if (Id != 0)
            {
                if (MessageBox.Show("Xóa thông tin nhà cung cấp?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                var response = await ServiceProvider.GetInstance().CallWebApi<string, SupplierDeleteResponse>("Supplier/DeleteSupplier", HttpMethod.Post, SupplierCd);

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
        async Task GetSupplier(string supplierCd)
        {
            if (supplierCd == SupplierCd) return;

            var response = await ServiceProvider.GetInstance().CallWebApi<string, SupplierAddResponse>("Supplier/GetSupplierByCd", HttpMethod.Post, supplierCd);
            if (response.StatusCode == 200)
            {
                if (response.SupplierData != null)
                {
                    Id = response.SupplierData.Id;
                    SupplierCd = response.SupplierData.SupplierCd;
                    Name = response.SupplierData.Name;
                    CategoryCd = response.SupplierData.CategoryCd;
                    Address1 = response.SupplierData.Address1;
                    Address2 = response.SupplierData.Address2;
                    PhoneNumber = response.SupplierData.PhoneNumber;
                    Email = response.SupplierData.Email;
                    Facebook = response.SupplierData.Facebook;
                    Note = response.SupplierData.Note;
                }
                else
                {
                    SupplierCd = supplierCd;
                    Id = 0;
                }
            }
            else
            {
                Message = response.Message;
            }

            OnPropertyChanged("ShowNewSupplierMark");
            OnPropertyChanged("IsShowTabOrderHistory");
        }

        void ClearAll()
        {
            Id = 0;
            SupplierCd = "";
            Name = "";
            CategoryCd = 0;
            Address1 = "";
            Address2 = "";
            PhoneNumber = "";
            Email = "";
            Facebook = "";
            Note = "";

            var handler = RequestFocus;
            if (handler != null) handler(this, new FocusEventArgs { FocusItem = "SupplierCd" });

            OnPropertyChanged("Id");
            OnPropertyChanged("ShowNewSupplierMark");
            OnPropertyChanged("IsShowTabOrderHistory");
        }

        public event EventHandler<FocusEventArgs> RequestFocus;

        #region "Variable"
        int id;
        string supplierCd;
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
        public string SupplierCd
        {
            get { return supplierCd; }
            set
            {
                supplierCd = value;
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
        public Visibility ShowNewSupplierMark
        {
            get { return Id == 0 && SupplierCd != null && SupplierCd != "" ? Visibility.Visible : Visibility.Hidden; }
        }
        public Visibility IsShowTabOrderHistory { get { return Id == 0 ? Visibility.Collapsed : Visibility.Visible; } }

        #endregion

        #region "Command"
        public ICommand SaveCommand { get; set; }
        public ICommand ContinuitySaveCommand { get; set; }
        public ICommand GetSupplierCommand { get; set; }
        public ICommand DeleteSupplierCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        #endregion
    }
}
