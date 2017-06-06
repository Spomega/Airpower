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
using Microsoft.Phone.Tasks;

namespace AirPowerApp
{
    public partial class MediaPage : PhoneApplicationPage
    {

        public static string preview_url= String.Empty;
        public static string price = String.Empty;
        public static string tempprice = String.Empty;
        public static string thumbnail_url = String.Empty;
        public static string item_id = String.Empty;
        public static string title = String.Empty;
        public static string type = String.Empty;
        public static bool visited = false;
      
        public MediaPage()
        {
            InitializeComponent();
        }



        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CategoryPage.visited = true;
           
           if (!visited)
          {
              
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AirPowerConstants.server_url + AirPowerConstants.GET_ITEMS + "?category_id="+CategoryPage.category+"&email=" + CategoryPage.user;
                MessageBox.Show("response " + url);
                String response = await Util.httpHelperGetWithToken(url);
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
            try
            {
                RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

                string apCurrency = "GHS";

                if (!CategoryPage.country.Equals("Ghana"))
                    apCurrency = "USD";

                if (returnObject.response_code == 200)
                {
                    List<Audio> audioSermons = new List<Audio>();
                    List<Video> videoSermons = new List<Video>();
                    List<Pdf> pdf = new List<Pdf>();

                    foreach (Audio au in returnObject.items.audios)
                    {
                        if (!CategoryPage.country.Equals("Ghana"))
                            tempprice = au.international_usd_price + apCurrency;
                        else
                            tempprice = au.price + apCurrency;

                        audioSermons.Add(new Audio()
                        {
                            thumbnail_url = au.thumbnail_url,
                            price = tempprice,
                            preview_url = au.preview_url,
                            download_url = au.download_url,
                            description = au.label + "\n" + "\t" + tempprice,
                            currency = apCurrency,
                            id = au.id
                        });

                    }

                    foreach (Video vd in returnObject.items.videos)
                    {
                        if (!CategoryPage.country.Equals("Ghana"))
                            tempprice =vd.international_usd_price + apCurrency;
                        else
                            tempprice = vd.price + apCurrency;
                        videoSermons.Add(new Video()
                        {
                            thumbnail_url = vd.thumbnail_url,
                            price = tempprice,
                            preview_url = vd.preview_url,
                            download_url = vd.download_url,
                            description = vd.label + "\n" + "\t" + price,
                            currency = apCurrency,
                            id = vd.id
                        });
                    }
                    foreach (Pdf pd in returnObject.items.pdfs)
                    {
                        if (!CategoryPage.country.Equals("Ghana"))
                            tempprice = pd.international_usd_price + apCurrency;
                        else
                            tempprice = pd.price + apCurrency;
                        pdf.Add(new Pdf()
                        {
                            thumbnail_url = pd.thumbnail_url,
                            price = tempprice,
                            preview_url = pd.preview_url,
                            download_url = pd.download_url,
                            description = pd.label + "\n" + "\t" + tempprice,
                            currency = apCurrency,
                            id = pd.id
                        });
                    }

                    lstBoxAudio.ItemsSource = audioSermons;
                    lstBoxVideo.ItemsSource = videoSermons;
                    lstBoxPDF.ItemsSource = pdf;

                    // MessageBox.Show("Categories " +categories.Count);
                }
                else
                    MessageBox.Show("Could Not Retrieve Sermons For This Category");

            }
            catch(Exception e )
            {
                MessageBox.Show(AirPowerConstants.ERROR_MESSAGE);
            }


        }


        public class SermonTempData
        {
            public String Title { get; set; }
            public string Image { get; set; }

            public string Description { get; set; }
        }


         public class Audio
        {
            public string id { get; set; }
            public string label { get; set; }
            public string description { get; set; }
            public string author { get; set; }
            public string price { get; set; }
            public string usd_price { get; set; }
            public string international_usd_price { get; set; }
            public string currency { get; set; }
            public string thumbnail_url { get; set; }
            public string preview_url { get; set; }
            public string download_url { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public string user_id { get; set; }
            public string category_id { get; set; }
            public string type { get; set; }
        }

        public class Video
        {
            public string id { get; set; }
            public string label { get; set; }
            public string description { get; set; }
            public string author { get; set; }
            public string price { get; set; }
            public string usd_price { get; set; }
            public string international_usd_price { get; set; }
            public string currency { get; set; }
            public string thumbnail_url { get; set; }
            public string preview_url { get; set; }
            public string download_url { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public object user_id { get; set; }
            public string category_id { get; set; }
            public string type { get; set; }
        }

        public class Pdf
        {
            public string id { get; set; }
            public string label { get; set; }
            public string description { get; set; }
            public string author { get; set; }
            public string price { get; set; }
            public string international_usd_price { get; set; }
            public string usd_price { get; set; }
            public string currency { get; set; }
            public string thumbnail_url { get; set; }
            public string preview_url { get; set; }
            public string download_url { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public object user_id { get; set; }
            public string category_id { get; set; }
            public string type { get; set; }
        }

        public class Items
        {
            public List<Audio> audios { get; set; }
            public List<Video> videos { get; set; }
            public List<Pdf> pdfs { get; set; }
        }

        public class RootObject
        {
            public int response_code { get; set; }
            public Items items { get; set; }
        }

        private void lstBoxAudio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected =(Audio) lstBoxAudio.SelectedItem;
           
            if (selected != null)
            {
              // MessageBox.Show(" " + selected.preview_url);
                preview_url = selected.preview_url;
                thumbnail_url = selected.thumbnail_url;
                item_id = selected.id;
                title = selected.description;
                type = selected.type;

            }
          //  NavigationService.Navigate(new Uri(string.Format("/MediaPage.xaml?Refresh=true&random={0}", Guid.NewGuid())));
          //  MessageBox.Show(" " + preview_url);
            refreshAudioList();
            NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));


        }

      
        private void AppBarlibrary_Click(object sender, EventArgs e)
        {
            LibraryPage.visited = false;
            NavigationService.Navigate(new Uri("/LibraryPage.xaml", UriKind.Relative));
        }

        private void lstBoxVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (Video)lstBoxVideo.SelectedItem;

            if (selected != null)
            {
                preview_url = selected.preview_url;
                thumbnail_url = selected.thumbnail_url;
                item_id = selected.id;
                title = selected.description;

            }
            //NavigationService.Navigate(new Uri(string.Format(NavigationService.Source +
            //                        "?Refresh=true&random={0}", Guid.NewGuid())));
            refreshVideoList();
            NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
        }

        private void lstBoxPDF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (Pdf)lstBoxPDF.SelectedItem;

            if (selected != null)
            {
                preview_url = selected.preview_url;
                thumbnail_url = selected.thumbnail_url;
                item_id = selected.id;
                title = selected.description;

            }
            refreshPdfList();
            NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));


        }

        private void refreshAudioList()
        {
            lstBoxAudio.SelectedItem = null;
        }

        private void refreshVideoList()
        {
            lstBoxVideo.SelectedItem = null;
        }

        private void refreshPdfList()
        {
            lstBoxPDF.SelectedItem = null;
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


    
