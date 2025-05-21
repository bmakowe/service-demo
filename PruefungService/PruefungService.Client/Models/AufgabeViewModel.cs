using System.Collections.Generic;

namespace PruefungService.Client.Models
{
    public class AufgabeViewModel
    {
        public int Id { get; set; }
        public string Frage { get; set; } = string.Empty;
        public List<AntwortViewModel> Antworten { get; set; } = new();
    }
}