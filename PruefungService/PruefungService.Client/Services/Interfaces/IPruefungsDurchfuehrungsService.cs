using PruefungService.Client.Models;

namespace PruefungService.Client.Services.Interfaces
{
    public interface IPruefungsDurchfuehrungsService
    {
        PruefungViewModel? AktuellePruefung { get; }
        List<AufgabeViewModel>? AktuellePruefungsaufgaben { get; }
        bool PruefungBeendet { get; }
        int VerbleibendeZeit { get; }
        double ZeitBalkenBreite { get; }
        
        // Füge das OnStateChange-Event hinzu
        event Action OnStateChange;
        
        Task StartePruefungAsync(int pruefungId);
        void WaehleAntwort(int aufgabeId, int antwortId);
        void BeendePruefung();
        void ZurueckZuPruefungsliste();
    }
}