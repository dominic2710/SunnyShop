using SellManagement.DesktopClient.Models;
using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Services.ClassifyName;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SellManagement.DesktopClient.ViewModels
{
    public class ListOfClassifyNameViewModel : BaseViewModel
    {
        public ListOfClassifyNameViewModel()
        {
            ListOfClassifyGroup = new ObservableCollection<ClassifyGroup>();
            ListOfClassifyGroup.Add(new ClassifyGroup { GroupId = 1, Name = "Nhóm sản phẩm " });
            ListOfClassifyGroup.Add(new ClassifyGroup { GroupId = 2, Name = "Thương hiệu sản phẩm" });
            ListOfClassifyGroup.Add(new ClassifyGroup { GroupId = 3, Name = "Xuất xứ sản phẩm" });
            ListOfClassifyGroup.Add(new ClassifyGroup { GroupId = 4, Name = "Phân loại khách hàng" });
            ListOfClassifyGroup.Add(new ClassifyGroup { GroupId = 5, Name = "Phân loại nhà cung cấp" });

            ShowBackdrop(false);

            IsDataChanged = false;

            SaveCommand = new AsyncCommand<object>(p => { return true; }, p => SaveListClassifyAsync(true), ShowBackdrop);

            ClearAllCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                if (IsDataChanged)
                {
                    switch (MessageBox.Show("Lưu thay đổi", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    {
                        case MessageBoxResult.Yes:
                            new Task(async () =>
                            {
                                await SaveListClassifyAsync(false);
                                await LoadListClassifyAsync();
                            }).Start();

                            break;
                        case MessageBoxResult.No:
                            new Task(async () =>
                            {
                                await LoadListClassifyAsync();
                            }).Start();
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }
                }
                else
                {
                    new Task(async () =>
                    {
                        await LoadListClassifyAsync();
                    }).Start();
                }
            });
        }

        async Task LoadListClassifyAsync()
        {
            ShowBackdrop(true);
            ListOfClassifyName = new ObservableCollection<ClassifyName>();

            var response = await ServiceProvider.GetInstance().CallWebApi<int, GetListClassifyNameByGroupIdResponse>("ClassifyName/GetListClassifyNameByGroupId", HttpMethod.Post, SelectedClassifyGroup.GroupId);
            if (response.StatusCode == 200)
            {
                ListOfClassifyName = new ObservableCollection<ClassifyName>(response.ListClassifyName);
            }
            ShowBackdrop(false);

            IsDataChanged = false;
        }
        async Task SaveListClassifyAsync(bool isShowConfirmMessage)
        {
            if (isShowConfirmMessage)
                if (MessageBox.Show("Lưu thay đổi?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

            ShowBackdrop(true);
            var request = new ClassifyNameUpdateRequest
            {
                ClassifyNameData = new ObservableCollection<ClassifyName>(ListOfClassifyName)
            };

            var response = await ServiceProvider.GetInstance().CallWebApi<ClassifyNameUpdateRequest, GetListClassifyNameByGroupIdResponse>("ClassifyName/UpdateClassifyName", HttpMethod.Post, request);
            if (response.StatusCode == 200)
            {
                MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                IsDataChanged = false;
            }
            else
            {
                Message = response.Message;
                MessageBox.Show($"Lỗi! \n {Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ShowBackdrop(false);
        }

        #region Properties
        private ObservableCollection<ClassifyGroup> listOfClassifyGroup;
        public ObservableCollection<ClassifyGroup> ListOfClassifyGroup
        {
            get { return listOfClassifyGroup; }
            set
            {
                listOfClassifyGroup = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ClassifyName> listOfClassifyName;
        public ObservableCollection<ClassifyName> ListOfClassifyName
        {
            get { return listOfClassifyName; }
            set
            {
                listOfClassifyName = value;
                listOfClassifyName.CollectionChanged += ListOfClassifyName_CollectionChanged;
                OnPropertyChanged();
            }
        }

        private void ListOfClassifyName_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                (e.NewItems[0] as ClassifyName).Code = ListOfClassifyName.Max(x => x.Code) + 1;
                (e.NewItems[0] as ClassifyName).GroupId = SelectedClassifyGroup.GroupId;
            }
            IsDataChanged = true;
        }

        private ClassifyGroup selectedClassifyGroup;
        public ClassifyGroup SelectedClassifyGroup
        {
            get { return selectedClassifyGroup; }
            set
            {
                if (IsDataChanged)
                {
                    switch (MessageBox.Show("Lưu thay đổi", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    {
                        case MessageBoxResult.Yes:
                            selectedClassifyGroup = value;
                            new Task(async () =>
                            {
                                await SaveListClassifyAsync(false);
                                await LoadListClassifyAsync();
                            }).Start();

                            break;
                        case MessageBoxResult.No:
                            selectedClassifyGroup = value;
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }
                }
                else
                {
                    selectedClassifyGroup = value;
                    new Task(async () =>
                    {
                        await LoadListClassifyAsync();
                    }).Start();
                }
                OnPropertyChanged();
            }
        }

        private ClassifyName selectedClassifyName;
        public ClassifyName SelectedClassifyName
        {
            get { return selectedClassifyName; }
            set
            {
                selectedClassifyName = value;
                OnPropertyChanged();
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public bool IsDataChanged { get; set; }
        #endregion

        #region Command
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteClassifyNameCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        public ICommand InputClassifyCodeCommand { get; set; }
        #endregion
    }
}
