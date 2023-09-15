using SellManagement.DesktopClient.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace SellManagement.DesktopClient.UserControls
{
    /// <summary>
    /// Interaction logic for ListSellOrderUserControl.xaml
    /// </summary>
    public partial class ListSellOrderUserControl : UserControl
    {
        public ListSellOrderUserControl()
        {
            InitializeComponent();
            if (DataContext is IOpenNewAble)
            {
                (DataContext as IOpenNewAble).RequestOpen += (_, __) => OpenNewTab(__.OpenUserControl, __.Header);
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (this.DataContext as ListSellOrderViewModel).IsVisble = this.IsVisible;
        }

        void OpenNewTab(UserControl userControl, string header)
        {
            var existingTabItem = PART_TabMain.Items.Cast<TabItem>()
                                    .Where(item => item.Header.ToString() == header)
                                    .FirstOrDefault();

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
            Binding SellOrderNoBinding = new Binding("SellOrderNo");
            SellOrderNoBinding.Source = userControl.DataContext;
            newTabItem.SetBinding(TabItem.HeaderProperty, SellOrderNoBinding);

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
                (this.DataContext as ListSellOrderViewModel).IsVisble = this.IsVisible;

            lastSelectedIndex = PART_TabMain.SelectedIndex;
        }
    }
}
