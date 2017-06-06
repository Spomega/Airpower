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
    public partial class ForgetPasswordPage : PhoneApplicationPage
    {

        public static string activationcode = null;
        public static string email = null;
        public ForgetPasswordPage()
        {
            InitializeComponent();
        }

        private async void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Email");
            }
            else if (txtPhonenumber.Equals(String.Empty))
            {

                MessageBox.Show("Enter Phonenumber");
            }
            else
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AirPowerConstants.server_url + AirPowerConstants.FORGET_PASSWORD;

                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",txtUsername.Text),
                new KeyValuePair<String,String>("phone_number",txtPhonenumber.Text),
               
            };

                String response = await Util.httpHelperPost(url, parameters);
                MessageBox.Show("response " + response);

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




        private void parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            // MessageBox.Show("in parse method " + returnObject.response_code);
            if (returnObject.response_code == 200)
            {

               // MessageBox.Show("Message:" + returnObject.message);
                activationcode = returnObject.forgot_password_code;
                email = returnObject.user.email;

                NavigationService.Navigate(new Uri("/ForgetPassConfirmPage.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Sending Email Failed,Check And Try Again");

            }



        }



        public class User
        {
            public int id { get; set; }
            public string email { get; set; }
            public bool activated { get; set; }
            public object activated_at { get; set; }
            public string last_login { get; set; }
            public string full_name { get; set; }
            public string created_at { get; set; }
            public object gender { get; set; }
            public object age { get; set; }
            public string phone_number { get; set; }
            public string updated_at { get; set; }
            public string deleted { get; set; }
        }

        public class RootObject
        {
            public int response_code { get; set; }
            public User user { get; set; }
            public string forgot_password_code { get; set; }
        }
    }
}