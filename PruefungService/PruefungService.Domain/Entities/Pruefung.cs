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

        public void SetId(int id)
        {
            Id = id;
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