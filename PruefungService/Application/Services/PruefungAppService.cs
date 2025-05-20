using PruefungService.Application.DTOs;
using PruefungService.Application.Exceptions;
using PruefungService.Application.Interfaces;
using PruefungService.Domain.Entities;
using PruefungService.Domain.Interfaces;
using PruefungService.Domain.Services;
using PruefungService.Domain.ValueObjects;

namespace PruefungService.Application.Services
{
    public class PruefungAppService : IPruefungAppService
    {
        private readonly IPruefungRepository _pruefungRepository;
        private readonly IAufgabenService _aufgabenService;
        private readonly PruefungValidierungsService _validierungsService;

        public PruefungAppService(
            IPruefungRepository pruefungRepository,
            IAufgabenService aufgabenService,
            PruefungValidierungsService validierungsService)
        {
            _pruefungRepository = pruefungRepository;
            _aufgabenService = aufgabenService;
            _validierungsService = validierungsService;
        }

        public async Task<IEnumerable<PruefungDto>> GetAllePruefungenAsync()
        {
            var pruefungen = await _pruefungRepository.GetAllePruefungenAsync();
            return pruefungen.Select(MapToPruefungDto);
        }

        public async Task<PruefungDto?> GetPruefungByIdAsync(int id)
        {
            var pruefung = await _pruefungRepository.GetPruefungByIdAsync(id);
            return pruefung != null ? MapToPruefungDto(pruefung) : null;
        }

        public async Task<IEnumerable<AufgabeDto>> GetAufgabenFuerPruefungAsync(int pruefungId)
        {
            var pruefung = await _pruefungRepository.GetPruefungByIdAsync(pruefungId);
            if (pruefung == null)
                throw new NotFoundException("Pruefung", pruefungId);

            var aufgaben = await _aufgabenService.GetAufgabenByIdsAsync(pruefung.AufgabenIds);
            return aufgaben.Select(MapToAufgabeDto);
        }

        public async Task<IEnumerable<AufgabeDto>> GetAlleAufgabenAsync()
        {
            var aufgaben = await _aufgabenService.GetAufgabenAsync();
            return aufgaben.Select(MapToAufgabeDto);
        }

        public async Task<PruefungDto> ErstellePruefungAsync(PruefungErstellenDto pruefungDto)
        {
            // Domain-Entität erstellen
            var pruefung = new Pruefung(
                pruefungDto.Titel,
                pruefungDto.Datum,
                pruefungDto.Zeitlimit
            );
            
            // Aufgaben zuweisen, wenn vorhanden
            if (pruefungDto.AufgabenIds != null && pruefungDto.AufgabenIds.Any())
            {
                pruefung.SetzeAufgaben(pruefungDto.AufgabenIds);
            }
            
            // Validieren
            if (!_validierungsService.IstPruefungGueltig(pruefung))
                throw new ValidationException("Die Prüfung ist nicht gültig.");
            
            // Persistieren
            var erstelltePruefung = await _pruefungRepository.ErstellePruefungAsync(pruefung);
            
            return MapToPruefungDto(erstelltePruefung);
        }

        public async Task<PruefungDto?> AktualisierePruefungAsync(int id, PruefungAktualisierenDto pruefungDto)
        {
            // Bestehende Prüfung abrufen
            var bestehendePruefung = await _pruefungRepository.GetPruefungByIdAsync(id);
            if (bestehendePruefung == null)
                return null;
            
            // Prüfung aktualisieren
            if (!string.IsNullOrWhiteSpace(pruefungDto.Titel))
            {
                bestehendePruefung.AendereTitel(pruefungDto.Titel);
            }
            
            if (pruefungDto.Datum != default)
            {
                bestehendePruefung.AendereDatum(pruefungDto.Datum);
            }
            
            if (pruefungDto.Zeitlimit > 0)
            {
                bestehendePruefung.AendereZeitlimit(pruefungDto.Zeitlimit);
            }
            
            // Validieren
            if (!_validierungsService.IstPruefungGueltig(bestehendePruefung))
                throw new ValidationException("Die aktualisierte Prüfung ist nicht gültig.");
            
            // Persistieren
            var aktualisierte = await _pruefungRepository.AktualisierePruefungAsync(bestehendePruefung);
            
            return aktualisierte != null ? MapToPruefungDto(aktualisierte) : null;
        }

        public async Task<PruefungDto?> WeiseAufgabenZuAsync(int id, AufgabenZuweisenDto aufgabenDto)
        {
            // Bestehende Prüfung abrufen
            var bestehendePruefung = await _pruefungRepository.GetPruefungByIdAsync(id);
            if (bestehendePruefung == null)
                return null;
            
            // Aufgaben zuweisen
            bestehendePruefung.SetzeAufgaben(aufgabenDto.AufgabenIds);
            
            // Validieren
            if (!_validierungsService.IstPruefungGueltig(bestehendePruefung))
                throw new ValidationException("Die Prüfung ist nach der Zuweisung der Aufgaben nicht gültig.");
            
            // Persistieren
            var aktualisierte = await _pruefungRepository.AktualisierePruefungAsync(bestehendePruefung);
            
            return aktualisierte != null ? MapToPruefungDto(aktualisierte) : null;
        }

        public async Task<bool> LoeschePruefungAsync(int id)
        {
            return await _pruefungRepository.LoeschePruefungAsync(id);
        }

        // Mapping-Methoden
        private PruefungDto MapToPruefungDto(Pruefung pruefung)
        {
            return new PruefungDto
            {
                Id = pruefung.Id,
                Titel = pruefung.Titel,
                AufgabenIds = pruefung.AufgabenIds.ToList(),
                Datum = pruefung.Datum,
                Zeitlimit = pruefung.Zeitlimit
            };
        }

        private AufgabeDto MapToAufgabeDto(Aufgabe aufgabe)
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