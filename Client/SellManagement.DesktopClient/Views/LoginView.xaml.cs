using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Models;
using System.Net.Http;
using SellManagement.DesktopClient.ViewModels;
using SellManagement.DesktopClient.Services.Login;
using SellManagement.DesktopClient.Helpers;
using SellManagement.DesktopClient.Services.Customer;
using System.Windows.Media.Animation;

namespace SellManagement.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            await Login();
        }

        async Task Login()
        {
            if (PART_UserName.Text == "" || PART_Password.Password == "")
                return;

            AuthenticateRequest request = new AuthenticateRequest
            {
                LoginId = PART_UserName.Text,
                Password = PART_Password.Password
            };
            PART_ErrorMessage.Text = "";
            PART_ErrorMsgPanel.Visibility = Visibility.Hidden;
            PART_LoginProgessbar.Visibility = Visibility.Visible;

            var response = await ServiceProvider.GetInstance().Authenticate(request);

            PART_LoginProgessbar.Visibility = Visibility.Hidden;

            if (response.StatusCode == 200)
            {
                MainWindowViewModel viewModel = new MainWindowViewModel
                {
                    UserName = response.UserName,
                    UserRole = response.UserRole,
                    LoginWindow = this
                };
                MainWindow mainWindow = new MainWindow
                {
                    DataContext = viewModel
                };

                this.Hide();
                mainWindow.ShowDialog();

                //ProductViewModel viewModel = new ProductViewModel();
                //ProductView productView = new ProductView
                //{
                //    DataContext = viewModel
                //};

                //this.Hide();
                //productView.ShowDialog();
            }
            else
            {
                PART_ErrorMessage.Text = response.Message;
                PART_ErrorMsgPanel.Visibility = Visibility.Visible;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PART_LoginProgessbar.Visibility = Visibility.Visible;
            this.IsEnabled = false;

            var response = await ServiceProvider.GetInstance().CallWebApi<object, AuthenticateResponse>("Login/FetchMe", HttpMethod.Post, null);
            if (response.StatusCode == 200)
            {
                MainWindowViewModel viewModel = new MainWindowViewModel
                {
                    UserName = response.UserName,
                    UserRole = response.UserRole,
                    LoginWindow = this
                };
                MainWindow mainWindow = new MainWindow
                {
                    DataContext = viewModel
                };

                this.Hide();
                mainWindow.ShowDialog();
            }

            this.IsEnabled = true;
            PART_UserName.Focus();
            PART_LoginProgessbar.Visibility = Visibility.Hidden;
        }

        private async void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender.GetType() == typeof(TextBox))
                {
                    PART_Password.Focus();
                }
                else
                {
                    await Login();
                }
            }
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PART_UserName.Focus();
        }
    }
}
