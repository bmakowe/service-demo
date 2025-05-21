using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AufgabenService.Application.DTOs;
using AufgabenService.Application.Exceptions;
using AufgabenService.Application.Interfaces;
using AufgabenService.Domain.Entities;
using AufgabenService.Domain.Services;

namespace AufgabenService.Application.Services
{
    public class AufgabenService : IAufgabenService
    {
        private readonly IAufgabenRepository _repository;

        public AufgabenService(IAufgabenRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AufgabeDto>> GetAllAufgabenAsync()
        {
            var aufgaben = await _repository.GetAllAsync();
            return aufgaben.Select(MapToDto);
        }

        public async Task<AufgabeDto> GetAufgabeByIdAsync(int id)
        {
            try
            {
                var aufgabe = await _repository.GetByIdAsync(id);
                return MapToDto(aufgabe);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException(ex.Message, ex);
            }
        }

        public async Task<AufgabeDto> CreateAufgabeAsync(AufgabeErstellenDto aufgabeDto)
        {
            ValidateAufgabeErstellenDto(aufgabeDto);
            
            var aufgabe = new Aufgabe(aufgabeDto.Frage);
            
            foreach (var antwortDto in aufgabeDto.Antworten)
            {
                aufgabe.AddAntwort(antwortDto.Text, antwortDto.IstRichtig);
            }

            try
            {
                AufgabenValidierungsService.ValidateAufgabe(aufgabe);
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                throw new ValidationException(ex.Message, ex);
            }

            var createdAufgabe = await _repository.AddAsync(aufgabe);
            return MapToDto(createdAufgabe);
        }

        public async Task<AufgabeDto> UpdateAufgabeAsync(int id, AufgabeAktualisierenDto aufgabeDto)
        {
            ValidateAufgabeAktualisierenDto(aufgabeDto);
            
            try
            {
                var aufgabe = await _repository.GetByIdAsync(id);
                
                aufgabe.UpdateFrage(aufgabeDto.Frage);
                
                var antworten = aufgabeDto.Antworten
                    .Select((a, i) => (Id: i + 1, Text: a.Text, IstRichtig: a.IstRichtig))
                    .ToList();
                    
                aufgabe.UpdateAntworten(antworten);

                try
                {
                    AufgabenValidierungsService.ValidateAufgabe(aufgabe);
                }
                catch (Domain.Exceptions.DomainException ex)
                {
                    throw new ValidationException(ex.Message, ex);
                }

                var updatedAufgabe = await _repository.UpdateAsync(aufgabe);
                return MapToDto(updatedAufgabe);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAufgabeAsync(int id)
        {
            try
            {
                // Prüfen, ob die Aufgabe existiert
                await _repository.GetByIdAsync(id);
                
                return await _repository.DeleteAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException(ex.Message, ex);
            }
        }

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
        
        private void ValidateAufgabeErstellenDto(AufgabeErstellenDto dto)
        {
            if (dto == null)
                throw new ValidationException("Aufgabendaten wurden nicht übermittelt.");
                
            if (string.IsNullOrWhiteSpace(dto.Frage))
                throw new ValidationException("Eine Aufgabe muss eine Frage haben.");
                
            if (dto.Antworten == null || dto.Antworten.Count < 2)
                throw new ValidationException("Eine Aufgabe muss mindestens zwei Antworten haben.");
                
            if (!dto.Antworten.Any(a => a.IstRichtig))
                throw new ValidationException("Mindestens eine Antwort muss als richtig markiert sein.");
                
            if (dto.Antworten.Count(a => a.IstRichtig) > 1)
                throw new ValidationException("Es darf nur eine richtige Antwort geben.");
                
            foreach (var antwort in dto.Antworten)
            {
                if (string.IsNullOrWhiteSpace(antwort.Text))
                    throw new ValidationException("Alle Antworten müssen einen Text haben.");
            }
        }
        
        private void ValidateAufgabeAktualisierenDto(AufgabeAktualisierenDto dto)
        {
            if (dto == null)
                throw new ValidationException("Aufgabendaten wurden nicht übermittelt.");
                
            if (string.IsNullOrWhiteSpace(dto.Frage))
                throw new ValidationException("Eine Aufgabe muss eine Frage haben.");
                
            if (dto.Antworten == null || dto.Antworten.Count < 2)
                throw new ValidationException("Eine Aufgabe muss mindestens zwei Antworten haben.");
                
            if (!dto.Antworten.Any(a => a.IstRichtig))
                throw new ValidationException("Mindestens eine Antwort muss als richtig markiert sein.");
                
            if (dto.Antworten.Count(a => a.IstRichtig) > 1)
                throw new ValidationException("Es darf nur eine richtige Antwort geben.");
                
            foreach (var antwort in dto.Antworten)
            {
                if (string.IsNullOrWhiteSpace(antwort.Text))
                    throw new ValidationException("Alle Antworten müssen einen Text haben.");
            }
        }
    }
}