using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;

namespace AirPowerApp
{
    public partial class PaymentPage : PhoneApplicationPage
    {

        public static bool visited = false;
        public PaymentPage()
        {
            InitializeComponent();


        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {



            if (!visited)
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                String url = String.Empty;
              
                if(CategoryPage.country.Equals("Ghana"))
                //request server url
                 url = AirPowerConstants.server_url + AirPowerConstants.GET_PAYOPTIONS + "?email=" + CategoryPage.user+"&country_code=GH";
                else
                    url = AirPowerConstants.server_url + AirPowerConstants.GET_PAYOPTIONS + "?email=" + CategoryPage.user + "&country_code="+CategoryPage.country;


                
                String response = await Util.httpHelperGetWithToken(url);
                // MessageBox.Show("response " + response);

                //when timer elapses
                timer.Tick += delegate
                {
                    if (response.Length > 0)
                    {
                        timer.Stop();
                        WaitIndicator.IsVisible = false;
                        parseJSONResponse(response);

                    }
                    else
                    {
                        timer.Stop();
                        WaitIndicator.IsVisible = false;
                        MessageBox.Show(AirPowerConstants.ERROR_MESSAGE);
                        //NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                    }
                };
                timer.Start();
            }

            base.OnNavigatedTo(e);
        }


        private void parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            if (returnObject.response_code == 200)
            {
                lstpaymentoption.Items.Add("--Select Payment Option--");

                foreach (var payoption in returnObject.payment_options)
                {
                    lstpaymentoption.Items.Add(payoption.name);
                }
            }
            else
                MessageBox.Show("Payment Options Not Available");

            visited = true;

        }


        public class PaymentOption
        {
            public string id { get; set; }
            public string name { get; set; }
            public string alias { get; set; }
            public string host { get; set; }
            public string port { get; set; }
            public string context_path { get; set; }
            public string protocol { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string api_key { get; set; }
        }

        public class RootObject
        {
            public int response_code { get; set; }
            public List<PaymentOption> payment_options { get; set; }
        }

       

        private void btnPayOption_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lstpaymentoption.SelectedItem;

            if(selectedItem.Equals("Mpower Payment"))
            {
                NavigationService.Navigate(new Uri("/MPowerPaymentPage.xaml", UriKind.Relative));
            }
            else if(selectedItem.Equals("Visa"))
            {
                NavigationService.Navigate(new Uri("/VisaPaymentPage.xaml", UriKind.Relative));
            }
            else if (selectedItem.Equals("MTN Mobile Money"))
            {
                NavigationService.Navigate(new Uri("/MtnPaymentPage.xaml", UriKind.Relative));
            }
            else if (selectedItem.Equals("Airtel Mobile Money"))
            {
                NavigationService.Navigate(new Uri("/AirtelMoneyPage.xaml", UriKind.Relative));
            }
            else if(selectedItem.Equals("--Select Payment Option--"))
            {
                MessageBox.Show("Select a Payment Option");
            }
            else if (selectedItem.Equals("Tigo Cash"))
            {
                NavigationService.Navigate(new Uri("/TigoPaymentPage.xaml", UriKind.Relative));
            }
            else if (selectedItem.Equals("Vodafone Cash"))
            {
                NavigationService.Navigate(new Uri("/VodafonePaymentPage.xaml", UriKind.Relative));
            }
        }
    }
}