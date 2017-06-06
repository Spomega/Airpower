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
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace AirPowerApp
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
           
        }

        public static List<Category> categories = new List<Category>();
        public static string accessToken = null;
        public static string email = null;



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txtEmail.Text = MainPage.email;
            
        }

        private async  void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Email");
            }
            else if(txtPassword.Password.Equals(String.Empty))
            {
                MessageBox.Show("Enter Password");
            }
            else
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AirPowerConstants.server_url + AirPowerConstants.LOGIN;

                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",txtEmail.Text),
                new KeyValuePair<String,String>("password",txtPassword.Password)
            };
                // MessageBox.Show(""+parameters.Capacity); 

                String response = await Util.httpHelperPost(url, parameters);
                //MessageBox.Show("response " + response);


                //when timer elapses
                timer.Tick += delegate
                {
                    if (response.Length > 0)
                    {
                        timer.Stop();
                        WaitIndicator.IsVisible = false;
                        parseJSONResponse1(response);
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
         // NavigationService.Navigate(new Uri("/CategoryPage.xaml", UriKind.Relative));
        }


        public async static Task<bool> facebookLogin(string name,string email,string facebook_token,string facebook_uid)
        {
           
           
            //request server url
            String url = AirPowerConstants.server_url + AirPowerConstants.FACEBOOK_LOGIN;

            //add request parameters
            var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("email",email),
                new KeyValuePair<String,String>("full_name",name),
                new KeyValuePair<String,String>("authentication_token",facebook_token),
                new KeyValuePair<String,String>("uid",facebook_uid),
                new KeyValuePair<String,String>("gender","male")
            };


            String response = await Util.httpHelperPost(url, parameters);
           // MessageBox.Show("response " + response);


           return  parseJSONResponse(response);
            
        }


        private void parseJSONResponse1(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

            // MessageBox.Show("in parse method " + returnObject.response_code);
            if (returnObject.response_code == 200)
            {
                categories = returnObject.categories;
                accessToken = returnObject.access_token;
                email = returnObject.user.email;
                // MessageBox.Show("Categories " +categories.Count);
                setSettings("user", email);
                setSettings("access_token", accessToken);

                NavigationService.Navigate(new Uri("/CategoryPage.xaml", UriKind.RelativeOrAbsolute));
                
            }
            else
            {
                 MessageBox.Show("Sign In Failed,Check Email And Password And Try Again");
                
            }



        }

        private static bool parseJSONResponse(String response)
        {
            RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

           // MessageBox.Show("in parse method " + returnObject.response_code);
              if (returnObject.response_code==200)
              { 
                categories = returnObject.categories;
                accessToken = returnObject.access_token;
                email = returnObject.user.email;
               // MessageBox.Show("Categories " +categories.Count);
                setSettings("user", email);
                setSettings("access_token", accessToken);

                //NavigationService.Navigate(new Uri("/CategoryPage.xaml", UriKind.RelativeOrAbsolute));
                return true;
            }
            else
              {
                 // MessageBox.Show("Sign In Failed,Check Email And Password And Try Again");
                  return false;
              }
               
           

        }


       public static void setSettings(string key,string value )
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if(!settings.Contains(key))
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

        private void forgotpassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ForgetPasswordPage.xaml", UriKind.Relative));
        }

    }
}