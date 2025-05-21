namespace AufgabenService.Domain.Entities
{
    public class Antwort
    {
        public int Id { get; private set; }
        public string Text { get; private set; } = string.Empty; // Standardwert hinzugefügt
        public bool IstRichtig { get; private set; }

        // Für ORM/Deserialisierung
        protected Antwort() 
        {
            Text = string.Empty; // Standardwert im leeren Konstruktor
        }

        public Antwort(int id, string text, bool istRichtig)
        {
            Id = id;
            Text = text;
            IstRichtig = istRichtig;
        }

        public void SetRichtig(bool istRichtig)
        {
            IstRichtig = istRichtig;
        }
    }
}