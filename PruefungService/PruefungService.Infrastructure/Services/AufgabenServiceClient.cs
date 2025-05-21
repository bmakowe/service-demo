using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PruefungService.Application.DTOs;
using PruefungService.Application.Interfaces;

namespace PruefungService.Infrastructure.Services
{
    public class AufgabenServiceClient : IAufgabenServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AufgabenServiceClient> _logger;

        public AufgabenServiceClient(HttpClient httpClient, ILogger<AufgabenServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<AufgabeDto>> GetAllAufgabenAsync()
        {
            try
            {
                _logger.LogInformation($"Sending request to {_httpClient.BaseAddress}api/aufgaben");
                
                var aufgaben = await _httpClient.GetFromJsonAsync<List<AufgabeDto>>("api/aufgaben");
                
                _logger.LogInformation($"Successfully received {aufgaben?.Count} tasks");
                
                return aufgaben ?? Enumerable.Empty<AufgabeDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks from AufgabenService");
                return Enumerable.Empty<AufgabeDto>();
            }
        }

        public async Task<AufgabeDto> GetAufgabeByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Sending request to {_httpClient.BaseAddress}api/aufgaben/{id}");
                
                return await _httpClient.GetFromJsonAsync<AufgabeDto>($"api/aufgaben/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching task with id {id} from AufgabenService");
                return null;
            }
        }

        public async Task<IEnumerable<AufgabeDto>> GetAufgabenByIdsAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<AufgabeDto>();

            try
            {
                // Da die API keinen Endpunkt für Mehrfachabfrage hat, rufen wir alle Aufgaben ab
                // und filtern auf der Client-Seite
                var alleAufgaben = await GetAllAufgabenAsync();
                return alleAufgaben.Where(a => ids.Contains(a.Id)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching tasks with ids {string.Join(",", ids)} from AufgabenService");
                return Enumerable.Empty<AufgabeDto>();
            }
        }
    }
}