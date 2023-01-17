using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.ViewModel.Helpers
{
    public class AccuWeatherHelper //Responsible for get requests of cities and weather conditions
    {
        public const string BASE_URL = "http://dataservice.accuweather.com";
        public const string AUTOCOMPLETE_ENDPOINT = "/locations/v1/cities/autocomplete?apikey={0}&q={1}"; //endpoint to get the cities
        public const string CURRENT_CONDITIONS_ENDPOINT = "/currentconditions/v1/{0}?apikey={1}"; //endpoint to get weather conditions
        public const string API_KEY = "G7YfEKyBOqGqvfnvGSvRsNSLZVB9EJDj";

        /// <summary>
        /// Get async request that returns a list of cities
        /// </summary>
        /// <param name="query">Name of the city in the textbox</param>
        /// <returns>List of cities from the API</returns>
        public static async Task<List<City>> GetCities(string query)
        {
            List<City> cities = new List<City>();

            //using string.Format to fill the placeholders in the string
            string url = BASE_URL + string.Format(AUTOCOMPLETE_ENDPOINT, API_KEY, query);

            //creating a http client connection
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url); //Get request passing the url
                string json = await response.Content.ReadAsStringAsync(); // reading the json from get request as string

                cities = JsonConvert.DeserializeObject<List<City>>(json);
            }

            return cities;

        }

        /// <summary>
        /// Get async method that returns the weather conditions of a specific city.
        /// </summary>
        /// <param name="cityKey">key from the api of the requested city</param>
        /// <returns></returns>
        public static async Task<CurrentConditions> GetCurrentConditions(string cityKey)
        {
            CurrentConditions currentConditions = new CurrentConditions();

            string url = BASE_URL + string.Format(CURRENT_CONDITIONS_ENDPOINT, cityKey, API_KEY);

            //creating a http client 
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                currentConditions = JsonConvert.DeserializeObject<List<CurrentConditions>>(json).FirstOrDefault();
            }

            return currentConditions;
        }
    }
}
