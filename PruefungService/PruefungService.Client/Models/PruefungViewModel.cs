using System;
using System.Collections.Generic;

namespace PruefungService.Client.Models
{
    public class PruefungViewModel
    {
        public int Id { get; set; }
        public string Titel { get; set; } = string.Empty;
        public List<int> AufgabenIds { get; set; } = new();
        public DateTime Datum { get; set; }
        public int Zeitlimit { get; set; } // in Minuten
    }
}