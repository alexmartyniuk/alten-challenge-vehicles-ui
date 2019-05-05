using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;

namespace VehiclesUI.Services
{
    public class ForecastService
    {
        private HttpClient _httpClient;
        public ForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }        

        public async Task<WeatherForecast[]> GetForecastAsync()
        {
            return await _httpClient.GetJsonAsync<WeatherForecast[]>("https://altenwebappapi.azurewebsites.net/api/values");
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }
    }
}