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
    public partial class RegistrationPage : PhoneApplicationPage
    {

        public static string activationCode = String.Empty;
        public static string email = String.Empty;
        public RegistrationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txtEmail.Text = MainPage.email;
        }



        private async void btnSignUp_Click(object sender, RoutedEventArgs e)
        {

            if (txtName.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter  Name");
            }
            else if (txtEmail.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Email");
            }
            else if (txtPassword.Password.Equals(String.Empty))
            {

                MessageBox.Show("Enter Password");
            }
            else if (txtPasswordConfirm.Password.Equals(String.Empty))
            {

                MessageBox.Show("Confirm Password");
            }
            else if (!txtPassword.Password.Equals(txtPasswordConfirm.Password))
            {

                MessageBox.Show("Password and Confirmation  Password Do Not Match");
            }

            else
            {

                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AirPowerConstants.server_url + AirPowerConstants.REGISTER;


                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",txtEmail.Text),
                new KeyValuePair<String,String>("password",txtPassword.Password),
                new KeyValuePair<String,String>("full_name",txtName.Text),
                new KeyValuePair<String,String>("activation_option","sms")
            };

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

        private void parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            activationCode = returnObject.activation_code;
            email = returnObject.user.email;

            if (returnObject.response_code == 200)
                NavigationService.Navigate(new Uri("/ActivationPage.xaml", UriKind.Relative));
            else
                MessageBox.Show("Sign Up Unsuccessful,Check If Your Email is Correct and Try Again");

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

        public class RootObject
        {
            public int response_code { get; set; }
            public User user { get; set; }
            public string activation_code { get; set; }
        }

    }
}