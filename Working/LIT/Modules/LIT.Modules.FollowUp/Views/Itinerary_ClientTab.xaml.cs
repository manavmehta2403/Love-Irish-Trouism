using LIT.Modules.TabControl.ViewModels;
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

namespace LIT.Modules.TabControl.Views
{
    /// <summary>
    /// Interaction logic for Itinerary_ClientTab.xaml
    /// </summary>
    public partial class Itinerary_ClientTab : UserControl
    {
        public Itinerary_ClientTabViewModel Itinerary_ClientTabViewModel { get; set; }
        public Itinerary_ClientTab()
        {
            InitializeComponent();
            cltbmore.Visibility = Visibility.Collapsed;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tt= sender as TextBox;
            var bindingExpression = tt.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
        }

        private void defaultprice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPriceoverride_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void Txtcommisionoverride_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtagentnet_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtsellprice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtsale_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtpaymentamount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }

        }

        private void txtexchangerate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtPayrefundamount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtfee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void defaultprice_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Tag = string.Empty;
                }
            }
        }

        private void TxtCommissionpercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Tag = string.Empty;
                }
            }

        }
    }
}
