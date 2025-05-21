using System.Collections.Generic;

namespace PruefungService.Application.DTOs
{
    public class AufgabenZuweisenDto
    {
        public List<int> AufgabenIds { get; set; } = new();
    }
}