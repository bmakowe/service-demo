namespace AufgabenService.Domain.Entities
{
    public class Aufgabe
    {
        public int Id { get; set; }
        public string Frage { get; set; } = string.Empty;
        public List<Antwort> Antworten { get; set; } = new();
    }
}