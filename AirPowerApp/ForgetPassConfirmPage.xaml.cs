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
    public partial class ForgetPassConfirmPage : PhoneApplicationPage
    {

        public static List<Category> categories = new List<Category>();
        public static string accessToken = null;
        public static string email = null;
        public ForgetPassConfirmPage()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            txtActivation.Text = ForgetPasswordPage.activationcode;
            base.OnNavigatedTo(e);
        }

        private async void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            if (txtActivation.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Email");
            }
            else
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AirPowerConstants.server_url + AirPowerConstants.FORGET_PASSWORD_CONFIRM;

                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",ForgetPasswordPage.email),
                //new KeyValuePair<String,String>("phone_number",txtPhonenumber.Text),
               
            };

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


        private void parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            // MessageBox.Show("in parse method " + returnObject.response_code);
            if (returnObject.response_code == 200)
            {
                categories = returnObject.categories;
                accessToken = returnObject.access_token;
                email = returnObject.user.email;
                // MessageBox.Show("Categories " +categories.Count);
                LoginPage.setSettings("user", email);
                LoginPage.setSettings("access_token", accessToken);

                NavigationService.Navigate(new Uri("/CategoryPage.xaml", UriKind.RelativeOrAbsolute));

            }
            else
            {
                MessageBox.Show("Sign In Failed,Check Email And Password And Try Again");

            }



        }


        public class User
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string gender { get; set; }
            public string age { get; set; }
            public string phone_number { get; set; }
            public string email { get; set; }
            public string updated_at { get; set; }
            public string created_at { get; set; }
            public string id { get; set; }
        }

        public class Category
        {
            public string id { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public string name { get; set; }

            public string thumbnail_url { get; set; }
        }

        public class RootObject
        {
            public int response_code { get; set; }
            public User user { get; set; }
            public List<Category> categories { get; set; }
            public string access_token { get; set; }
        }
    }
}