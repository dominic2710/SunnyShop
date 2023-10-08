using SunnyShop.MobileClient.Helpers;
using SunnyShop.MobileClient.Models;
using SunnyShop.MobileClient.Services.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;

namespace SunnyShop.MobileClient.ViewModels
{
    public class WarehouseInboundCheckingViewModel : BaseViewModel, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            var barcode = HttpUtility.UrlDecode(query["barcode"].ToString());
            var productCd = HttpUtility.UrlDecode(query["productCd"].ToString());
            var productName = HttpUtility.UrlDecode(query["productName"].ToString());
            var sapoProductId = HttpUtility.UrlDecode(query["sapoProductId"].ToString());
            var lotNo = HttpUtility.UrlDecode(query["lotNo"].ToString());
            var expiryDate = DateTime.Parse(HttpUtility.UrlDecode(query["expiryDate"].ToString()));
            var addQuantity =int.Parse(HttpUtility.UrlDecode(query["addQuantity"].ToString()));

            var existedInventoryDetail = InventoryDetails.Where(x => x.Barcode == barcode)
                                                            .Where(x => x.LotNo == lotNo)
                                                            .Where(x => x.ExpiryDate == expiryDate)
                                                            .FirstOrDefault();

            if (existedInventoryDetail != null)
            {
                existedInventoryDetail.Quantity += addQuantity;
            }
            else
            {
                InventoryDetails.Add(new InventoryDetail
                {
                    Barcode = barcode,
                    ExpiryDate = expiryDate,
                    LotNo = lotNo,
                    ProductCd = productCd,
                    ProductName = productName,
                    Quantity = addQuantity,
                    SapoProductId = sapoProductId,
                    IsEditable = false,
                    IsDisplay = true
                });
            }
        }

        //async Task AddProductAsync()
        //{
        //    var existedInventoryDetail = InventoryDetails.Where(x => x.Barcode == ScannedBarcode).FirstOrDefault();

        //    if (existedInventoryDetail != null)
        //    {
        //        existedInventoryDetail.Quantity++;

        //        OnPropertyChanged(nameof(InventoryDetails));
        //        return;
        //    }
            
        //    var result = await Services.ServiceProvider.GetInstance().CallWebApi<string, GetProductByCdResponse>("/Product/GetProductByBarcode", HttpMethod.Post, ScannedBarcode);

        //    if (result.StatusCode == 200)
        //    {
        //        var scannedProduct = result.ProductData;

        //        if (scannedProduct != null)
        //        {
        //            InventoryDetails.Add(new InventoryDetail
        //            {
        //                Barcode = ScannedBarcode,
        //                ExpiryDate = DateTime.Now,
        //                LotNo = DateTime.Now.ToString("yyyyMMdd"),
        //                ProductCd = scannedProduct.ProductCd,
        //                ProductName = scannedProduct.Name,
        //                Quantity = 1,
        //                SapoProductId = scannedProduct.SapoProductId
        //            });

        //            OnPropertyChanged(nameof(InventoryDetails));
        //        }
        //        else
        //        {
        //            ErrorMessage = "Không tìm thấy sản phẩm tương ứng";
        //            //await Shell.Current.CurrentPage.DisplayAlert("Lỗi", "Không tìm thấy sản phẩm tương ứng", "OK");
        //        }
        //    }
        //    else
        //    {
        //        ErrorMessage = result.StatusMessage;
        //        //await Shell.Current.CurrentPage.DisplayAlert("Lỗi", result.StatusMessage, "OK");
        //    }
        //}

        //public string ScannedBarcode { get; set; }

        private ObservableCollection<InventoryDetail> _inventoryDetails;
        public ObservableCollection<InventoryDetail> InventoryDetails
        {
            get { return _inventoryDetails; }
            set { _inventoryDetails = value; OnPropertyChanged(); }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged(); }
        }

        public WarehouseInboundCheckingViewModel()
        {
            InventoryDetails = new ObservableCollection<InventoryDetail>();

            OpenScanBarcodePageCommand = new Command<object>(async (param) =>
            {
                ErrorMessage = "";
                await Shell.Current.GoToAsync($"ScanBarcodePage");
            });
        }

        public ICommand OpenScanBarcodePageCommand { get; set; }

        //async Task LoadListProductAsync()
        //{
        //    Global.ListProduct = new ObservableCollection<Product>();

        //    var result = await Services.ServiceProvider.GetInstance().CallWebApi<object, GetListProductResponse>("/Product/GetListProduct", HttpMethod.Post, null);

        //    if (result.StatusCode == 200)
        //    {
        //        Global.ListProduct = new ObservableCollection<Product>(result.ListProduct);
        //    }
        //}
    }
}
