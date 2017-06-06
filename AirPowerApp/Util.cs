using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirPowerApp
{
    class Util
    {
       

        public static async Task<string> httpHelperPost(string url, List<KeyValuePair<string, string>> parameters)
        {

            var httpClient = new HttpClient(new HttpClientHandler());
            httpClient.DefaultRequestHeaders.Add("cookie", "access_token=" + CategoryPage.token);
            var responseString = string.Empty;
            try
            {
               
               
                HttpResponseMessage response = await httpClient.PostAsync(url, new FormUrlEncodedContent(parameters));

                response.EnsureSuccessStatusCode();

                 responseString = await response.Content.ReadAsStringAsync();

                //return await  Task.Run(()=>responseString);
            }
            catch(Exception e)
            {
              Console.WriteLine(e.Message);
            }
           

            return responseString;
        }


        public static async Task<string> httpHelperGetWithToken(string url)
        {
            var responseString = string.Empty;
            try
            {
                var httpClient = new HttpClient(new HttpClientHandler());
                httpClient.DefaultRequestHeaders.Add("cookie", "access_token=" +  IsolatedStorageSettings.ApplicationSettings["access_token"] as String);



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


        public static void wait(int time)
        {
           
        }
    }
}
