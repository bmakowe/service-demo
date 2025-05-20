namespace AufgabenService.Domain.Entities
{
    public class Aufgabe
    {
        public int Id { get; private set; }
        public string Frage { get; private set; }
        private readonly List<Antwort> _antworten = new();
        public IReadOnlyCollection<Antwort> Antworten => _antworten.AsReadOnly();

        // Private Constructor für EF Core
        private Aufgabe() { }

        public Aufgabe(string frage)
        {
            if (string.IsNullOrWhiteSpace(frage))
                throw new ArgumentException("Frage darf nicht leer sein", nameof(frage));
            
            Frage = frage;
        }

        public void AendereFragetext(string neuerFragetext)
        {
            if (string.IsNullOrWhiteSpace(neuerFragetext))
                throw new ArgumentException("Frage darf nicht leer sein", nameof(neuerFragetext));
            
            Frage = neuerFragetext;
        }

        public void FuegeAntwortHinzu(string antwortText, bool istRichtig)
        {
            if (string.IsNullOrWhiteSpace(antwortText))
                throw new ArgumentException("Antworttext darf nicht leer sein", nameof(antwortText));
            
            // Wenn eine richtige Antwort hinzugefügt wird, alle anderen auf falsch setzen
            if (istRichtig)
            {
                foreach (var existierendeAntwort in _antworten)
                {
                    existierendeAntwort.SetzeRichtigkeit(false);
                }
            }
            
            var neueId = _antworten.Count > 0 ? _antworten.Max(a => a.Id) + 1 : 1;
            _antworten.Add(new Antwort(neueId, antwortText, istRichtig));
        }

        public void EntferneAntwort(int antwortId)
        {
            var antwort = _antworten.FirstOrDefault(a => a.Id == antwortId);
            if (antwort == null)
                throw new KeyNotFoundException($"Antwort mit ID {antwortId} nicht gefunden.");
            
            bool warRichtig = antwort.IstRichtig;
            _antworten.Remove(antwort);
            
            // Wenn die richtige Antwort entfernt wurde und noch andere Antworten existieren,
            // die erste als richtig markieren
            if (warRichtig && _antworten.Any())
            {
                _antworten.First().SetzeRichtigkeit(true);
            }
        }

        public void SetzeRichtigeAntwort(int antwortId)
        {
            var antwort = _antworten.FirstOrDefault(a => a.Id == antwortId);
            if (antwort == null)
                throw new KeyNotFoundException($"Antwort mit ID {antwortId} nicht gefunden.");
            
            foreach (var existierendeAntwort in _antworten)
            {
                existierendeAntwort.SetzeRichtigkeit(existierendeAntwort.Id == antwortId);
            }
        }

        // Factory-Methode für das Erstellen von kompletten Aufgaben
        public static Aufgabe Erstellen(int id, string frage, List<(string Text, bool IstRichtig)> antworten)
        {
            var aufgabe = new Aufgabe(frage) { Id = id };
            
            foreach (var (text, istRichtig) in antworten)
            {
                aufgabe.FuegeAntwortHinzu(text, istRichtig);
            }
            
            // Sicherstellen, dass mindestens eine Antwort als richtig markiert ist
            if (!aufgabe.Antworten.Any(a => a.IstRichtig) && aufgabe.Antworten.Any())
            {
                aufgabe._antworten.First().SetzeRichtigkeit(true);
            }
            
            return aufgabe;
        }
    }
}