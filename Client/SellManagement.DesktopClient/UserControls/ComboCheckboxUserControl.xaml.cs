using Newtonsoft.Json.Linq;
using SellManagement.DesktopClient.Models;
using SellManagement.DesktopClient.ViewModels;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SellManagement.DesktopClient.UserControls
{
    /// <summary>
    /// Interaction logic for ComboCheckboxUserControl.xaml
    /// </summary>
    public partial class ComboCheckboxUserControl : UserControl
    {
        public ComboCheckboxUserControl()
        {
            InitializeComponent();
        }

        private bool IsUpdatingCheckState = false;
        private ObservableCollection<FilterItemModel> ItemsSourceFiltered;

        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(ObservableCollection<FilterItemModel>), typeof(ComboCheckboxUserControl), new PropertyMetadata(null, ItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ObservableCollection<FilterItemModel>), typeof(ComboCheckboxUserControl), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register("SelectionChangedCommand", typeof(ICommand), typeof(ComboCheckboxUserControl), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectionChangedCommandParameterProperty =
            DependencyProperty.Register("SelectionChangedCommandParameter", typeof(object), typeof(ComboCheckboxUserControl), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ComboCheckboxUserControl), new PropertyMetadata(null, TitleChanged));


        [Bindable(true)]
        [Category("Action")]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        public object SelectionChangedCommandParameter
        {
            get { return GetValue(SelectionChangedCommandParameterProperty); }
            set { SetValue(SelectionChangedCommandParameterProperty, value); }
        }

        public ObservableCollection<FilterItemModel> ItemSource
        {
            get { return (ObservableCollection<FilterItemModel>)GetValue(ItemSourceProperty); }
            set {
                SetValue(ItemSourceProperty, value);
            }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ComboCheckboxUserControl;
            control.PART_ListItem.ItemsSource = e.NewValue as ObservableCollection<FilterItemModel>;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        private static void TitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ComboCheckboxUserControl;
            control.PART_Title.Text = e.NewValue as string;
        }

        public ObservableCollection<FilterItemModel> SelectedItem
        {
            get { return (ObservableCollection<FilterItemModel>)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        private void PART_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateFilterList(sender);
            SelectedItem.Add(((sender as CheckBox).Tag as FilterItemModel));
            SelectionChangedCommand.Execute(SelectionChangedCommandParameter);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedItem = new ObservableCollection<FilterItemModel>(ItemSource.Where(x => x.IsChecked == true));
            PART_Title.Text = $"{Title}: {ShareContanst.SELECT_ALL}";
            this.ToolTip = $"{Title}: {ShareContanst.SELECT_ALL}";
        }

        private void PART_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateFilterList(sender);
            SelectedItem.Remove(((sender as CheckBox).Tag as FilterItemModel));
            SelectionChangedCommand.Execute(SelectionChangedCommandParameter);
        }

        private void UpdateFilterList(object targetCheckbox)
        {
            if (IsUpdatingCheckState) return;
            IsUpdatingCheckState = !IsUpdatingCheckState;

            CheckBox checkBox = targetCheckbox as CheckBox;

            if (checkBox.Tag != null)
            {
                //if check into SelectAll
                if ((checkBox.Tag as FilterItemModel).DisplayName == ShareContanst.SELECT_ALL)
                {
                    if ((bool)checkBox.IsChecked)
                    {
                        foreach (var item in ItemSource)
                            item.IsChecked = true;
                    }
                    else
                    {
                        foreach (var item in ItemSource)
                            item.IsChecked = false;
                    }
                }
                else
                {
                    var item = ItemSource
                                    .Where(x => x.IsChecked == false)
                                    .Where(x => x.DisplayName != ShareContanst.SELECT_ALL)
                                    .FirstOrDefault();
                    //If all of item is checked
                    if (item == null)
                    {
                        ItemSource.Where(x => x.DisplayName == ShareContanst.SELECT_ALL).SingleOrDefault().IsChecked = true;
                    }
                    else
                    {
                        ItemSource.Where(x => x.DisplayName == ShareContanst.SELECT_ALL).SingleOrDefault().IsChecked = false;
                    }
                }
            }

            bool isSelectAll = ItemSource.Where(x => x.DisplayName == ShareContanst.SELECT_ALL)
                                                .Where(x => x.IsChecked == true).Any();
            string selectedDisplayName;
            if (isSelectAll)
            {
                selectedDisplayName = ShareContanst.SELECT_ALL;
                SetIndicator(false);
            }
            else
            {
                selectedDisplayName = string.Join(", ", ItemSource.Where(x => x.IsChecked == true).Select(x => x.DisplayName));
                SetIndicator(true);
            }
            
            PART_Title.Text = $"{Title}: {selectedDisplayName}";
            this.ToolTip = $"{Title}: {selectedDisplayName}";

            IsUpdatingCheckState = !IsUpdatingCheckState;
        }

        private void SetIndicator(bool isShow)
        {
            if (isShow)
            {
                PART_ShowFilterIndicator.Visibility = Visibility.Visible;
                PART_HideFilterIndicator.Visibility = Visibility.Collapsed;
                PART_Title.Foreground = new SolidColorBrush(Color.FromRgb(33,150,243));
            }
            else
            {
                PART_ShowFilterIndicator.Visibility = Visibility.Collapsed;
                PART_HideFilterIndicator.Visibility = Visibility.Visible;
                PART_Title.Foreground = new SolidColorBrush(Colors.Gray);

            }
        }

        private void PART_FilterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ItemsSourceFiltered = new ObservableCollection<FilterItemModel>(this.ItemSource
                                        .Where(x => string.IsNullOrEmpty(PART_FilterText.Text)
                                                || x.DisplayName.ToLower().Contains(PART_FilterText.Text.ToLower())));
            PART_ListItem.ItemsSource = ItemsSourceFiltered;
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e)
        {
            PART_FilterText.Focus();
        }

        private void PopupBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PART_FilterText.Focus();
        }
    }
}
