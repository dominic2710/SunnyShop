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
    public class ProductViewModel: BaseViewModel, IFocusable
    {
        public ProductViewModel()
        {
            ShowBackdrop(false);

            SaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveProduct(true), ShowBackdrop);

            ContinuitySaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveProduct(false), ShowBackdrop);

            GetProductCommand = new AsyncCommand<object>(p => { return true; }, p => GetProduct(p.ToString()), ShowBackdrop);

            DeleteProductCommand = new AsyncCommand<object>(p => { return Id != 0; }, p => DeleteProduct(), ShowBackdrop);

            ClearAllCommand = new RelayCommand<object>(p => { return true; }, p => ClearAll());
        }

        async Task SaveProduct(bool IsClearAllAfter)
        {
            if (MessageBox.Show("Lưu thông tin sản phẩm?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            //Check input
            if (ProductCd == null || ProductCd == "")
            {
                MessageBox.Show("Chưa nhập mã sản phẩm", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "ProductCd" });
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
                MessageBox.Show("Chưa nhập tên sản phẩm", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "ProductName" });
                return;
            }
            if (TradeMarkCd == 0)
            {
                MessageBox.Show("Chưa chọn thương hiệu", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "TradeMark" });
                return;
            }
            if (OriginCd == 0)
            {
                MessageBox.Show("Chưa chọn xuất xứ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RequestFocus(this, new FocusEventArgs { FocusItem = "Origin" });
                return;
            }

            var product = new Product
            {
                Id = Id,
                ProductCd = ProductCd,
                Barcode = Barcode,
                Name = Name,
                CategoryCd = CategoryCd,
                TradeMarkCd = TradeMarkCd,
                OriginCd = OriginCd,
                CostPrice = CostPrice,
                SoldPrice = SoldPrice,
                Detail = Detail
            };

            int saveStatus;
            if (Id == 0)
            {
                var request = new ProductAddRequest
                {
                    ProductData = product
                };

                var response = await ServiceProvider.GetInstance().CallWebApi<ProductAddRequest, ProductAddResponse>("Product/AddProduct", HttpMethod.Post, request);
                saveStatus = response.StatusCode;
                if (response.StatusCode == 200) { }
                else
                {
                    Message = response.Message;
                }
            }
            else
            {
                var request = new ProductUpdateRequest
                {
                    ProductData = product
                };
                var response = await ServiceProvider.GetInstance().CallWebApi<ProductUpdateRequest, ProductUpdateResponse>("Product/UpdateProduct", HttpMethod.Post, request);
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
                    ProductCd = "";

                    var handler = RequestFocus;
                    if (handler != null) handler(this, new FocusEventArgs { FocusItem = "ProductCd" });

                    OnPropertyChanged("Id");
                    OnPropertyChanged("ShowNewProductMark");
                    OnPropertyChanged("IsShowTabComboInfo");
                    OnPropertyChanged("IsShowTabInventoryInfo");
                }
            }
            else
                MessageBox.Show($"Lỗi! \n {Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        async Task DeleteProduct()
        {
            if (Id != 0)
            {
                if (MessageBox.Show("Xóa thông tin đơn vị vận chuyển?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                var response = await ServiceProvider.GetInstance().CallWebApi<string, ProductDeleteResponse>("Product/DeleteProduct", HttpMethod.Post, ProductCd);

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
        async Task GetProduct(string productCd)
        {
            if (productCd == ProductCd) return;

            var response = await ServiceProvider.GetInstance().CallWebApi<string, ProductAddResponse>("Product/GetProductByCd", HttpMethod.Post, productCd);
            if (response.StatusCode == 200)
            {
                if (response.ProductData != null)
                {
                    Id = response.ProductData.Id;
                    ProductCd = response.ProductData.ProductCd;
                    Barcode = response.ProductData.Barcode;
                    Name = response.ProductData.Name;
                    CategoryCd = response.ProductData.CategoryCd;
                    TradeMarkCd = response.ProductData.TradeMarkCd;
                    OriginCd = response.ProductData.OriginCd;
                    CostPrice = response.ProductData.CostPrice;
                    SoldPrice = response.ProductData.SoldPrice;
                    Detail = response.ProductData.Detail;
                }
                else
                {
                    ProductCd = productCd;
                    Id = 0;
                }
            }
            else
            {
                Message = response.Message;
            }

            OnPropertyChanged("ShowNewProductMark");
            OnPropertyChanged("IsShowTabComboInfo");
            OnPropertyChanged("IsShowTabInventoryInfo");
        }

        private void ClearAll()
        {
            Id = 0;
            ProductCd = "";
            Barcode = "";
            Name = "";
            CategoryCd = 0;
            TradeMarkCd = 0;
            OriginCd = 0;
            CostPrice = 0;
            SoldPrice = 0;
            Detail = "";

            var handler = RequestFocus;
            if (handler != null) handler(this, new FocusEventArgs { FocusItem = "ProductCd" });

            OnPropertyChanged("ShowNewProductMark");
            OnPropertyChanged("IsShowTabComboInfo");
            OnPropertyChanged("IsShowTabInventoryInfo");
        }

        public event EventHandler<FocusEventArgs> RequestFocus;

        #region "Variable"
        private int id;
        private string productCd;
        private string barCode;
        private string name;
        private int categoryCd;
        private int tradeMarkCd;
        private int originCd;
        private int costPrice;
        private int soldPrice;
        private string detail;
        private string message;
        #endregion

        #region "Properties"
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string ProductCd
        {
            get { return productCd;}
            set
            {
                productCd = value;
                OnPropertyChanged();
            }
        }
        public string Barcode
        {
            get { return barCode; }
            set
            {
                barCode = value;
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
                OnPropertyChanged("IsAllowEditComboInfo");
            }
        }
        public int TradeMarkCd
        {
            get { return tradeMarkCd; }
            set
            {
                tradeMarkCd = value;
                OnPropertyChanged();
            }
        }
        public int OriginCd
        {
            get { return originCd; }
            set
            {
                originCd = value;
                OnPropertyChanged();
            }
        }
        public int CostPrice
        {
            get { return costPrice; }
            set
            {
                costPrice = value;
                OnPropertyChanged();
            }
        }
        public int SoldPrice
        {
            get { return soldPrice; }
            set
            {
                soldPrice = value;
                OnPropertyChanged();
            }
        }
        public string Detail
        {
            get { return detail; }
            set
            {
                detail = value;
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

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; }
        }
        public Visibility ShowNewProductMark
        {
            get { return Id == 0 && ProductCd != null && ProductCd != "" ? Visibility.Visible : Visibility.Hidden; }
        }
        public bool IsAllowEditComboInfo { get { return CategoryCd == ShareContanst.CATEGORY_COMBO; } }
        public Visibility IsShowTabInventoryInfo { get { return Id == 0 ? Visibility.Collapsed : Visibility.Visible; } }

        #endregion

        #region "Command"
        public ICommand SaveCommand { get; set; }
        public ICommand ContinuitySaveCommand { get; set; }
        public ICommand GetProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        #endregion
    }
}
