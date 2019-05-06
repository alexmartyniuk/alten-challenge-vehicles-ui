using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using VehiclesUI.Models;

namespace VehiclesUI.Services
{
    public class VehiclesService
    {
        private HttpClient _httpClient;
        public VehiclesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }        

        public async Task<Vehicle[]> GetVehiclesAsync()
        {
            return await _httpClient.GetJsonAsync<Vehicle[]>("https://altenvehiclesapi.azurewebsites.net/api/vehicles");
        }

        public async Task<Customer[]> GetCustomersAsync()
        {
            return await _httpClient.GetJsonAsync<Customer[]>("https://altenvehiclesapi.azurewebsites.net/api/customers");
        }
    }
}