using AufgabenService.Application.DTOs;
using AufgabenService.Domain.Entities;
using System.Linq;

namespace AufgabenService.Application.Mapping
{
    public static class MappingProfile
    {
        public static AufgabeDto MapToDto(Aufgabe aufgabe)
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