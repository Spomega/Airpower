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
using System.IO;
using Microsoft.Phone.Tasks;

namespace AirPowerApp
{
    public partial class LibraryPage : PhoneApplicationPage
    {
        public static bool visited = false;
        public static List<Item> audio = new List<Item>();
        public static List<Item> video = new List<Item>();
        public static List<Item> pdf = new List<Item>();
        public static string url = String.Empty;
        WebClient _webClient; // Used for downloading 
        private bool _playSoundAfterDownload;
       
        public LibraryPage()
        {
            InitializeComponent();
            _webClient = new WebClient();
            _webClient.OpenReadCompleted += (s1, e1) =>
            {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                //when timer elapses
                timer.Tick += delegate
                {
                        timer.Stop();
                        WaitIndicator.IsVisible = false;
                };
                timer.Start();
                if (e1.Error == null)
                {
                    try
                    {
                        string fileName = url.Substring(url.LastIndexOf("/") + 1).Trim();
                        bool isSpaceAvailable = IsSpaceIsAvailable(e1.Result.Length);

                        if (isSpaceAvailable)
                        {
                            // Save mp3 to Isolated Storage
                            using (var isfs = new IsolatedStorageFileStream(fileName,
                                                FileMode.CreateNew,
                                                IsolatedStorageFile.GetUserStoreForApplication()))
                            {
                                long fileLen = e1.Result.Length;
                                byte[] b = new byte[fileLen];
                                e1.Result.Read(b, 0, b.Length);
                                isfs.Write(b, 0, b.Length);
                                isfs.Flush();
                            }

                            if (_playSoundAfterDownload)
                            {
                                _playSoundAfterDownload = false;
                                PlaySound(fileName);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not enough to save space available to download media.");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show(e1.Error.Message);
                }
            };

           

        }


        // Check to make sure there are enough space available on the phone
        // in order to save the image that we are downloading on to the phone
        private bool IsSpaceIsAvailable(long spaceReq)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {

                long spaceAvail = store.AvailableFreeSpace;
                if (spaceReq > spaceAvail)
                {
                    return false;
                }
                return true;
            }
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CategoryPage.visited = true;
            MediaPage.visited = true;
            if (!visited)
            {

                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AirPowerConstants.server_url + AirPowerConstants.GET_LIBRARY + "?email=" + CategoryPage.user;


                
                String response = await Util.httpHelperGetWithToken(url);
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

            List<ItemLibrary> library = new List<ItemLibrary>();
            
           

            foreach (ItemLibrary libItem in returnObject.item_library)
            {


                if (libItem.item!=null)
                {
                    if (libItem.item.type.Equals("audio"))
                    {
                        audio.Add(new Item()
                        {
                            thumbnail_url = libItem.item.thumbnail_url,
                            description = libItem.item.description,
                            download_url = libItem.item.download_url

                        });
                    }
                    if (libItem.item.type.Equals("video"))
                    {
                        video.Add(new Item()
                        {
                            thumbnail_url = libItem.item.thumbnail_url,
                            description = libItem.item.description,
                            download_url = libItem.item.download_url

                        });
                    }

                    if (libItem.item.type.Equals("pdf"))
                    {
                        pdf.Add(new Item()
                        {
                            thumbnail_url = libItem.item.thumbnail_url,
                            description = libItem.item.description,
                            download_url = libItem.item.download_url

                        });
                    }
                }
              

            }


            lstBoxAudio.ItemsSource = audio;
            lstBoxVideo.ItemsSource = video;
            lstBoxPDF.ItemsSource = pdf;
            // MessageBox.Show("Categories " +categories.Count);
            //visited = true;
            audio = new List<Item>();
            video = new List<Item>();
            pdf = new List<Item>();
            //audio.Clear();
            //video.Clear();
            //pdf.Clear();
           
        }


        private void PlaySound(string url)
        {
            string fileName = url.Substring(url.LastIndexOf("/") + 1).Trim();

            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(fileName))
                {
                    using (var isoStream = isf.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                    {
                        //mediaSound.Stop();
                        //mediaSound.SetSource(isoStream);

                        //mediaSound.Position = System.TimeSpan.FromSeconds(0);
                        //mediaSound.Volume = 20;
                        //mediaSound.Play();
                        MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();

                        mediaPlayerLauncher.Media = new Uri(fileName, UriKind.RelativeOrAbsolute);
                        mediaPlayerLauncher.Location = MediaLocationType.Data;
                        mediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop;
                        mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;
                        visited = true;
                        mediaPlayerLauncher.Show();
                    }
                }
                else
                {
                    _playSoundAfterDownload = true;
                    WaitIndicator.IsVisible = true;
                    WaitIndicator.Text = "Downloading New Sermons....Please Wait";
                    _webClient.OpenReadAsync(new Uri(url));


                }
            }

        }


        public class Item
        {
            public string id { get; set; }
            public string label { get; set; }
            public string description { get; set; }
            public string author { get; set; }
            public string price { get; set; }
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

        public class ItemLibrary
        {
            public Item item { get; set; }
            public string status { get; set; }
            public string created_at { get; set; }
        }

        public class RootObject
        {
            public int response_code { get; set; }
            public List<ItemLibrary> item_library { get; set; }
        }


        private async void lstBoxVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecteditem = (Item)lstBoxVideo.SelectedItem;
            //MessageBox.Show(selecteditem.download_url);
            url = selecteditem.download_url;

              string fileName = url.Substring(url.LastIndexOf("/") + 1).Trim();

              using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
              {
                  if (isf.FileExists(fileName))
                  {
                      PlaySound(url);
                  }

                  else
                  {
                      try
                      {
                          CustomMessageBox messageBox = new CustomMessageBox()
                          {
                              Caption = "Sermon Has Been Purchased",
                              Message = "Do You Want To ",
                              LeftButtonContent = "View Online",
                              RightButtonContent = "Download",

                          };
                          switch (await messageBox.ShowAsync())
                          {
                              case CustomMessageBoxResult.LeftButton:
                                  MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();

                                  mediaPlayerLauncher.Media = new Uri(url, UriKind.RelativeOrAbsolute);
                                  mediaPlayerLauncher.Location = MediaLocationType.Data;
                                  mediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop;
                                  mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;
                                  visited = true;
                                  mediaPlayerLauncher.Show();
                                  //         mePlayAudio.Source =new Uri(url,UriKind.Absolute);
                                  ////mePlayAudio.Source = new Uri("/audio/supernatural_guidance.mp3", UriKind.Relative);
                                  //    mePlayAudio.Play();
                                  break;
                              case CustomMessageBoxResult.RightButton:
                                  PlaySound(url);
                                  break;
                              case CustomMessageBoxResult.None:
                                  // Do something.
                                  PlaySound(url);
                                  break;
                              default:
                                  PlaySound(url);
                                  break;
                          }

                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show("Sorry could not play audio at the moment,check your internet connection");
                      }
                      // refreshVideoList();
                  }
              }
        }

        private void lstBoxPDF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshPdfList();
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

        private async  void lstBoxAudio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecteditem =(Item)lstBoxAudio.SelectedItem;
            url = selecteditem.download_url;
          //  MessageBox.Show(url);

            string fileName = url.Substring(url.LastIndexOf("/") + 1).Trim();

            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(fileName))
                {
                    PlaySound(url);
                }
                else
                {
                    try
                    {
                        if (selecteditem != null)
                        {
                            CustomMessageBox messageBox = new CustomMessageBox()
                            {
                                Caption = "Sermon Has Been Purchased",
                                Message = "Do You Want To ",
                                LeftButtonContent = "Listen Online",
                                RightButtonContent = "Download",

                            };

                            switch (await messageBox.ShowAsync())
                            {
                                case CustomMessageBoxResult.LeftButton:
                                    MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();

                                    mediaPlayerLauncher.Media = new Uri(url, UriKind.RelativeOrAbsolute);
                                    mediaPlayerLauncher.Location = MediaLocationType.Data;
                                    mediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop;
                                    mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;
                                    visited = true;
                                    mediaPlayerLauncher.Show();
                                    //         mePlayAudio.Source =new Uri(url,UriKind.Absolute);
                                    ////mePlayAudio.Source = new Uri("/audio/supernatural_guidance.mp3", UriKind.Relative);
                                    //    mePlayAudio.Play();
                                    break;
                                case CustomMessageBoxResult.RightButton:
                                    PlaySound(url);
                                    // refreshAudioList();
                                    break;
                                case CustomMessageBoxResult.None:
                                    // Do something.
                                    PlaySound(url);
                                    break;
                                default:
                                    PlaySound(url);
                                    break;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Sorry could not play audio at the moment,check your internet connection");
                    }
                }
            }


        }
    }
}