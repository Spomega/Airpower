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
using Microsoft.Phone.Tasks;

namespace AirPowerApp
{
    public partial class MpowerConfirmPage : PhoneApplicationPage
    {
        public MpowerConfirmPage()
        {
            InitializeComponent();
        }

        private async void btnMpowerConfirm_Click(object sender, RoutedEventArgs e)
        {

            if (PartnerPage.partner)
            {
                if (txtConfirmCode.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Enter Confirmation Token");
                }
                else
                {
                    WaitIndicator.IsVisible = true;
                    var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                    //request server url
                    String url = AirPowerConstants.server_url + AirPowerConstants.MPOWER_CONFIRM;

                    //add request parameters
               var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",CategoryPage.user),
                new KeyValuePair<String,String>("item_id","100"),
                new KeyValuePair<String,String>("invoice_token",MpowerPaymentPage.invoice_token),
                new KeyValuePair<String,String>("confirm_token",txtConfirmCode.Text),
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
                if (txtConfirmCode.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Enter Confirmation Token");
                }
                else
                {
                    WaitIndicator.IsVisible = true;
                    var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                    //request server url
                    String url = AirPowerConstants.server_url + AirPowerConstants.MPOWER_CONFIRM;

                    //add request parameters
                    var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",CategoryPage.user),
                new KeyValuePair<String,String>("item_id",MediaPage.item_id),
                new KeyValuePair<String,String>("invoice_token",MpowerPaymentPage.invoice_token),
                new KeyValuePair<String,String>("confirm_token",txtConfirmCode.Text)
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

            if (returnObject.response_code == 200)
            {
                MessageBox.Show("Payment successful continue to your library");
                NavigationService.Navigate(new Uri("/LibraryPage.xaml", UriKind.Relative));
            }
            else
                MessageBox.Show("Payment Not successful please try again later");
            // MessageBox.Show("Categories " +categories.Count);



        }


        private void AppBarlibrary_Click(object sender, EventArgs e)
        {
            LibraryPage.visited = false;
            NavigationService.Navigate(new Uri("/LibraryPage.xaml", UriKind.Relative));
        }



        private void AppBarShare_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareTask = new ShareStatusTask();
            shareTask.Status = "Hello,I listen to Kakra Baiden on-the-go with his AirPower app and I thought you'd like it\n\n"
            + "Install now! http://getonairpower.com ";
            shareTask.Show();
        }

        private void menuPartner_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PartnerPage.xaml", UriKind.Relative));
        }

        public class Item
        {
            public string id { get; set; }
            public string label { get; set; }
            public string description { get; set; }
            public string author { get; set; }
            public string price { get; set; }
            public string currency { get; set; }
            public string thumbnail_url { get; set; }
            public string preview_url { get; set; }
            public string download_url { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public string user_id { get; set; }
            public string category_id { get; set; }
            public string type { get; set; }
        }

        public class ItemLibrary
        {
            public Item item { get; set; }
            public string status { get; set; }
        }

        public class RootObject
        {
            public int response_code { get; set; }
            public string invoice_status { get; set; }
            public string invoice_response { get; set; }
            public string invoice_description { get; set; }
            public List<ItemLibrary> item_library { get; set; }
        }
    }
}