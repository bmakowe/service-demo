using System.Linq;
using PruefungService.Application.DTOs;
using PruefungService.Domain.Entities;

namespace PruefungService.Application.Mapping
{
    public static class MappingProfile
    {
        public static PruefungDto MapToDto(Pruefung pruefung)
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