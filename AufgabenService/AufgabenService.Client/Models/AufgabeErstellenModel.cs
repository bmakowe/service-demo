namespace AufgabenService.Client.Models
{
    public class AufgabeErstellenModel
    {
        public string Frage { get; set; } = string.Empty;
        public List<AntwortErstellenModel> Antworten { get; set; } = new();
    }
}