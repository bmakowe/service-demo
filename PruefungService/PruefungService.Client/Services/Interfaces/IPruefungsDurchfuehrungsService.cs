using PruefungService.Client.Models;

namespace PruefungService.Client.Services.Interfaces
{
    // Service für die Client-seitige Logik der Prüfungsdurchführung
    public interface IPruefungsDurchfuehrungsService
    {
        PruefungViewModel? AktuellePruefung { get; }
        List<AufgabeViewModel>? AufgabenListe { get; }
        int VerbleibendeZeit { get; }
        bool PruefungBeendet { get; }
        
        Task<bool> StartePruefungAsync(int pruefungId);
        void StartTimer();
        void StopTimer();
        void BeendePruefung();
        void WaehleAntwort(int aufgabeId, int antwortId);
        PruefungsErgebnisModel BerechnePruefungsErgebnis();
    }
}