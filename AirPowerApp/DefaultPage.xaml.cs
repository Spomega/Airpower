using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace AirPowerApp
{
    public partial class DefaultPage : PhoneApplicationPage
    {
        public DefaultPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    //if (IsolatedStorageSettings.ApplicationSettings.Contains("user"))
        //    //{
        //    //    NavigationService.Navigate(new Uri("/CategoryPage.xaml", UriKind.Relative));
        //    //}
        //    //base.OnNavigatedTo(e);
        //}
    }
}