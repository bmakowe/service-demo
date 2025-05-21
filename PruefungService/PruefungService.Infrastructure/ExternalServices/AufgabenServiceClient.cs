using System.Text.Json;
using PruefungService.Domain.Interfaces;
using PruefungService.Domain.ValueObjects;

namespace PruefungService.Infrastructure.ExternalServices
{
    public class AufgabenServiceClient : IAufgabenService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AufgabenServiceClient> _logger;

        public AufgabenServiceClient(HttpClient httpClient, ILogger<AufgabenServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Aufgabe>> GetAufgabenAsync()
        {
            try
            {
                _logger.LogInformation("Sende Anfrage an: {BaseAddress}api/aufgaben", _httpClient.BaseAddress);
                
                var response = await _httpClient.GetFromJsonAsync<List<AufgabeDto>>("api/aufgaben");
                if (response == null)
                {
                    _logger.LogWarning("Keine Aufgaben vom AufgabenService empfangen");
                    return Enumerable.Empty<Aufgabe>();
                }
                
                _logger.LogInformation("Erfolgreich {Count} Aufgaben empfangen", response.Count);
                return response.Select(MapToEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen der Aufgaben: {Message}", ex.Message);
                return Enumerable.Empty<Aufgabe>();
            }
        }

        public async Task<Aufgabe?> GetAufgabeByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<AufgabeDto>($"api/aufgaben/{id}");
                return response != null ? MapToEntity(response) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen der Aufgabe {Id}: {Message}", id, ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Aufgabe>> GetAufgabenByIdsAsync(IEnumerable<int> ids)
        {
            var aufgaben = new List<Aufgabe>();
            
            foreach (var id in ids)
            {
                var aufgabe = await GetAufgabeByIdAsync(id);
                if (aufgabe != null)
                {
                    aufgaben.Add(aufgabe);
                }
            }
            
            return aufgaben;
        }

        private Aufgabe MapToEntity(AufgabeDto dto)
        {
            return new Aufgabe(
                dto.Id,
                dto.Frage,
                dto.Antworten.Select(a => new Antwort(a.Id, a.Text, a.IstRichtig))
            );
        }
    }

    // DTO für die Deserialisierung der API-Antworten
    internal class AufgabeDto
    {
        public int Id { get; set; }
        public string Frage { get; set; } = string.Empty;
        public List<AntwortDto> Antworten { get; set; } = new();
    }

    internal class AntwortDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IstRichtig { get; set; }
    }
}