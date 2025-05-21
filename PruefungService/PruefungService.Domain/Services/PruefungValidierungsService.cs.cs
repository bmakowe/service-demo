using PruefungService.Domain.Entities;

namespace PruefungService.Domain.Services
{
    public class PruefungValidierungsService
    {
        public bool IstPruefungGueltig(Pruefung pruefung)
        {
            // Titel darf nicht leer sein
            if (string.IsNullOrWhiteSpace(pruefung.Titel))
                return false;
            
            // Zeitlimit muss größer als 0 sein
            if (pruefung.Zeitlimit <= 0)
                return false;
            
            // Eine Prüfung sollte mindestens eine Aufgabe haben
            if (!pruefung.AufgabenIds.Any())
                return false;
            
            return true;
        }
    }
}