namespace AufgabenService.Client.Models
{
    public class AufgabenViewModel
    {
        public int Id { get; set; }
        public string Frage { get; set; } = string.Empty;
        public List<AntwortViewModel> Antworten { get; set; } = new();
    }
}