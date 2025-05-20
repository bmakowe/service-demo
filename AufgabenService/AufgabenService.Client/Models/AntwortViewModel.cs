namespace AufgabenService.Client.Models
{
    public class AntwortViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IstRichtig { get; set; }
    }
}