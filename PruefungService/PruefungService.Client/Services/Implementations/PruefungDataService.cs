using System.Net.Http.Json;
using PruefungService.Client.Models;
using PruefungService.Client.Services.Interfaces;

namespace PruefungService.Client.Services.Implementations
{
    public class PruefungDataService : IPruefungDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "api/pruefung";

        public PruefungDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<PruefungViewModel>> GetAllePruefungenAsync()
        {
            try
            {
                var pruefungen = await _httpClient.GetFromJsonAsync<List<PruefungViewModel>>(_baseUrl);
                return pruefungen ?? new List<PruefungViewModel>();
            }
            catch (Exception)
            {
                // In einer realen Anwendung würde hier Logging erfolgen
                return new List<PruefungViewModel>();
            }
        }

        public async Task<PruefungViewModel?> GetPruefungByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PruefungViewModel>($"{_baseUrl}/{id}");
            }
            catch (Exception)
            {
                // In einer realen Anwendung würde hier Logging erfolgen
                return null;
            }
        }

        public async Task<IEnumerable<AufgabeViewModel>> GetAufgabenFuerPruefungAsync(int pruefungId)
        {
            try
            {
                var aufgaben = await _httpClient.GetFromJsonAsync<List<AufgabeViewModel>>($"{_baseUrl}/{pruefungId}/aufgaben");
                return aufgaben ?? new List<AufgabeViewModel>();
            }
            catch (Exception)
            {
                // In einer realen Anwendung würde hier Logging erfolgen
                return new List<AufgabeViewModel>();
            }
        }

        public async Task<IEnumerable<AufgabeViewModel>> GetAlleAufgabenAsync()
        {
            try
            {
                var aufgaben = await _httpClient.GetFromJsonAsync<List<AufgabeViewModel>>("api/aufgaben");
                return aufgaben ?? new List<AufgabeViewModel>();
            }
            catch (Exception)
            {
                // In einer realen Anwendung würde hier Logging erfolgen
                return new List<AufgabeViewModel>();
            }
        }

        public async Task<PruefungViewModel> ErstellePruefungAsync(PruefungErstellenModel pruefung)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, pruefung);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<PruefungViewModel>() 
                ?? throw new InvalidOperationException("Die Antwort konnte nicht deserialisiert werden.");
        }

        public async Task<PruefungViewModel?> AktualisierePruefungAsync(int id, PruefungErstellenModel pruefung)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", pruefung);
            
            if (!response.IsSuccessStatusCode)
                return null;
            
            return await response.Content.ReadFromJsonAsync<PruefungViewModel>();
        }

        public async Task<PruefungViewModel?> WeiseAufgabenZuAsync(int id, AufgabenZuweisenModel aufgaben)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}/aufgaben", aufgaben);
            
            if (!response.IsSuccessStatusCode)
                return null;
            
            return await response.Content.ReadFromJsonAsync<PruefungViewModel>();
        }

        public async Task<bool> LoeschePruefungAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}