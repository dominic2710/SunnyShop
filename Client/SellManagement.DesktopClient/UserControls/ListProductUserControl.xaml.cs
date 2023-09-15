using SellManagement.DesktopClient.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SellManagement.DesktopClient.UserControls
{
    /// <summary>
    /// Interaction logic for ListProductUserControl.xaml
    /// </summary>
    public partial class ListProductUserControl : UserControl
    {
        public ListProductUserControl()
        {
            InitializeComponent();
            if (DataContext is IOpenNewAble)
            {
                (DataContext as IOpenNewAble).RequestOpen += (_, __) => OpenNewTab(__.OpenUserControl, __.Header);
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (this.DataContext as ListProductViewModel).IsVisble = this.IsVisible;
        }

        void OpenNewTab(UserControl userControl, string header)
        {
            var existingTabItem = PART_TabMain.Items.Cast<TabItem>().FirstOrDefault(item => item.Header.ToString() == header);

            if (existingTabItem != null)
            {
                PART_TabMain.SelectedItem = existingTabItem;
                return;
            }

            var newTabItem = new TabItem
            {
                Header = header,
                Content = userControl
            };
            Binding productCdBiding = new Binding("ProductCd");
            productCdBiding.Source = userControl.DataContext;
            newTabItem.SetBinding(TabItem.HeaderProperty, productCdBiding);

            PART_TabMain.Items.Add(newTabItem);
            PART_TabMain.SelectedItem = newTabItem;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var tabItem = PART_TabMain.Items[0] as TabItem;
            var dockPanel = tabItem.Template.FindName("PART_Dock", tabItem) as DockPanel;

            if (dockPanel == null) return;
            if (dockPanel.Children.Count < 2) return;

            dockPanel.Children.RemoveAt(1);
        }
        private void btnCloseTab_Click(object sender, RoutedEventArgs e)
        {
            var tabItem = ((sender as Button).Parent as DockPanel).TemplatedParent as TabItem;

            if (tabItem == null) return;

            PART_TabMain.SelectedIndex = 0;
            PART_TabMain.Items.Remove(tabItem);
        }

        int lastSelectedIndex = 0;

        private void PART_TabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lastSelectedIndex == PART_TabMain.SelectedIndex)
                return;

            if (PART_TabMain.SelectedIndex == 0)
                (this.DataContext as ListProductViewModel).IsVisble = this.IsVisible;

            lastSelectedIndex = PART_TabMain.SelectedIndex;
        }
    }
}
