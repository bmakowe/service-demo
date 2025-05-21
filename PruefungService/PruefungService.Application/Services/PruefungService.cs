using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruefungService.Application.DTOs;
using PruefungService.Application.Exceptions;
using PruefungService.Application.Interfaces;
using PruefungService.Domain.Entities;

namespace PruefungService.Application.Services
{
    public class PruefungService : IPruefungService
    {
        private readonly IPruefungRepository _repository;
        private readonly IAufgabenServiceClient _aufgabenServiceClient;

        public PruefungService(IPruefungRepository repository, IAufgabenServiceClient aufgabenServiceClient)
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
            var pruefung = await _repository.GetByIdAsync(id);
            if (pruefung == null)
                throw new NotFoundException($"Prüfung mit ID {id} wurde nicht gefunden.");
                
            return MapToDto(pruefung);
        }

        public async Task<IEnumerable<AufgabeDto>> GetAufgabenForPruefungAsync(int pruefungId)
        {
            var pruefung = await _repository.GetByIdAsync(pruefungId);
            if (pruefung == null)
                throw new NotFoundException($"Prüfung mit ID {pruefungId} wurde nicht gefunden.");

            if (!pruefung.AufgabenIds.Any())
                return new List<AufgabeDto>();

            return await _aufgabenServiceClient.GetAufgabenByIdsAsync(pruefung.AufgabenIds);
        }

        public async Task<PruefungDto> CreatePruefungAsync(PruefungErstellenDto pruefungDto)
        {
            ValidatePruefungErstellenDto(pruefungDto);

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
            var pruefung = await _repository.GetByIdAsync(id);
            if (pruefung == null)
                throw new NotFoundException($"Prüfung mit ID {id} wurde nicht gefunden.");

            if (!string.IsNullOrEmpty(pruefungDto.Titel))
                pruefung.UpdateTitel(pruefungDto.Titel);
                
            if (pruefungDto.Datum != default)
                pruefung.UpdateDatum(pruefungDto.Datum);
                
            if (pruefungDto.Zeitlimit > 0)
                pruefung.UpdateZeitlimit(pruefungDto.Zeitlimit);

            var updatedPruefung = await _repository.UpdateAsync(pruefung);
            return MapToDto(updatedPruefung);
        }

        public async Task<PruefungDto> UpdatePruefungAufgabenAsync(int id, AufgabenZuweisenDto aufgabenDto)
        {
            var pruefung = await _repository.GetByIdAsync(id);
            if (pruefung == null)
                throw new NotFoundException($"Prüfung mit ID {id} wurde nicht gefunden.");

            pruefung.UpdateAufgabenIds(aufgabenDto.AufgabenIds);

            var updatedPruefung = await _repository.UpdateAsync(pruefung);
            return MapToDto(updatedPruefung);
        }

        public async Task<bool> DeletePruefungAsync(int id)
        {
            var pruefung = await _repository.GetByIdAsync(id);
            if (pruefung == null)
                throw new NotFoundException($"Prüfung mit ID {id} wurde nicht gefunden.");
                
            return await _repository.DeleteAsync(id);
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
        
        private void ValidatePruefungErstellenDto(PruefungErstellenDto dto)
        {
            if (dto == null)
                throw new ValidationException("Prüfungsdaten wurden nicht übermittelt.");
                
            if (string.IsNullOrWhiteSpace(dto.Titel))
                throw new ValidationException("Eine Prüfung muss einen Titel haben.");
                
            if (dto.Zeitlimit <= 0)
                throw new ValidationException("Das Zeitlimit muss größer als 0 sein.");
        }
    }
}