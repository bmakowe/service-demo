namespace PruefungService.Domain.ValueObjects
{
    public class Antwort
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IstRichtig { get; set; }

        // Private Constructor für Deserialisierung
        private Antwort() { }

        public Antwort(int id, string text, bool istRichtig)
        {
            Id = id;
            Text = text;
            IstRichtig = istRichtig;
        }
    }
}