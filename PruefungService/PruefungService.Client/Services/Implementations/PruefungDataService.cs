using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<PruefungViewModel>> GetAllPruefungenAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<PruefungViewModel>>("api/pruefung");
                return response ?? new List<PruefungViewModel>();
            }
            catch (Exception)
            {
                // In einer produktiven Anwendung würde man hier entsprechend loggen
                // und möglicherweise eine benutzerdefinierte Exception zurückgeben
                return new List<PruefungViewModel>();
            }
        }

        public async Task<PruefungViewModel?> GetPruefungByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PruefungViewModel>($"api/pruefung/{id}");
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return null;
            }
        }

        public async Task<IEnumerable<AufgabeViewModel>> GetAufgabenForPruefungAsync(int pruefungId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AufgabeViewModel>>($"api/pruefung/{pruefungId}/aufgaben");
                return response ?? new List<AufgabeViewModel>();
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return new List<AufgabeViewModel>();
            }
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
                // Fehlerbehandlung
                return new List<AufgabeViewModel>();
            }
        }

        public async Task<PruefungViewModel?> CreatePruefungAsync(PruefungErstellenModel pruefung)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/pruefung", pruefung);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PruefungViewModel>();
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return null;
            }
        }

        public async Task<PruefungViewModel?> UpdatePruefungAsync(int id, PruefungAktualisierenModel pruefung)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/pruefung/{id}", pruefung);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PruefungViewModel>();
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return null;
            }
        }

        public async Task<PruefungViewModel?> UpdatePruefungAufgabenAsync(int id, AufgabenZuweisenModel aufgaben)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/pruefung/{id}/aufgaben", aufgaben);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PruefungViewModel>();
            }
            catch (Exception)
            {
                // Fehlerbehandlung
                return null;
            }
        }

        public async Task<bool> DeletePruefungAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/pruefung/{id}");
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