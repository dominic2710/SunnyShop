using CommunityToolkit.Maui.Core.Platform;
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
    public class EditInventoryDetailViewModel: BaseViewModel, IQueryAttributable
    {
        private readonly WarehouseInboundCheckingViewModel _warehouseInboundCheckingViewModel;
        private ObservableCollection<InventoryDetail> inventoryDetails;
        public ObservableCollection<InventoryDetail> InventoryDetails
        {
            get { return inventoryDetails; }
            set { inventoryDetails = value; OnPropertyChanged(); }
        }

        public string ScannedBarcode { get; set; }

        private bool showRunning;
        public bool ShowRunning
        {
            get { return showRunning; }
            set
            {
                showRunning = value;
                OnPropertyChanged();
            }
        }

        private string productName;
        public string ProductName
        {
            get { return productName;}
            set { productName = value; OnPropertyChanged();}
        }

        private int addQuantity;
        public int AddQuantity
        {
            get { return addQuantity;}
            set { addQuantity = value; OnPropertyChanged();}
        }

        private InventoryDetail selectedInventoryDetail;
        public InventoryDetail SelectedInventoryDetail
        {
            get { return selectedInventoryDetail; }
            set { selectedInventoryDetail = value; OnPropertyChanged(); }
        }
        public ICommand ApplyInventoryDetailCommand { get; set; }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            ScannedBarcode = HttpUtility.UrlDecode(query["barCode"].ToString());

            Task.Run(AddProductAsync);
        }

        public EditInventoryDetailViewModel(WarehouseInboundCheckingViewModel warehouseInboundCheckingViewModel)
        {
            _warehouseInboundCheckingViewModel = warehouseInboundCheckingViewModel;
            ApplyInventoryDetailCommand = new Command<object>(async (p) =>
            {
                if (SelectedInventoryDetail != null)
                {
                    await Shell.Current.GoToAsync($"WarehouseInboundCheckingPage?barcode={SelectedInventoryDetail.Barcode}&" +
                                                        $"productCd={SelectedInventoryDetail.ProductCd}&" +
                                                        $"productName={SelectedInventoryDetail.ProductName}&" +
                                                        $"sapoProductId={SelectedInventoryDetail.SapoProductId}&" +
                                                        $"lotNo={SelectedInventoryDetail.LotNo}&" +
                                                        $"expiryDate={SelectedInventoryDetail.ExpiryDate}&" +
                                                        $"addQuantity={AddQuantity}");
                }
            });
        }

        async Task AddProductAsync()
        {
            ShowRunning = true;
            var existedInventoryDetails = _warehouseInboundCheckingViewModel.InventoryDetails
                                            .Where(x => x.Barcode == ScannedBarcode).ToList();

            if (existedInventoryDetails.Count > 0)
            {
                InventoryDetails = new ObservableCollection<InventoryDetail>(existedInventoryDetails);
                InventoryDetails.Add(new InventoryDetail
                {
                    ProductCd = existedInventoryDetails[0].ProductCd,
                    Barcode = existedInventoryDetails[0].Barcode,
                    ExpiryDate = DateTime.Now,
                    LotNo = "",
                    ProductName = existedInventoryDetails[0].ProductName,
                    Quantity = 0,
                    SapoProductId = existedInventoryDetails[0].SapoProductId,
                    IsEditable = true
                });

                for (int i = 0; i < InventoryDetails.Count; i++)
                {
                    InventoryDetails[i].PageNumber = $"{i+1}/{InventoryDetails.Count}";
                }
            }
            else
            {
                InventoryDetails = new ObservableCollection<InventoryDetail>();
                var result = await Services.ServiceProvider.GetInstance().CallWebApi<string, GetProductByCdResponse>("/Product/GetProductByBarcode", HttpMethod.Post, ScannedBarcode);

                if (result.StatusCode == 200)
                {
                    var scannedProduct = result.ProductData;

                    if (scannedProduct != null)
                    {
                        InventoryDetails.Add(new InventoryDetail
                        {
                            Barcode = ScannedBarcode,
                            ExpiryDate = DateTime.Now,
                            LotNo = "",
                            ProductCd = scannedProduct.ProductCd,
                            ProductName = scannedProduct.Name,
                            Quantity = 0,
                            PageNumber = "1/1",
                            IsEditable = true,
                            IsDisplay = false,
                            SapoProductId = scannedProduct.SapoProductId
                        });
                    }
                    else
                    {
                        ProductName = "";
                        AddQuantity = 0;

                        ShowRunning = false;

                        await Shell.Current.DisplayAlert("Lỗi", $"Barcode: {ScannedBarcode}\r\nKhông tìm thấy sản phẩm tương ứng!", "OK");
                    }
                }
                else
                {
                    ProductName = "";
                    AddQuantity = 0;

                    ShowRunning = false;
                    await Shell.Current.DisplayAlert("Lỗi", result.StatusMessage, "OK");
                }
            }

            ProductName = InventoryDetails[0].ProductName;
            AddQuantity = 1;

            ShowRunning = false;
        }

    }
}
