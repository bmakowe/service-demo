using System.Collections.Generic;

namespace PruefungService.Application.DTOs
{
    public class AufgabeDto
    {
        public int Id { get; set; }
        public string Frage { get; set; } = string.Empty;
        public List<AntwortDto> Antworten { get; set; } = new();
    }
}