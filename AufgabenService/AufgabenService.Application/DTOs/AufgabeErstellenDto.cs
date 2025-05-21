using System.Collections.Generic;

namespace AufgabenService.Application.DTOs
{
    public class AufgabeErstellenDto
    {
        public string Frage { get; set; } = string.Empty;
        public List<AntwortErstellenDto> Antworten { get; set; } = new();
    }
}