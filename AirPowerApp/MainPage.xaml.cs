using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AirPowerApp.Resources;
using System.Windows.Media;
using System.IO.IsolatedStorage;

namespace AirPowerApp
{
    public partial class MainPage : PhoneApplicationPage
    {

        public static string email = String.Empty;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

           
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void btnGetStarted_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Email");
            }
            else
            {
                email = txtEmail.Text;
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Email");
            }
            else
            {
                email = txtEmail.Text;
                NavigationService.Navigate(new Uri("/RegistrationPage.xaml", UriKind.Relative));
            }
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
      

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}