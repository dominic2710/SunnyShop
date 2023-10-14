using SunnyShop.MobileClient.Helpers;
using SunnyShop.MobileClient.Models;
using SunnyShop.MobileClient.Pages;
using SunnyShop.MobileClient.Services.Inventory;
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

            var sourcePage = HttpUtility.UrlDecode(query["sourcePage"].ToString());

            switch (sourcePage)
            {
                case nameof(ListWarehouseInboundCheckingPage):
                    if (!query.ContainsKey("inventoryNo")) return;
                    InventoryNo = HttpUtility.UrlDecode(query["inventoryNo"].ToString());
                    if (!string.IsNullOrEmpty(InventoryNo))
                    {
                        Task.Run(GetInventoryData);
                    }
                    break;

                case nameof(EditInventoryDetailPage):
                    var barcode = HttpUtility.UrlDecode(query["barcode"].ToString());
                    var productCd = HttpUtility.UrlDecode(query["productCd"].ToString());
                    var productName = HttpUtility.UrlDecode(query["productName"].ToString());
                    var sapoProductId = HttpUtility.UrlDecode(query["sapoProductId"].ToString());
                    var lotNo = HttpUtility.UrlDecode(query["lotNo"].ToString());
                    var expiryDate = DateTime.Parse(HttpUtility.UrlDecode(query["expiryDate"].ToString()));
                    var addQuantity = int.Parse(HttpUtility.UrlDecode(query["addQuantity"].ToString()));

                    var existedInventoryDetail = InventoryDetails.Where(x => x.Barcode == barcode)
                                                                    //.Where(x => x.LotNo == lotNo)
                                                                    .Where(x => x.ExpiryDate == expiryDate)
                                                                    .FirstOrDefault();

                    if (existedInventoryDetail != null)
                    {
                        existedInventoryDetail.Quantity += addQuantity;
                    }
                    else
                    {
                        InventoryDetails.Add(new Models.InventoryDetail
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
                    break;
            }
            
        }

        private ObservableCollection<Models.InventoryDetail> _inventoryDetails;
        public ObservableCollection<Models.InventoryDetail> InventoryDetails
        {
            get { return _inventoryDetails; }
            set { _inventoryDetails = value; OnPropertyChanged(); OnPropertyChanged(nameof(DetailCount)); }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged(); }
        }

        private string inventoryNo;
        public string InventoryNo
        {
            get { return inventoryNo; }
            set { inventoryNo = value; OnPropertyChanged();}
        }

        private DateTime inventoryDate;
        public DateTime InventoryDate
        {
            get { return inventoryDate; }
            set { inventoryDate = value; OnPropertyChanged();}
        }

        private string note;
        public string Note
        {
            get { return note; }
            set { note = value; OnPropertyChanged(); }
        }

        public int DetailCount
        {
            get { return InventoryDetails == null ? 0 : InventoryDetails.Sum(x => x.Quantity); }
        }

        public WarehouseInboundCheckingViewModel()
        {
            InventoryDetails = new ObservableCollection<Models.InventoryDetail>();
            InventoryDate = DateTime.Now;


            OpenScanBarcodePageCommand = new Command<object>(async (param) =>
            {
                ErrorMessage = "";
                await Shell.Current.GoToAsync($"ScanBarcodePage");
            });

            DeleteDetailCommand = new Command<object>(DeleteDetail);

            SaveDataCommand = new Command<object>(async(param) => await SaveData());

            DeleteDataCommand = new Command<object>(async (param)=>
            {
                await DeleteData();
            }, param =>
            {
                return !string.IsNullOrEmpty(InventoryNo);
            });
        }

        void DeleteDetail(object param)
        {

        }

        async Task GetInventoryData()
        {
            var result = await Services.ServiceProvider.GetInstance().CallWebApi<string, InventoryAddResponse>
                                                            ("/Inventory/GetInventoryByNo", HttpMethod.Post, InventoryNo);

            if (result.StatusCode == 200)
            {
                //await Shell.Current.DisplayAlert("Thông báo", "Lưu thông tin kiểm kê thành công", "OK");

                InventoryNo = result.InventoryData.InventoryNo;
                InventoryDate = result.InventoryData.InventoryDate;
                Note = result.InventoryData.Note;

                InventoryDetails = new ObservableCollection<Models.InventoryDetail>(
                                        result.InventoryData.InventoryDetails.Select(x => new Models.InventoryDetail
                                        {
                                            ProductCd = x.ProductCd,
                                            ProductName = x.ProductName,
                                            ExpiryDate = x.ExpiryDate,
                                            Barcode = x.Barcode,
                                            Quantity = x.Quantity,
                                            IsDisplay = true,
                                            IsEditable = false,
                                        }));
            }
            else
            {
                await Shell.Current.DisplayAlert("Lỗi", result.StatusMessage, "OK");
            }
        }

        async Task SaveData()
        {
            var confirmed = await Shell.Current.DisplayAlert("Xác nhận", "Lưu thông tin kiểm kê", "Đồng ý", "Hủy");
            if (!confirmed) return;

            var inventoryData = new Services.Inventory.Inventory
            {
                InventoryNo = InventoryNo,
                InventoryDate = InventoryDate,
                Note = Note,
                InventoryDetails = InventoryDetails.Select(x => new Services.Inventory.InventoryDetail
                {
                    InventoryNo = InventoryNo,
                    Barcode = x.Barcode,
                    ProductCd = x.ProductCd,
                    ProductName = x.ProductName,
                    ExpiryDate = x.ExpiryDate,
                    Quantity = x.Quantity,
                })
            };

            //Update
            if (!string.IsNullOrEmpty(InventoryNo))
            {
                var request = new InventoryUpdateRequest
                {
                    InventoryData = inventoryData
                };

                var result = await Services.ServiceProvider.GetInstance().CallWebApi<InventoryUpdateRequest, InventoryUpdateResponse>
                                            ("/Inventory/UpdateInventory", HttpMethod.Post, request);

                if (result.StatusCode == 200)
                {
                    await Shell.Current.DisplayAlert("Thông báo", "Lưu thông tin kiểm kê thành công", "OK");
                    await Shell.Current.GoToAsync($"ListWarehouseInboundCheckingPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Lỗi", result.StatusMessage, "OK");
                }
            }
            //Add
            else
            {
                var request = new InventoryAddRequest
                {
                    InventoryData = inventoryData,
                };
                var result = await Services.ServiceProvider.GetInstance().CallWebApi<InventoryAddRequest, InventoryAddResponse>
                                                            ("/Inventory/AddInventory", HttpMethod.Post, request);

                if (result.StatusCode == 200)
                {
                    await Shell.Current.DisplayAlert("Thông báo", "Lưu thông tin kiểm kê thành công", "OK");
                    await Shell.Current.GoToAsync($"ListWarehouseInboundCheckingPage");

                    InventoryNo = result.InventoryData.InventoryNo;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Lỗi", result.StatusMessage, "OK");
                }
            }

        }

        async Task DeleteData()
        {
            var confirmed = await Shell.Current.DisplayAlert("Xác nhận", "Delete thông tin kiểm kê này?", "Đồng ý", "Hủy");
            if (!confirmed) return;

            var result = await Services.ServiceProvider.GetInstance().CallWebApi<string, InventoryDeleteResponse>
                                        ("/Inventory/UpdateInventory", HttpMethod.Post, InventoryNo);

            if (result.StatusCode == 200)
            {
                await Shell.Current.DisplayAlert("Thông báo", "Đã delete thông tin kiểm kê", "OK");
                await Shell.Current.GoToAsync($"ListWarehouseInboundCheckingPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Lỗi", result.StatusMessage, "OK");
            }
        }

        public ICommand OpenScanBarcodePageCommand { get; set; }
        public ICommand DeleteDetailCommand { get; set; }
        public ICommand EditDetailCommand { get; set; }
        public ICommand SaveDataCommand { get; set; }
        public ICommand DeleteDataCommand { get; set; }

    }
}
