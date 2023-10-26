using LIT.Modules.TabControl.Commands;
using LIT.Modules.TabControl.ViewModels;
using LITModels.LITModels.Models;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ItineraryClientTab.xaml
    /// </summary>
    public partial class ItineraryClientTab : UserControl
    {
        
        public ItineraryClientTabViewModel CTviewModel { get; set; }
        public Itinerary_ClientTabPassengerCommand ctcommand { get; set; }
        public ItineraryClientTab()
        {
            InitializeComponent();
            cltbmore.Visibility = Visibility.Collapsed;
            CTviewModel = new ItineraryClientTabViewModel();
            ctcommand =new Itinerary_ClientTabPassengerCommand(CTviewModel);
            CTviewModel.Groupoption = true;
            RBGroup.IsChecked = true;

        }
        private void txtpaxoverride_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tt= sender as TextBox;
            var bindingExpression = tt.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
            if (tt != null && (lbltotalpassengerval!= null && Convert.ToInt64(lbltotalpassengerval.Content)> 0))
            {
                if (Convert.ToInt64(tt.Text) > 0)
                {
                    if (Convert.ToInt64(tt.Text) > Convert.ToInt64(lbltotalpassengerval.Content))
                    {
                        MessageBox.Show("Pax override should not be greater than Total passenger");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Pax override should not be zero");
                    return;
                }
            }
                
        }
        private void OnLeadPassengerSelect(object sender, RoutedEventArgs e)
        {
           if (((System.Windows.UIElement)sender).IsKeyboardFocusWithin == true)
            {
                string Passengerid = string.Empty;
                Passengerid = ((SQLDataAccessLayer.Models.PassengerDetails)((System.Windows.FrameworkElement)sender).DataContext).Passengerid;

                CTviewModel.PassengerDetailsobser = ((LIT.Modules.TabControl.ViewModels.ItineraryClientTabViewModel)dgPassengerdetails.DataContext).PassengerDetailsobser;
                if (CTviewModel.PassengerDetailsobser.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Passengerid))
                    {
                        CTviewModel.PassengerDetailsobser.ToList().ForEach(x => { x.LeadPassenger = false; });
                        CTviewModel.PassengerDetailsobser.Where(x => x.Passengerid == Passengerid).FirstOrDefault().LeadPassenger = true;
                        dgPassengerdetails.ItemsSource = null;
                        dgPassengerdetails.ItemsSource = CTviewModel.PassengerDetailsobser;
                    }
                }
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

        private void txtroomsbkd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtpaxsold_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }

        }

        private void txtroomssold_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtpaxbkd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }
        private void txtAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private void txtFeePercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }
    }
}
