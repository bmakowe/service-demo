namespace PruefungService.Domain.Entities
{
    public class Pruefung
    {
        public int Id { get; private set; }
        public string Titel { get; private set; }
        private readonly List<int> _aufgabenIds = new();
        public IReadOnlyCollection<int> AufgabenIds => _aufgabenIds.AsReadOnly();
        public DateTime Datum { get; private set; }
        public int Zeitlimit { get; private set; }  // in Minuten

        // Private Constructor für EF Core
        private Pruefung() { }

        public Pruefung(string titel, DateTime datum, int zeitlimit)
        {
            if (string.IsNullOrWhiteSpace(titel))
                throw new ArgumentException("Titel darf nicht leer sein", nameof(titel));
            
            if (zeitlimit <= 0)
                throw new ArgumentException("Zeitlimit muss größer als 0 sein", nameof(zeitlimit));
            
            Titel = titel;
            Datum = datum;
            Zeitlimit = zeitlimit;
        }

        public void AendereTitel(string neuerTitel)
        {
            if (string.IsNullOrWhiteSpace(neuerTitel))
                throw new ArgumentException("Titel darf nicht leer sein", nameof(neuerTitel));
            
            Titel = neuerTitel;
        }

        public void AendereDatum(DateTime neuesDatum)
        {
            Datum = neuesDatum;
        }

        public void AendereZeitlimit(int neuesZeitlimit)
        {
            if (neuesZeitlimit <= 0)
                throw new ArgumentException("Zeitlimit muss größer als 0 sein", nameof(neuesZeitlimit));
            
            Zeitlimit = neuesZeitlimit;
        }

        public void SetzeAufgaben(IEnumerable<int> aufgabenIds)
        {
            _aufgabenIds.Clear();
            _aufgabenIds.AddRange(aufgabenIds);
        }

        public void FuegeAufgabeHinzu(int aufgabeId)
        {
            if (!_aufgabenIds.Contains(aufgabeId))
            {
                _aufgabenIds.Add(aufgabeId);
            }
        }

        public void EntferneAufgabe(int aufgabeId)
        {
            _aufgabenIds.Remove(aufgabeId);
        }

        // Factory-Methode für das Erstellen von kompletten Prüfungen
        public static Pruefung Erstellen(int id, string titel, DateTime datum, int zeitlimit, IEnumerable<int> aufgabenIds)
        {
            var pruefung = new Pruefung(titel, datum, zeitlimit) { Id = id };
            pruefung.SetzeAufgaben(aufgabenIds);
            return pruefung;
        }
    }
}