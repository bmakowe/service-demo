using AutoMapper;
using AufgabenService.Application.DTOs;
using AufgabenService.Application.Exceptions;
using AufgabenService.Application.Interfaces;
using AufgabenService.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AufgabenService.Application.Services
{
    public class AufgabenAppService : IAufgabenService
    {
        private readonly IAufgabenRepository _aufgabenRepository;
        private readonly IMapper _mapper;

        public AufgabenAppService(IAufgabenRepository aufgabenRepository, IMapper mapper)
        {
            _aufgabenRepository = aufgabenRepository;
            _mapper = mapper;
        }

        public async Task<List<AufgabeDto>> GetAlleAufgabenAsync()
        {
            var aufgaben = await _aufgabenRepository.GetAlleAufgabenAsync();
            return _mapper.Map<List<AufgabeDto>>(aufgaben);
        }

        public async Task<AufgabeDto?> GetAufgabeByIdAsync(int id)
        {
            var aufgabe = await _aufgabenRepository.GetAufgabeByIdAsync(id);
            return aufgabe != null ? _mapper.Map<AufgabeDto>(aufgabe) : null;
        }

        public async Task<AufgabeDto> ErstelleAufgabeAsync(AufgabeErstellenDto aufgabeDto)
        {
            ValidateAufgabe(aufgabeDto.Frage, aufgabeDto.Antworten);

            var aufgabe = new Aufgabe
            {
                Frage = aufgabeDto.Frage,
                Antworten = aufgabeDto.Antworten.Select((a, index) => new Antwort
                {
                    Id = index + 1,
                    Text = a.Text,
                    IstRichtig = a.IstRichtig
                }).ToList()
            };

            var erstellteAufgabe = await _aufgabenRepository.CreateAufgabeAsync(aufgabe);
            return _mapper.Map<AufgabeDto>(erstellteAufgabe);
        }

        public async Task<AufgabeDto?> AktualisiereAufgabeAsync(int id, AufgabeAktualisierenDto aufgabeDto)
        {
            var aufgabe = await _aufgabenRepository.GetAufgabeByIdAsync(id);
            if (aufgabe == null)
            {
                return null;
            }

            ValidateAufgabe(aufgabeDto.Frage, aufgabeDto.Antworten);

            aufgabe.Frage = aufgabeDto.Frage;
            aufgabe.Antworten.Clear();
            
            for (int i = 0; i < aufgabeDto.Antworten.Count; i++)
            {
                var antwortDto = aufgabeDto.Antworten[i];
                aufgabe.Antworten.Add(new Antwort
                {
                    Id = i + 1,
                    Text = antwortDto.Text,
                    IstRichtig = antwortDto.IstRichtig
                });
            }

            var aktualisierteAufgabe = await _aufgabenRepository.UpdateAufgabeAsync(aufgabe);
            return aktualisierteAufgabe != null ? _mapper.Map<AufgabeDto>(aktualisierteAufgabe) : null;
        }

        public async Task<bool> LoescheAufgabeAsync(int id)
        {
            return await _aufgabenRepository.DeleteAufgabeAsync(id);
        }

        private void ValidateAufgabe(string frage, List<AntwortErstellenDto> antworten)
        {
            if (string.IsNullOrWhiteSpace(frage))
            {
                throw new ValidationException("Die Frage darf nicht leer sein.");
            }

            if (antworten.Count < 2)
            {
                throw new ValidationException("Eine Aufgabe muss mindestens zwei Antwortmöglichkeiten haben.");
            }

            if (!antworten.Any(a => a.IstRichtig))
            {
                throw new ValidationException("Mindestens eine Antwort muss als richtig markiert sein.");
            }

            if (antworten.Any(a => string.IsNullOrWhiteSpace(a.Text)))
            {
                throw new ValidationException("Alle Antworten müssen einen Text haben.");
            }
        }
    }
}