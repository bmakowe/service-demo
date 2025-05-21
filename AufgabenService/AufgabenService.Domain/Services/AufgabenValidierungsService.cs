using System;
using System.Linq;
using AufgabenService.Domain.Entities;
using AufgabenService.Domain.Exceptions;

namespace AufgabenService.Domain.Services
{
    public class AufgabenValidierungsService
    {
        public static void ValidateAufgabe(Aufgabe aufgabe)
        {
            if (aufgabe == null)
                throw new ArgumentNullException(nameof(aufgabe));

            if (string.IsNullOrWhiteSpace(aufgabe.Frage))
                throw new DomainException("Eine Aufgabe muss eine Frage haben.");

            if (aufgabe.Antworten == null || aufgabe.Antworten.Count < 2)
                throw new DomainException("Eine Aufgabe muss mindestens zwei Antworten haben.");

            if (!aufgabe.Antworten.Any(a => a.IstRichtig))
                throw new DomainException("Mindestens eine Antwort muss als richtig markiert sein.");
                
            if (aufgabe.Antworten.Count(a => a.IstRichtig) > 1)
                throw new DomainException("Es darf nur eine richtige Antwort geben.");

            foreach (var antwort in aufgabe.Antworten)
            {
                if (string.IsNullOrWhiteSpace(antwort.Text))
                    throw new DomainException("Alle Antworten müssen einen Text haben.");
            }
        }
    }
}