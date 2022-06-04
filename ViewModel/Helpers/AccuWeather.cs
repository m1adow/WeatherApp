using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.ViewModel.Helpers
{
    public class AccuWeather
    {
        public const string BASEURL = "http://dataservice.accuweather.com/";
        public const string AUTOCOMPLETEENDPOINT = "locations/v1/cities/autocomplete?apikey={0}&q={1}";
        public const string CURRENTCONDITIONENDPOINT = "currentconditions/v1/{0}?apikey={1}";
        public const string APIKEY = "kK9mUsvk1o3axUnynLoKkBmPNDMqnl1j";

        public async Task<List<City>?> GetCities(string query)
        {
            List<City>? cities = new();

            string url = BASEURL + string.Format(AUTOCOMPLETEENDPOINT, APIKEY, query);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                cities = JsonConvert.DeserializeObject<List<City>>(json);
            }

            return cities;
        }

        public async Task<CurrentConditions?> GetCurrentConditions(string cityKey)
        {
            CurrentConditions? currentConditions = new();

            string url = BASEURL + string.Format(CURRENTCONDITIONENDPOINT, cityKey, APIKEY);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                currentConditions = JsonConvert.DeserializeObject<List<CurrentConditions>>(json)?.FirstOrDefault();
            }

            return currentConditions;
        }
    }
}
