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
#if DEBUG
        private const string ServiceBaseUrl = "http://localhost:50701/api/";
#else
        private const string ServiceBaseUrl = "https://altenvehiclesapi.azurewebsites.net/api/";
#endif
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

            var serviceUrl = ServiceBaseUrl + "vehicles?" + string.Join("&", queryParams);    

            return await _httpClient.GetJsonAsync<Vehicle[]>(serviceUrl);
        }

        public async Task<Customer[]> GetCustomersAsync()
        {
            return await _httpClient.GetJsonAsync<Customer[]>(ServiceBaseUrl + "customers");
        }

        public async Task ConnectAsync(int vehicleId)
        {
            await _httpClient.PostAsync(ServiceBaseUrl + $"vehicles/{vehicleId}/connect", null);
        }
    }
}