using SunnyShop.MobileClient.Pages;
using SunnyShop.MobileClient.Services.Inventory;
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
    public class ListWarehouseInboundCheckingViewModel: BaseViewModel, IQueryAttributable
    {
        private ObservableCollection<Models.Inventory> inventoryHeads;
        public ObservableCollection<Models.Inventory> InventoryHeads
        {
            get { return inventoryHeads; }
            set { inventoryHeads = value; OnPropertyChanged(); }
        }

        public ListWarehouseInboundCheckingViewModel()
        {
            Task.Run(GetListInventoryHead);

            AddInventoryCommand = new Command<object>(async (param) =>
            {
                await Shell.Current.GoToAsync($"WarehouseInboundCheckingPage?sourcePage={nameof(ListWarehouseInboundCheckingPage)}");
            });

            EditInventoryCommand = new Command<object>(async (param) =>
            {
                var selectedDetail = param as Models.Inventory;
                await Shell.Current.GoToAsync($"WarehouseInboundCheckingPage?" +
                                                        $"sourcePage={nameof(ListWarehouseInboundCheckingPage)}" +
                                                        $"&inventoryNo={selectedDetail.InventoryNo}");
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Task.Run(GetListInventoryHead);
        }

        async Task GetListInventoryHead()
        {
            var result = await Services.ServiceProvider.GetInstance().CallWebApi<object, GetListInventoryResponse>
                                            ("/Inventory/GetListInventory", HttpMethod.Post, null);

            if (result.StatusCode == 200)
            {
                InventoryHeads = new ObservableCollection<Models.Inventory>(
                                        result.ListInventory.Select(x => new Models.Inventory
                                        {
                                            InventoryNo = x.InventoryNo,
                                            InventoryDate = x.InventoryDate,
                                            Note = x.Note,
                                            ProductCount = x.InventoryDetails.Sum(y=>y.Quantity)
                                        }));
            }
            else
            {
                await Shell.Current.DisplayAlert("Lỗi", result.StatusMessage, "OK");
            }
        }

        public ICommand AddInventoryCommand { get; set; }
        public ICommand EditInventoryCommand { get; set; }
    }
}

