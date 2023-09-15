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
    /// Interaction logic for NumericUpDownUserControl.xaml
    /// </summary>
    public partial class NumericUpDownUserControl : UserControl
    {
        public NumericUpDownUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDownUserControl), new UIPropertyMetadata(0,textChangedCallback));
        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(int), typeof(NumericUpDownUserControl), new UIPropertyMetadata(0, textChangedCallback));
        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(int), typeof(NumericUpDownUserControl), new UIPropertyMetadata(0, textChangedCallback));
        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(int), typeof(NumericUpDownUserControl), new UIPropertyMetadata(0, textChangedCallback));
        public event EventHandler ValueChanged;

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }
        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        public int Step
        {
            get { return (int)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        private static void textChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDownUserControl input = d as NumericUpDownUserControl;
            input.PART_Value.Text = e.NewValue.ToString();
        }

        private void PART_Plus_Click(object sender, RoutedEventArgs e)
        {
            Value += Step;
            if (Value > Max) Value = Max;
            PART_Value.Text = Value.ToString();
        }

        private void PART_Minus_Click(object sender, RoutedEventArgs e)
        {
            Value -= Step;
            if (Value < Min) Value = Min;
            PART_Value.Text = Value.ToString();
        }

        private void PART_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            int result = 0;
            if (int.TryParse(PART_Value.Text, out result))
            {
                if (result > Max) result = Max;
                if (result < Min) result = Min;

                Value = result;
                PART_Value.Text = Value.ToString();
            }
            else
                PART_Value.Text = Value.ToString();

            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        private void PART_Value_GotFocus(object sender, RoutedEventArgs e)
        {
            PART_Value.SelectAll();
        }
    }
}
