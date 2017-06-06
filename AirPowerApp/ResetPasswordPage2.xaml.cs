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
    public partial class ResetPasswordPage2 : PhoneApplicationPage
    {
        public ResetPasswordPage2()
        {
            InitializeComponent();
        }

        private async void btnConfirmReset_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Equals(String.Empty))
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
                String url = AirPowerConstants.server_url + AirPowerConstants.PASSWORD_RESET;

                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",txtEmail.Text),
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



        private void parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            // MessageBox.Show("in parse method " + returnObject.response_code);
            if (returnObject.response_code == 200)
            {
              
                MessageBox.Show("Message:" + returnObject.message);


                NavigationService.Navigate(new Uri("/ResetPasswordPage2.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Sending Email Failed,Check And Try Again");

            }



        }


        public class RootObject
        {
            public int response_code { get; set; }
            public string message { get; set; }
        }
    }
}