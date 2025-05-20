using System.Net.Http.Json;
using AufgabenService.Client.Models;
using AufgabenService.Client.Services.Interfaces;

namespace AufgabenService.Client.Services.Implementations
{
    public class AufgabenDataService : IAufgabenDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "api/aufgaben";

        public AufgabenDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AufgabeViewModel>> GetAlleAufgabenAsync()
        {
            try
            {
                var aufgaben = await _httpClient.GetFromJsonAsync<List<AufgabeViewModel>>(_baseUrl);
                return aufgaben ?? new List<AufgabeViewModel>();
            }
            catch (Exception)
            {
                // In einer realen Anwendung würde hier Logging erfolgen
                return new List<AufgabeViewModel>();
            }
        }

        public async Task<AufgabeViewModel?> GetAufgabeByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<AufgabeViewModel>($"{_baseUrl}/{id}");
            }
            catch (Exception)
            {
                // In einer realen Anwendung würde hier Logging erfolgen
                return null;
            }
        }

        public async Task<AufgabeViewModel> ErstelleAufgabeAsync(AufgabeErstellenModel aufgabe)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, aufgabe);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<AufgabeViewModel>() 
                ?? throw new InvalidOperationException("Die Antwort konnte nicht deserialisiert werden.");
        }

        public async Task<AufgabeViewModel?> AktualisiereAufgabeAsync(int id, AufgabeErstellenModel aufgabe)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", aufgabe);
            
            if (!response.IsSuccessStatusCode)
                return null;
            
            return await response.Content.ReadFromJsonAsync<AufgabeViewModel>();
        }

        public async Task<bool> LoescheAufgabeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}