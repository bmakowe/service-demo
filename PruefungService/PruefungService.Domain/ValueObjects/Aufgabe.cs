using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PruefungService.Domain.ValueObjects
{
    public class Aufgabe
    {
        public int Id { get; set; }
        public string Frage { get; set; } = string.Empty;
        
        // Private Liste für die Antworten
        private List<Antwort> _antworten = new();
        
        // Öffentliche ReadOnly-Property für Antworten
        public IReadOnlyCollection<Antwort> Antworten => _antworten.AsReadOnly();
        

        public Aufgabe()
        {
            Frage = string.Empty;
            _antworten = new List<Antwort>();
        }
        
        // Methode zum Hinzufügen von Antworten
        public void AddAntwort(Antwort antwort)
        {
            _antworten.Add(antwort);
        }
        
        // Methode zum Entfernen von Antworten
        public void RemoveAntwort(Antwort antwort)
        {
            _antworten.Remove(antwort);
        }
        
        // Methode zum Aktualisieren der Antwortenliste
        public void SetAntworten(List<Antwort> antworten)
        {
            _antworten.Clear();
            if (antworten != null)
            {
                _antworten.AddRange(antworten);
            }
        }
        
        // Falls du eine Property für die Liste selbst brauchst (z.B. für Serialisierung)
        public List<Antwort> GetAntwortListe()
        {
            return _antworten.ToList(); // Gibt eine Kopie zurück
        }
    }
}