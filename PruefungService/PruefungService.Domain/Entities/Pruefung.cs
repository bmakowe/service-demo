using System;
using System.Collections.Generic;
using System.Linq;

namespace PruefungService.Domain.Entities
{
    public class Pruefung
    {
        public int Id { get; private set; }
        public string Titel { get; private set; } = string.Empty;
        private List<int> _aufgabenIds = new();
        public IReadOnlyCollection<int> AufgabenIds => _aufgabenIds.AsReadOnly();
        public DateTime Datum { get; private set; }
        public int Zeitlimit { get; private set; } // in Minuten

        // Für ORM/Deserialisierung
        protected Pruefung() { }

        public Pruefung(string titel, DateTime datum, int zeitlimit, List<int>? aufgabenIds = null)
        {
            if (string.IsNullOrWhiteSpace(titel))
                throw new ArgumentException("Titel darf nicht leer sein", nameof(titel));
                
            if (zeitlimit <= 0)
                throw new ArgumentException("Zeitlimit muss größer als 0 sein", nameof(zeitlimit));
                
            Titel = titel;
            Datum = datum;
            Zeitlimit = zeitlimit;
            
            if (aufgabenIds != null)
                _aufgabenIds = new List<int>(aufgabenIds);
        }

        // Factory-Methode zum Erstellen von Pruefung-Objekten
        public static Pruefung Create(string titel, DateTime datum, int zeitlimit, List<int> aufgabenIds)
        {
            return new Pruefung(titel, datum, zeitlimit, aufgabenIds);
        }

        // Methode zum Setzen der ID
        public void SetId(int id)
        {
            Id = id;
        }

        // Immutable-Stil-Methode für das Setzen der ID
        public Pruefung WithId(int id)
        {
            var pruefung = new Pruefung(this.Titel, this.Datum, this.Zeitlimit, this._aufgabenIds.ToList());
            pruefung.SetId(id);
            return pruefung;
        }

        // Immutable-Stil-Methode für das Aktualisieren der Aufgaben
        public Pruefung WithAufgaben(List<int> aufgabenIds)
        {
            var pruefung = new Pruefung(this.Titel, this.Datum, this.Zeitlimit, aufgabenIds);
            pruefung.SetId(this.Id);
            return pruefung;
        }

        public void UpdateTitel(string titel)
        {
            if (string.IsNullOrWhiteSpace(titel))
                throw new ArgumentException("Titel darf nicht leer sein", nameof(titel));
                
            Titel = titel;
        }

        public void UpdateDatum(DateTime datum)
        {
            Datum = datum;
        }

        public void UpdateZeitlimit(int zeitlimit)
        {
            if (zeitlimit <= 0)
                throw new ArgumentException("Zeitlimit muss größer als 0 sein", nameof(zeitlimit));
                
            Zeitlimit = zeitlimit;
        }

        public void UpdateAufgabenIds(List<int> aufgabenIds)
        {
            _aufgabenIds.Clear();
            if (aufgabenIds != null)
                _aufgabenIds.AddRange(aufgabenIds);
        }
    }
}