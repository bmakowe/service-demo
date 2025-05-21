using System.Net.Http.Json;
using PruefungService.Client.Models;
using PruefungService.Client.Services.Interfaces;

namespace PruefungService.Client.Services.Implementations
{
    public class PruefungDataService : IPruefungDataService
    {
        private readonly HttpClient _httpClient;

        public PruefungDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PruefungViewModel>> GetAllePruefungenAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PruefungViewModel>>("api/pruefung") ?? new List<PruefungViewModel>();
        }

        public async Task<PruefungViewModel?> GetPruefungByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PruefungViewModel>($"api/pruefung/{id}");
        }

        public async Task<List<AufgabeViewModel>> GetAlleAufgabenAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AufgabeViewModel>>("api/aufgaben") ?? new List<AufgabeViewModel>();
        }

        public async Task<List<AufgabeViewModel>> GetAufgabenFuerPruefungAsync(int pruefungId)
        {
            return await _httpClient.GetFromJsonAsync<List<AufgabeViewModel>>($"api/pruefung/{pruefungId}/aufgaben") ?? new List<AufgabeViewModel>();
        }

        public async Task<PruefungViewModel?> CreatePruefungAsync(PruefungErstellenModel pruefungDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pruefung", pruefungDto);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PruefungViewModel>();
            }
            
            return null;
        }

        public async Task<PruefungViewModel?> UpdatePruefungAufgabenAsync(int pruefungId, AufgabenZuweisenModel aufgabenDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/pruefung/{pruefungId}/aufgaben", aufgabenDto);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PruefungViewModel>();
            }
            
            return null;
        }

        public async Task<bool> DeletePruefungAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/pruefung/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}