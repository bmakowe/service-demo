using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<AufgabeViewModel>> GetAllAufgabenAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AufgabeViewModel>>("api/aufgaben");
                return response ?? new List<AufgabeViewModel>();
            }
            catch (Exception)
            {
                // In einer produktiven Anwendung würde man hier entsprechend loggen
                // und möglicherweise eine benutzerdefinierte Exception zurückgeben
                return new List<AufgabeViewModel>();
            }
        }

        public async Task<AufgabeViewModel> GetAufgabeByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<AufgabeViewModel>($"api/aufgaben/{id}");
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return null;
            }
        }

        public async Task<AufgabeViewModel> CreateAufgabeAsync(AufgabeErstellenModel aufgabe)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/aufgaben", aufgabe);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<AufgabeViewModel>();
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return null;
            }
        }

        public async Task<AufgabeViewModel> UpdateAufgabeAsync(int id, AufgabeErstellenModel aufgabe)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/aufgaben/{id}", aufgabe);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<AufgabeViewModel>();
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return null;
            }
        }

        public async Task<bool> DeleteAufgabeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/aufgaben/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return false;
            }
        }
    }
}