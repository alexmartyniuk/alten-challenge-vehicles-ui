using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
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

        public async Task<Vehicle[]> GetVehiclesAsync(int? customerId, bool? connected)
        {
            var queryParams = new List<string>();            

            if (connected.HasValue)
            {
                queryParams.Add($"connected={connected.Value}");   
            }
            if (customerId.HasValue)
            {
               queryParams.Add($"customerId={customerId.Value}");     
            } 

            var serviceUrl = "https://altenvehiclesapi.azurewebsites.net/api/vehicles?" + string.Join("&", queryParams);    

            return await _httpClient.GetJsonAsync<Vehicle[]>(serviceUrl);
        }

        public async Task<Customer[]> GetCustomersAsync()
        {
            return await _httpClient.GetJsonAsync<Customer[]>("https://altenvehiclesapi.azurewebsites.net/api/customers");
        }
    }
}