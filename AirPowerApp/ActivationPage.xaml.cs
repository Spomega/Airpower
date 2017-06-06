﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.IO.IsolatedStorage;

namespace AirPowerApp
{
    public partial class ActivationPage : PhoneApplicationPage
    {
        string email = String.Empty;
        public ActivationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txtActivate.Text = RegistrationPage.activationCode;
        }

        private async void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator.IsVisible = true;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
            //request server url
            String url = AirPowerConstants.server_url + AirPowerConstants.ACTIVATE;


             //add request parameters
            var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("activation_code",txtActivate.Text),
                new KeyValuePair<String,String>("email",RegistrationPage.email)
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



        private void parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            if (returnObject.response_code == 200)
            {
                setSettings("user",returnObject.user.email);
                setSettings("access_token", returnObject.access_token);
                NavigationService.Navigate(new Uri("/CategoryPage.xaml", UriKind.Relative));
            }
                
            else
                MessageBox.Show("Activation Failed");

        }

        void setSettings(string key, string value)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (!settings.Contains(key))
            {
                settings.Add(key, value);
            }
            else
            {

            }

            settings.Save();
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
            public string access_token { get; set; }
        }

    }
}