using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruefungService.Application.DTOs;
using PruefungService.Application.Exceptions;
using PruefungService.Application.Interfaces;
using PruefungService.Domain.Entities;
using PruefungService.Domain.Services; // Für PruefungValidierungsService, falls existiert
using PruefungService.Domain.ValueObjects; // Für Aufgabe, wenn es ein ValueObject ist
using PruefungService.Domain.Interfaces; // Für IAufgabenService, falls existiert

namespace PruefungService.Application.Services
{
    public class PruefungAppService : IPruefungAppService
    {
        private readonly IPruefungRepository _repository;
        private readonly IAufgabenServiceClient _aufgabenServiceClient; // Verwende IAufgabenServiceClient statt IAufgabenService
        
        public PruefungAppService(
            IPruefungRepository repository,
            IAufgabenServiceClient aufgabenServiceClient) // Korrigierte Abhängigkeit
        {
            _repository = repository;
            _aufgabenServiceClient = aufgabenServiceClient;
        }

        public async Task<IEnumerable<PruefungDto>> GetAllPruefungenAsync()
        {
            var pruefungen = await _repository.GetAllAsync();
            return pruefungen.Select(MapToDto);
        }

        public async Task<PruefungDto> GetPruefungByIdAsync(int id)
        {
            try
            {
                var pruefung = await _repository.GetByIdAsync(id);
                return MapToDto(pruefung);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<AufgabeDto>> GetAufgabenForPruefungAsync(int pruefungId)
        {
            try
            {
                var pruefung = await _repository.GetByIdAsync(pruefungId);
                
                if (!pruefung.AufgabenIds.Any())
                    return new List<AufgabeDto>();
                
                var aufgaben = await _aufgabenServiceClient.GetAufgabenByIdsAsync(pruefung.AufgabenIds);
                return aufgaben;
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException(ex.Message, ex);
            }
        }

        public async Task<PruefungDto> CreatePruefungAsync(PruefungErstellenDto pruefungDto)
        {
            if (pruefungDto == null)
                throw new ValidationException("Prüfungsdaten wurden nicht übermittelt.");

            var pruefung = new Pruefung(
                pruefungDto.Titel,
                pruefungDto.Datum,
                pruefungDto.Zeitlimit,
                pruefungDto.AufgabenIds?.ToList() ?? new List<int>()
            );

            var createdPruefung = await _repository.AddAsync(pruefung);
            return MapToDto(createdPruefung);
        }

        public async Task<PruefungDto> UpdatePruefungAsync(int id, PruefungAktualisierenDto pruefungDto)
        {
            if (pruefungDto == null)
                throw new ValidationException("Prüfungsdaten wurden nicht übermittelt.");

            try
            {
                var pruefung = await _repository.GetByIdAsync(id);
                
                if (!string.IsNullOrEmpty(pruefungDto.Titel))
                    pruefung.UpdateTitel(pruefungDto.Titel);
                
                if (pruefungDto.Datum != default)
                    pruefung.UpdateDatum(pruefungDto.Datum);
                
                if (pruefungDto.Zeitlimit > 0)
                    pruefung.UpdateZeitlimit(pruefungDto.Zeitlimit);

                var updatedPruefung = await _repository.UpdateAsync(pruefung);
                return MapToDto(updatedPruefung);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException(ex.Message, ex);
            }
        }

        public async Task<PruefungDto> UpdatePruefungAufgabenAsync(int id, AufgabenZuweisenDto aufgabenDto)
        {
            if (aufgabenDto == null)
                throw new ValidationException("Aufgabendaten wurden nicht übermittelt.");

            try
            {
                var pruefung = await _repository.GetByIdAsync(id);
                pruefung.UpdateAufgabenIds(aufgabenDto.AufgabenIds);

                var updatedPruefung = await _repository.UpdateAsync(pruefung);
                return MapToDto(updatedPruefung);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException(ex.Message, ex);
            }
        }

        public async Task<bool> DeletePruefungAsync(int id)
        {
            try
            {
                await _repository.GetByIdAsync(id);
                return await _repository.DeleteAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException(ex.Message, ex);
            }
        }

        private PruefungDto MapToDto(Pruefung pruefung)
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
    }
}