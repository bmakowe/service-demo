using System.Net.Http.Json;
using AufgabenService.Client.Models;
using AufgabenService.Client.Services.Interfaces;

namespace AufgabenService.Client.Services.Implementations
{
    public class AufgabenDataService : IAufgabenDataService
    {
        private readonly HttpClient _httpClient;

        public AufgabenDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AufgabenViewModel>> GetAlleAufgabenAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AufgabenViewModel>>("api/aufgaben") ?? new List<AufgabenViewModel>();
        }

        public async Task<AufgabenViewModel?> GetAufgabeByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AufgabenViewModel>($"api/aufgaben/{id}");
        }

        public async Task<AufgabenViewModel?> CreateAufgabeAsync(AufgabeErstellenModel aufgabeDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/aufgaben", aufgabeDto);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AufgabenViewModel>();
            }
            
            return null;
        }

        public async Task<AufgabenViewModel?> UpdateAufgabeAsync(int id, AufgabeErstellenModel aufgabeDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/aufgaben/{id}", aufgabeDto);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AufgabenViewModel>();
            }
            
            return null;
        }

        public async Task<bool> DeleteAufgabeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/aufgaben/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}