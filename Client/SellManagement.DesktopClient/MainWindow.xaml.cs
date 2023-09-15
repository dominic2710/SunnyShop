using SellManagement.DesktopClient.ViewModels;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace SellManagement.DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool IsLogout;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsLogout = false;
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            if (DataContext is ICloseable)
            {
                (DataContext as ICloseable).RequestClose += (_, __) =>
                {
                    IsLogout = true;
                    this.Close();
                };
            }

            MainWindow mainWindow = this;
            Screen screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);

            if (screen.WorkingArea.Width <= Width || screen.WorkingArea.Height <= Height)
                WindowState = WindowState.Maximized;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsLogout) return;
            (DataContext as MainWindowViewModel).LoginWindow.Close();
        }
    }
}
