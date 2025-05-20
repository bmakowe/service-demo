namespace AufgabenService.Domain.Entities
{
    public class Antwort
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public bool IstRichtig { get; private set; }

        // Private Constructor für EF Core
        private Antwort() { }

        public Antwort(int id, string text, bool istRichtig)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Antworttext darf nicht leer sein", nameof(text));
            
            Id = id;
            Text = text;
            IstRichtig = istRichtig;
        }

        public void AendereText(string neuerText)
        {
            if (string.IsNullOrWhiteSpace(neuerText))
                throw new ArgumentException("Antworttext darf nicht leer sein", nameof(neuerText));
            
            Text = neuerText;
        }

        public void SetzeRichtigkeit(bool istRichtig)
        {
            IstRichtig = istRichtig;
        }
    }
}