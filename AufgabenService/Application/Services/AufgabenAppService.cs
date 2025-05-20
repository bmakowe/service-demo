using AufgabenService.Application.DTOs;
using AufgabenService.Application.Interfaces;
using AufgabenService.Domain.Entities;
using AufgabenService.Domain.Interfaces;
using AufgabenService.Domain.Services;

namespace AufgabenService.Application.Services
{
    public class AufgabenAppService : IAufgabenService
    {
        private readonly IAufgabenRepository _aufgabenRepository;
        private readonly AufgabenValidierungsService _validierungsService;

        public AufgabenAppService(
            IAufgabenRepository aufgabenRepository,
            AufgabenValidierungsService validierungsService)
        {
            _aufgabenRepository = aufgabenRepository;
            _validierungsService = validierungsService;
        }

        public async Task<IEnumerable<AufgabeDto>> GetAlleAufgabenAsync()
        {
            var aufgaben = await _aufgabenRepository.GetAlleAufgabenAsync();
            return aufgaben.Select(MapToDto);
        }

        public async Task<AufgabeDto?> GetAufgabeByIdAsync(int id)
        {
            var aufgabe = await _aufgabenRepository.GetAufgabeByIdAsync(id);
            return aufgabe != null ? MapToDto(aufgabe) : null;
        }

        public async Task<AufgabeDto> ErstelleAufgabeAsync(AufgabeErstellenDto aufgabeDto)
        {
            // Domain-Entität erstellen
            var aufgabe = new Aufgabe(aufgabeDto.Frage);
            
            // Antworten hinzufügen
            foreach (var antwortDto in aufgabeDto.Antworten)
            {
                aufgabe.FuegeAntwortHinzu(antwortDto.Text, antwortDto.IstRichtig);
            }
            
            // Validieren
            if (!_validierungsService.IstAufgabeGueltig(aufgabe))
                throw new InvalidOperationException("Die Aufgabe ist nicht gültig.");
            
            // Persistieren
            var erstellteAufgabe = await _aufgabenRepository.ErstelleAufgabeAsync(aufgabe);
            
            return MapToDto(erstellteAufgabe);
        }

        public async Task<AufgabeDto?> AktualisiereAufgabeAsync(int id, AufgabeAktualisierenDto aufgabeDto)
        {
            // Bestehende Aufgabe abrufen
            var bestehendeAufgabe = await _aufgabenRepository.GetAufgabeByIdAsync(id);
            if (bestehendeAufgabe == null)
                return null;
            
            // Aufgabe aktualisieren
            var aktualisierteAufgabe = Aufgabe.Erstellen(
                id,
                aufgabeDto.Frage,
                aufgabeDto.Antworten.Select(a => (a.Text, a.IstRichtig)).ToList()
            );
            
            // Validieren
            if (!_validierungsService.IstAufgabeGueltig(aktualisierteAufgabe))
                throw new InvalidOperationException("Die aktualisierte Aufgabe ist nicht gültig.");
            
            // Persistieren
            var gespeicherteAufgabe = await _aufgabenRepository.AktualisiereAufgabeAsync(aktualisierteAufgabe);
            
            return gespeicherteAufgabe != null ? MapToDto(gespeicherteAufgabe) : null;
        }

        public async Task<bool> LoescheAufgabeAsync(int id)
        {
            return await _aufgabenRepository.LoescheAufgabeAsync(id);
        }

        // Mapping-Methoden
        private AufgabeDto MapToDto(Aufgabe aufgabe)
        {
            return new AufgabeDto
            {
                Id = aufgabe.Id,
                Frage = aufgabe.Frage,
                Antworten = aufgabe.Antworten.Select(a => new AntwortDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    IstRichtig = a.IstRichtig
                }).ToList()
            };
        }
    }
}