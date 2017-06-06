using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AirPowerApp
{
    public partial class PartnerPage : PhoneApplicationPage
    {

        public static bool partner = false;
        public static string amount = String.Empty;
        public PartnerPage()
        {
            InitializeComponent();
            CategoryPage.visited = true;
        }

        private void btnDonate_Click(object sender, RoutedEventArgs e)
        {
            if (txtAmount.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Amount");
            }
            else
            {

                amount = txtAmount.Text;
                partner = true;
                PaymentPage.visited = false;
                NavigationService.Navigate(new Uri("/PaymentPage.xaml", UriKind.Relative));
            }
            
        }




    }
}