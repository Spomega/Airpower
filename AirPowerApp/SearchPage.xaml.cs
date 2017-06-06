using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace AirPowerApp
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }


        private void AppBarlibrary_Click(object sender, EventArgs e)
        {
            LibraryPage.visited = false;
            NavigationService.Navigate(new Uri("/LibraryPage.xaml", UriKind.Relative));
        }



        private void AppBarShare_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareTask = new ShareStatusTask();
            shareTask.Status = "Hello,I listen to Kakra Baiden on-the-go with his AirPower app and I thought you'd like it\n\n"
            + "Install now! http://getonairpower.com ";
            shareTask.Show();
        }
    }
}