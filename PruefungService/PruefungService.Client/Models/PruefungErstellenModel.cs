using System;
using System.Collections.Generic;

namespace PruefungService.Client.Models
{
    public class PruefungErstellenModel
    {
        public string Titel { get; set; } = string.Empty;
        public DateTime Datum { get; set; } = DateTime.Now.AddDays(7);
        public int Zeitlimit { get; set; } = 30;
        public List<int>? AufgabenIds { get; set; }
    }
}