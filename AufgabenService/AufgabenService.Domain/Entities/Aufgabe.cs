using System;
using System.Collections.Generic;
using System.Linq;

namespace AufgabenService.Domain.Entities
{
    public class Aufgabe
    {
        public int Id { get; private set; }
        public string Frage { get; private set; } = string.Empty; // Standardwert hinzugefügt
        private List<Antwort> _antworten = new();
        public IReadOnlyCollection<Antwort> Antworten => _antworten.AsReadOnly();

        // Für ORM/Deserialisierung
        protected Aufgabe() 
        {
            Frage = string.Empty; // Standardwert im leeren Konstruktor
        }

        public Aufgabe(string frage)
        {
            if (string.IsNullOrWhiteSpace(frage))
                throw new ArgumentException("Frage darf nicht leer sein", nameof(frage));
                
            Frage = frage;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void UpdateFrage(string frage)
        {
            if (string.IsNullOrWhiteSpace(frage))
                throw new ArgumentException("Frage darf nicht leer sein", nameof(frage));
                
            Frage = frage;
        }

        public void AddAntwort(string text, bool istRichtig)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Antworttext darf nicht leer sein", nameof(text));
                
            int nextId = _antworten.Count > 0 ? _antworten.Max(a => a.Id) + 1 : 1;
            _antworten.Add(new Antwort(nextId, text, istRichtig));
            
            EnsureSingleRichtigeAntwort();
        }

        public void UpdateAntworten(List<(int Id, string Text, bool IstRichtig)> antworten)
        {
            if (antworten == null || antworten.Count < 2)
                throw new ArgumentException("Eine Aufgabe muss mindestens 2 Antworten haben");
                
            _antworten.Clear();
            
            foreach (var (id, text, istRichtig) in antworten)
            {
                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentException("Antworttext darf nicht leer sein");
                    
                _antworten.Add(new Antwort(id, text, istRichtig));
            }
            
            EnsureSingleRichtigeAntwort();
        }

        private void EnsureSingleRichtigeAntwort()
        {
            // Sicherstellen, dass genau eine Antwort richtig ist
            if (!_antworten.Any(a => a.IstRichtig))
            {
                if (_antworten.Any())
                    _antworten[0].SetRichtig(true);
            }
            else if (_antworten.Count(a => a.IstRichtig) > 1)
            {
                var richtigeAntworten = _antworten.Where(a => a.IstRichtig).Skip(1).ToList();
                foreach (var antwort in richtigeAntworten)
                {
                    antwort.SetRichtig(false);
                }
            }
        }
    }
}