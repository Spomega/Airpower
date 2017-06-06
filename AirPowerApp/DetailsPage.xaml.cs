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
using System.Windows.Media.Imaging;

namespace AirPowerApp
{
    public partial class DetailsPage : PhoneApplicationPage
    {
       // public String url { get; set; }
        public DetailsPage()
        {
            InitializeComponent();
              //  new URL(){url=MediaPage.download_url};

          
            //mePlayAudio.DataContext = new URL() { url = MediaPage.download_url };
           // url = MediaPage.download_url;
        }


       

        private void btnPlayPreviev_Click(object sender, RoutedEventArgs e)
        {
          // MessageBox.Show("url " + MediaPage.download_url);
            mePlayAudio.Source =new Uri(MediaPage.preview_url,UriKind.Absolute);
           
            //mePlayAudio.Source = new Uri("/audio/supernatural_guidance.mp3", UriKind.Relative);
            mePlayAudio.Play();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            hubtile1.Message=  MediaPage.title;
            hubtile1.Source = new BitmapImage(new Uri(MediaPage.thumbnail_url, UriKind.Absolute));
            MediaPage.visited = true;
        }

        public class URL
        {
            public String url { get; set; }
        }

        private void price_Click(object sender, RoutedEventArgs e)
        {
            PaymentPage.visited = false;
            NavigationService.Navigate(new Uri("/PaymentPage.xaml", UriKind.Relative));
        }






    }
}