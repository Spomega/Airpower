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
    public partial class MpowerPaymentPage : PhoneApplicationPage
    {

        public static string invoice_token = String.Empty;
        public MpowerPaymentPage()
        {
            InitializeComponent();
        }



        private async void btnMpowerPay_Click(object sender, RoutedEventArgs e)
        {

            if (PartnerPage.partner)
            {

                if (txtUsername.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Enter UserName");
                }
                else
                {
                    WaitIndicator.IsVisible = true;
                    var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                    //request server url
                    String url = AirPowerConstants.server_url + AirPowerConstants.MPOWER_PAY;

                    //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",CategoryPage.user),
                new KeyValuePair<String,String>("item_id","100"),
                new KeyValuePair<String,String>("username",txtUsername.Text),
                new KeyValuePair<String,String>("amount",PartnerPage.amount)
            };
                    // MessageBox.Show(""+parameters.Capacity);

                    String response = await Util.httpHelperPost(url, parameters);
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
            }
            else
            {

                if (txtUsername.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Enter UserName");
                }
                else
                {
                    WaitIndicator.IsVisible = true;
                    var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                    //request server url
                    String url = AirPowerConstants.server_url + AirPowerConstants.MPOWER_PAY;

                    //add request parameters
                    var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",CategoryPage.user),
                new KeyValuePair<String,String>("item_id",MediaPage.item_id),
                new KeyValuePair<String,String>("username",txtUsername.Text)
            };
                    // MessageBox.Show(""+parameters.Capacity);

                    String response = await Util.httpHelperPost(url, parameters);
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
            }
         
        }


        private void parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            if (returnObject.response_code==200)
            {
                MessageBox.Show("Please Proceed to Finalize Payment With Confirmation Code "+ returnObject.invoice_token);
                invoice_token = returnObject.invoice_token;
                NavigationService.Navigate(new Uri("/MpowerConfirmPage.xaml", UriKind.Relative));
            }
               
            else
                MessageBox.Show("Payment Not Made");
            // MessageBox.Show("Categories " +categories.Count);

            


        }




        public class RootObject
        {
            public int response_code { get; set; }
            public string invoice_status { get; set; }
            public string invoice_token { get; set; }
        }







        }



    
}