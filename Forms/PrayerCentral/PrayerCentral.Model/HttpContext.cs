using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrayerCentral.Model
{
    public class HttpContext
    {
        private readonly HttpClient _HttpClient;

        public HttpContext()
        {
            _HttpClient = new HttpClient { BaseAddress = new Uri("http://10.0.1.30/GG.PrayerCentral/api/"), Timeout = TimeSpan.FromSeconds(10) };
        }

        public async Task<string> GetAsync(string controller)
        {
            string url = $"{controller}/checklogin";
            string responseJson;
            //T retItem = default(T);

            try
            {
                responseJson = await _HttpClient.GetStringAsync(url);
            }
            catch (Exception e)
            {
                responseJson = "";
            }

            //retItem = JsonConvert.DeserializeObject<T>(responseJson);

            return responseJson;
        }
    }
}
