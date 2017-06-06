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
using Microsoft.Phone.Tasks;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Net.NetworkInformation;

namespace AirPowerApp
{
    public partial class CategoryPage : PhoneApplicationPage
    {
        public static int category = 0;
        public static string token = String.Empty;
        public static string user = String.Empty;
        public static string country = String.Empty;
        public static bool visited = false;
        public static string location = String.Empty;
        public CategoryPage()
        {
            InitializeComponent(); 

            
           token = IsolatedStorageSettings.ApplicationSettings["access_token"] as String;
           user = IsolatedStorageSettings.ApplicationSettings["user"] as String;
           
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
           
              
                if (!visited)
                {
                    getLocation();
                    WaitIndicator.IsVisible = true;
                    var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                    //request server url
                    String url = AirPowerConstants.server_url + AirPowerConstants.CATEGORIES + "?email=" + user;



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
                else
                {

                }
                //// else
                // {
                //     List<TileItem> tileItems = new List<TileItem>();
                //     foreach (AirPowerApp.LoginPage.Category c in LoginPage.categories)
                //     {
                //         tileItems.Add(new TileItem() { ImageUri = c.thumbnail_url, Title = c.name, GroupTag = "Category", CategoryId = c.id });
                //     }

                //     this.tileList.ItemsSource = tileItems;
                // }

            
           
    
           base.OnNavigatedTo(e);
           


        }

        private void parseJSONResponse(String response)
        {
            try
            {
                RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

                if (returnObject.response_code == 200)
                {
                    List<TileItem> tileItems = new List<TileItem>();
                    foreach (Category c in returnObject.categories)
                    {
                        MessageBox.Show("CategoryId " + c.id);
                        tileItems.Add(new TileItem() { ImageUri = c.thumbnail_url, Title = c.name +"\n"+c.item_count+" Sermon(s)", GroupTag = "Category", CategoryId = c.id });
                    }

                    this.tileList.ItemsSource = tileItems;
                    visited = true;

                }
                else
                    MessageBox.Show("Categories Not Loaded");
            }
            catch(Exception e)
            {
                MessageBox.Show(AirPowerConstants.ERROR_MESSAGE);
            }
          


        }

        public class Category
        {
            public int id { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public string name { get; set; }
            public string item_count { get; set; }
            public string thumbnail_url { get; set; }
        }

        public class RootObject
        {
            public int response_code { get; set; }
            public List<Category> categories { get; set; }
        }
  
        
        public  class TileItem
        {
            public string ImageUri
            {
                get;
                set;
            }

            public string Title
            {
                get;
                set;
            }

            public  int CategoryId
            {
                get;
                set;

            }

            //public string Notification
            //{
            //    get;
            //    set;
            //}

            //public bool DisplayNotification
            //{
            //    get
            //    {
            //        return !string.IsNullOrEmpty(this.Notification);
            //    }
            //}

            //public string Message
            //{
            //    get;
            //    set;
            //}

            public string GroupTag
            {
                get;
                set;
            }
        }


        public  async void  getLocation()
        {
           

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                // Request the current position
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                string lat = geoposition.Coordinate.Latitude.ToString("0.00");
                string lon = geoposition.Coordinate.Longitude.ToString("0.00");
              
              //  WaitIndicator.IsVisible = true;
                //var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                //request server url
                String url = "https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyDSd-z0-63E8cS6gy2pX2lnq3CegS8vEZA&latlng=" + lat + "," + lon + "&sensor=true";
               // MessageBox.Show(url);


                // MessageBox.Show(""+parameters.Capacity);

                String response = await httpHelperGetWithToken(url);
                // MessageBox.Show("response " + response);


                //when timer elapses
               // timer.Tick += delegate
                //{
                   // if (response.Length > 0)
                    //{
                       // timer.Stop();
                       // WaitIndicator.IsVisible = false;
                       location = parseLocationJSONResponse(response);
                       
                       country = location;
                      // MessageBox.Show("location  in " + country);
                  //  }
                  //  else
                 //   {
                        //timer.Stop();
                        //WaitIndicator.IsVisible = false;

                        //NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                   // }
               // };
               // timer.Start();

            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                   MessageBox.Show("location  is disabled in phone settings.");
                }
                //else
                {
                    // something else happened acquring the location
                }
            }


            //return location;
        }


        private string parseLocationJSONResponse(String response)
        {

            String located = String.Empty;
            try
            {
                ReturnObject returnObject = SimpleJson.DeserializeObject<ReturnObject>(response);

                Result res = returnObject.results[0];

                //AddressComponent a = res.address_components[5];

                String form = res.formatted_address;
                

              //  MessageBox.Show(a.short_name);
               // MessageBox.Show(form.Substring(form.LastIndexOf(',')+2).Trim());
                located = form.Substring(form.LastIndexOf(',') + 1).Trim();
            }
            catch(Exception e)
            {
                located = "Ghana";
            }
           
           
            return located;
        }

        public  async Task<string> httpHelperGetWithToken(string url)
        {
            var responseString = string.Empty;
            try
            {
                var httpClient = new HttpClient(new HttpClientHandler());




                HttpResponseMessage response = await httpClient.GetAsync(url);


                response.EnsureSuccessStatusCode();

                responseString = await response.Content.ReadAsStringAsync();

                //return await  Task.Run(()=>responseString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                responseString = e.Message;
            }

            return responseString;
        }

        private void tileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TileItem item = (TileItem)tileList.SelectedItem;

            if (item != null)
            {
                category = item.CategoryId;
                MediaPage.visited = false;
                MessageBox.Show("selected " + item.CategoryId);
            }

            refreshTileList();
            NavigationService.Navigate(new Uri("/MediaPage.xaml", UriKind.Relative));
        }


        private void AppBarlibrary_Click(object sender, EventArgs e)
        {
            LibraryPage.visited = false;
            NavigationService.Navigate(new Uri("/LibraryPage.xaml", UriKind.Relative));
        }

       
        public  void refreshTileList()
        {
            tileList.SelectedItem = null;
        }

        private void AppBarShare_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareTask = new ShareStatusTask();
            shareTask.Status = "Hello,I listen to Kakra Baiden on-the-go with his AirPower app and I thought you'd like it\n\n"
            + "Install now! http://getonairpower.com ";
            shareTask.Show();
        }

        private void menuPartner_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PartnerPage.xaml", UriKind.Relative));
        }


        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Bounds
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Northeast2
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest2
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Viewport
        {
            public Northeast2 northeast { get; set; }
            public Southwest2 southwest { get; set; }
        }

        public class Geometry
        {
            public Bounds bounds { get; set; }
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }

        public class Result
        {
            public List<AddressComponent> address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public List<string> types { get; set; }
        }

        public class ReturnObject
        {
            public List<Result> results { get; set; }
            public string status { get; set; }
        }

        private void AppBarSearch_Click(object sender, EventArgs e)
        {
             NavigationService.Navigate(new Uri("/SearchPanoramaPage.xaml", UriKind.Relative));
        }
        


    }
}