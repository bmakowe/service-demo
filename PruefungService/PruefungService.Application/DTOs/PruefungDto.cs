using System;
using System.Collections.Generic;

namespace PruefungService.Application.DTOs
{
    public class PruefungDto
    {
        public int Id { get; set; }
        public string Titel { get; set; } = string.Empty;
        public List<int> AufgabenIds { get; set; } = new();
        public DateTime Datum { get; set; }
        public int Zeitlimit { get; set; } // in Minuten
    }
}