namespace PruefungService.Domain.ValueObjects
{
    // Diese Klasse repräsentiert eine Aufgabe, die vom AufgabenService abgerufen wird
    // Es ist ein Value Object, keine Domain-Entity, da es nur eine Kopie der Daten
    // aus dem anderen Service darstellt
    public class Aufgabe
    {
        public int Id { get; private set; }
        public string Frage { get; private set; }
        public IReadOnlyCollection<Antwort> Antworten { get; private set; }

        // Private Constructor für Deserialisierung
        private Aufgabe() { }

        public Aufgabe(int id, string frage, IEnumerable<Antwort> antworten)
        {
            Id = id;
            Frage = frage;
            Antworten = antworten.ToList().AsReadOnly();
        }
    }

    public class Antwort
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public bool IstRichtig { get; private set; }

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