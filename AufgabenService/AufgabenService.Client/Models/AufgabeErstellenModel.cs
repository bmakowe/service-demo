using System.Collections.Generic;

namespace AufgabenService.Client.Models
{
    public class AufgabeErstellenModel
    {
        public string Frage { get; set; } = string.Empty;
        public List<AnwortErstellenModel> Antworten { get; set; } = new();
    }
}