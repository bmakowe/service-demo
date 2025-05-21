using AufgabenService.Domain.Entities;

namespace AufgabenService.Domain.Services
{
    public class AufgabenValidierungsService
    {
        public bool IstAufgabeGueltig(Aufgabe aufgabe)
        {
            // Mindestens 2 Antworten erforderlich
            if (aufgabe.Antworten.Count < 2)
                return false;
            
            // Eine Antwort muss als richtig markiert sein
            if (!aufgabe.Antworten.Any(a => a.IstRichtig))
                return false;
            
            // Frage darf nicht leer sein
            if (string.IsNullOrWhiteSpace(aufgabe.Frage))
                return false;
            
            // Alle Antworttexte müssen gefüllt sein
            if (aufgabe.Antworten.Any(a => string.IsNullOrWhiteSpace(a.Text)))
                return false;
            
            return true;
        }
    }
}