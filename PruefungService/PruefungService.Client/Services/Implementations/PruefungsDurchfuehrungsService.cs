using PruefungService.Client.Models;
using PruefungService.Client.Services.Interfaces;

namespace PruefungService.Client.Services.Implementations
{
    public class PruefungsDurchfuehrungsService : IPruefungsDurchfuehrungsService, IDisposable
    {
        private readonly IPruefungDataService _pruefungDataService;
        private System.Threading.Timer? _timer;

        public PruefungViewModel? AktuellePruefung { get; private set; }
        public List<AufgabeViewModel>? AktuellePruefungsaufgaben { get; private set; }
        public bool PruefungBeendet { get; private set; }
        public int VerbleibendeZeit { get; private set; }
        public double ZeitBalkenBreite { get; private set; }

        public event Action? OnStateChange;

        public PruefungsDurchfuehrungsService(IPruefungDataService pruefungDataService)
        {
            _pruefungDataService = pruefungDataService;
        }

        public async Task StartePruefungAsync(int pruefungId)
        {
            AktuellePruefung = await _pruefungDataService.GetPruefungByIdAsync(pruefungId);
            var aufgaben = await _pruefungDataService.GetAufgabenForPruefungAsync(pruefungId);
            AktuellePruefungsaufgaben = aufgaben.ToList();
            
            PruefungBeendet = false;
            
            if (AktuellePruefung != null)
            {
                VerbleibendeZeit = AktuellePruefung.Zeitlimit * 60;
                ZeitBalkenBreite = 100;
                
                _timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);
            }
            
            NotifyStateChanged();
        }

        public void WaehleAntwort(int aufgabeId, int antwortId)
        {
            // Hier könnte die Antwort gespeichert werden
            NotifyStateChanged();
        }

        public void BeendePruefung()
        {
            PruefungBeendet = true;
            
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer?.Dispose();
            _timer = null;
            
            NotifyStateChanged();
        }

        public void ZurueckZuPruefungsliste()
        {
            AktuellePruefung = null;
            AktuellePruefungsaufgaben = null;
            PruefungBeendet = false;
            
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer?.Dispose();
            _timer = null;
            
            NotifyStateChanged();
        }

        private void TimerCallback(object? state)
        {
            if (VerbleibendeZeit > 0)
            {
                VerbleibendeZeit--;
                
                if (AktuellePruefung != null)
                {
                    ZeitBalkenBreite = (double)VerbleibendeZeit / (AktuellePruefung.Zeitlimit * 60) * 100;
                }
                
                NotifyStateChanged();
            }
            else
            {
                BeendePruefung();
            }
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}