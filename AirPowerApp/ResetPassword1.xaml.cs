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
    public partial class ResetPassword1 : PhoneApplicationPage
    {
        public ResetPassword1()
        {
            InitializeComponent();
        }

        public static string resetCode = null;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txtUsername.Text = MainPage.email;

        }


        private async void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {

            if (txtUsername.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Customer Name");
            }
            else
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AirPowerConstants.server_url + AirPowerConstants.PASSWORD_RESET;

                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",txtUsername.Text),
                new KeyValuePair<String,String>("activation_option","email"),
               
            };
                // MessageBox.Show(url);

                String response = await Util.httpHelperPost(url, parameters);
                //MessageBox.Show("response " + response);


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

        public class User
        {
            public string full_name { get; set; }
            public string gender { get; set; }
            public string age { get; set; }
            public string phone_number { get; set; }
            public string email { get; set; }
            public string updated_at { get; set; }
            public string created_at { get; set; }
            public string id { get; set; }
        }

        public class RootObject
        {
            public int response_code { get; set; }
            public User user { get; set; }
            public string reset_code { get; set; }
        }

        private void parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            // MessageBox.Show("in parse method " + returnObject.response_code);
            if (returnObject.response_code == 200)
            {
                resetCode = returnObject.reset_code;
                MessageBox.Show("Code :" + resetCode);

                NavigationService.Navigate(new Uri("/ResetPasswordPage2.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Sending Email Failed,Check And Try Again");

            }



        }
    }
}
      


       


    


      

    