using System.Collections.Generic;

namespace AufgabenService.Application.DTOs
{
    public class AufgabeAktualisierenDto
    {
        public string Frage { get; set; } = string.Empty;
        public List<AntwortErstellenDto> Antworten { get; set; } = new();
    }
}