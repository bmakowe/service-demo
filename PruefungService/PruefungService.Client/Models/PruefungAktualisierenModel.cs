using System;

namespace PruefungService.Client.Models
{
    public class PruefungAktualisierenModel
    {
        public string? Titel { get; set; }
        public DateTime Datum { get; set; }
        public int Zeitlimit { get; set; }
    }
}