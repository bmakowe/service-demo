namespace AufgabenService.Domain.Entities
{
    public class Antwort
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IstRichtig { get; set; }
    }
}