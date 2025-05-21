using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruefungService.Application.DTOs;
using PruefungService.Application.Exceptions;
using PruefungService.Application.Interfaces;
using PruefungService.Domain.Entities;
using PruefungService.Domain.Interfaces;

namespace PruefungService.Application.Services
{
    public class PruefungAppService : IPruefungAppService
    {
        private readonly IPruefungService _pruefungService;
        private readonly IPruefungRepository _pruefungRepository;
        private readonly IAufgabenServiceClient _aufgabenServiceClient;

        public PruefungAppService(
            IPruefungService pruefungService,
            IPruefungRepository pruefungRepository, 
            IAufgabenServiceClient aufgabenServiceClient)
        {
            _pruefungService = pruefungService ?? throw new ArgumentNullException(nameof(pruefungService));
            _pruefungRepository = pruefungRepository ?? throw new ArgumentNullException(nameof(pruefungRepository));
            _aufgabenServiceClient = aufgabenServiceClient ?? throw new ArgumentNullException(nameof(aufgabenServiceClient));
        }

        public async Task<PruefungDto?> GetPruefungByIdAsync(int id)
        {
            var pruefung = await _pruefungRepository.GetByIdAsync(id);
            if (pruefung == null)
                return null;

            return new PruefungDto
            {
                Id = pruefung.Id,
                Titel = pruefung.Titel,
                Datum = pruefung.Datum,
                Zeitlimit = pruefung.Zeitlimit,
                AufgabenIds = pruefung.AufgabenIds.ToList() // Explizite Konvertierung zu List<int>
            };
        }

        public async Task<IEnumerable<PruefungDto>> GetAllePruefungenAsync()
        {
            var pruefungen = await _pruefungRepository.GetAllAsync();
            var result = new List<PruefungDto>();

            foreach (var pruefung in pruefungen)
            {
                result.Add(new PruefungDto
                {
                    Id = pruefung.Id,
                    Titel = pruefung.Titel,
                    Datum = pruefung.Datum,
                    Zeitlimit = pruefung.Zeitlimit,
                    AufgabenIds = pruefung.AufgabenIds.ToList() // Explizite Konvertierung zu List<int>
                });
            }

            return result;
        }

        public async Task<IEnumerable<AufgabeDto>> GetAufgabenFuerPruefungAsync(int pruefungId)
        {
            var pruefung = await _pruefungRepository.GetByIdAsync(pruefungId);
            if (pruefung == null)
                throw new NotFoundException($"Prüfung mit ID {pruefungId} nicht gefunden");

            // Annahme: Es gibt eine GetAufgabenByIdsAsync-Methode statt GetAufgabenAsync
            var aufgaben = await _aufgabenServiceClient.GetAufgabenByIdsAsync(pruefung.AufgabenIds.ToList());
            return aufgaben;
        }

        public async Task<IEnumerable<AufgabeDto>> GetAlleAufgabenAsync()
        {
            // Annahme: Es gibt eine GetAllAufgabenAsync-Methode statt GetAlleAufgabenAsync
            return await _aufgabenServiceClient.GetAllAufgabenAsync();
        }

        public async Task<PruefungDto> ErstellePruefungAsync(PruefungErstellenDto pruefungDto)
        {
            // Anstatt direkter Instantiierung müssen wir eine Factory-Methode oder 
            // einen öffentlichen Konstruktor in der Pruefung-Klasse verwenden
            var pruefung = Pruefung.Create(
                pruefungDto.Titel, 
                pruefungDto.Datum, 
                pruefungDto.Zeitlimit,
                pruefungDto.AufgabenIds ?? new List<int>());

            await _pruefungRepository.AddAsync(pruefung);

            return new PruefungDto
            {
                Id = pruefung.Id,
                Titel = pruefung.Titel,
                Datum = pruefung.Datum,
                Zeitlimit = pruefung.Zeitlimit,
                AufgabenIds = pruefung.AufgabenIds.ToList() // Explizite Konvertierung zu List<int>
            };
        }

        public async Task<PruefungDto?> AktualisierePruefungAsync(int id, PruefungAktualisierenDto pruefungDto)
        {
            var pruefung = await _pruefungRepository.GetByIdAsync(id);
            if (pruefung == null)
                return null;

            // Da Pruefung unveränderlich ist, erstellen wir eine neue Instanz mit den aktualisierten Werten
            var updatedPruefung = Pruefung.Create(
                string.IsNullOrEmpty(pruefungDto.Titel) ? pruefung.Titel : pruefungDto.Titel,
                pruefungDto.Datum != default ? pruefungDto.Datum : pruefung.Datum,
                pruefungDto.Zeitlimit > 0 ? pruefungDto.Zeitlimit : pruefung.Zeitlimit,
                pruefung.AufgabenIds.ToList());

            // Wir müssen sicherstellen, dass die ID beibehalten wird
            updatedPruefung = updatedPruefung.WithId(pruefung.Id);

            await _pruefungRepository.UpdateAsync(updatedPruefung);

            return new PruefungDto
            {
                Id = updatedPruefung.Id,
                Titel = updatedPruefung.Titel,
                Datum = updatedPruefung.Datum,
                Zeitlimit = updatedPruefung.Zeitlimit,
                AufgabenIds = updatedPruefung.AufgabenIds.ToList() // Explizite Konvertierung zu List<int>
            };
        }

        public async Task<PruefungDto?> WeiseAufgabenZuAsync(int id, AufgabenZuweisenDto aufgabenDto)
        {
            var pruefung = await _pruefungRepository.GetByIdAsync(id);
            if (pruefung == null)
                return null;

            // Eine neue Instanz mit den aktualisierten Aufgaben erstellen
            var updatedPruefung = pruefung.WithAufgaben(aufgabenDto.AufgabenIds);

            await _pruefungRepository.UpdateAsync(updatedPruefung);

            return new PruefungDto
            {
                Id = updatedPruefung.Id,
                Titel = updatedPruefung.Titel,
                Datum = updatedPruefung.Datum,
                Zeitlimit = updatedPruefung.Zeitlimit,
                AufgabenIds = updatedPruefung.AufgabenIds.ToList() // Explizite Konvertierung zu List<int>
            };
        }

        public async Task<bool> LoeschePruefungAsync(int id)
        {
            var pruefung = await _pruefungRepository.GetByIdAsync(id);
            if (pruefung == null)
                return false;

            await _pruefungRepository.DeleteAsync(id);
            return true;
        }
    }
}