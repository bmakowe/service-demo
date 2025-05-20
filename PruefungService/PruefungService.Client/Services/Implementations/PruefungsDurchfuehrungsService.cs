using System.Timers;
using Microsoft.JSInterop;
using PruefungService.Client.Models;
using PruefungService.Client.Services.Interfaces;

namespace PruefungService.Client.Services.Implementations
{
    public class PruefungsDurchfuehrungsService : IPruefungsDurchfuehrungsService, IDisposable
    {
        private readonly IPruefungDataService _pruefungDataService;
        private readonly IJSRuntime _jsRuntime;
        private System.Timers.Timer? _timer;
        private Dictionary<int, int> _benutzerAntworten = new();
        
        public PruefungViewModel? AktuellePruefung { get; private set; }
        public List<AufgabeViewModel>? AufgabenListe { get; private set; }
        public int VerbleibendeZeit { get; private set; }
        public bool PruefungBeendet { get; private set; }
        
        // Event für UI-Updates
        public event Action? OnChange;

        public PruefungsDurchfuehrungsService(
            IPruefungDataService pruefungDataService,
            IJSRuntime jsRuntime)
        {
            _pruefungDataService = pruefungDataService;
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> StartePruefungAsync(int pruefungId)
        {
            try
            {
                // Prüfungsdaten laden
                AktuellePruefung = await _pruefungDataService.GetPruefungByIdAsync(pruefungId);
                
                if (AktuellePruefung == null)
                    return false;
                
                // Aufgaben für diese Prüfung laden
                var aufgaben = await _pruefungDataService.GetAufgabenFuerPruefungAsync(pruefungId);
                AufgabenListe = aufgaben.ToList();
                
                // Prüfungsstatus initialisieren
                PruefungBeendet = false;
                _benutzerAntworten.Clear();
                
                // Timer initialisieren
                VerbleibendeZeit = AktuellePruefung.Zeitlimit * 60; // In Sekunden
                
                // UI über Änderungen informieren
                NotifyStateChanged();
                
                return true;
            }
            catch (Exception)
            {
                // In einer realen Anwendung würde hier Logging erfolgen
                return false;
            }
        }

        public void StartTimer()
        {
            // Timer erstellen (1-Sekunden-Intervall)
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (VerbleibendeZeit > 0)
            {
                VerbleibendeZeit--;
                NotifyStateChanged();
            }
            else
            {
                BeendePruefung();
            }
        }

        public void StopTimer()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }

        public void BeendePruefung()
        {
            PruefungBeendet = true;
            StopTimer();
            NotifyStateChanged();
        }

        public void WaehleAntwort(int aufgabeId, int antwortId)
        {
            if (!PruefungBeendet)
            {
                _benutzerAntworten[aufgabeId] = antwortId;
            }
        }

        public PruefungsErgebnisModel BerechnePruefungsErgebnis()
        {
            if (AktuellePruefung == null || AufgabenListe == null)
            {
                throw new InvalidOperationException("Keine aktive Prüfung vorhanden.");
            }
            
            int richtigBeantwortet = 0;
            
            foreach (var aufgabe in AufgabenListe)
            {
                // Prüfen, ob die Aufgabe beantwortet wurde
                if (_benutzerAntworten.TryGetValue(aufgabe.Id, out int antwortId))
                {
                    // Prüfen, ob die gewählte Antwort richtig ist
                    var gewaehlteAntwort = aufgabe.Antworten.FirstOrDefault(a => a.Id == antwortId);
                    if (gewaehlteAntwort != null && gewaehlteAntwort.IstRichtig)
                    {
                        richtigBeantwortet++;
                    }
                }
            }
            
            return new PruefungsErgebnisModel
            {
                PruefungId = AktuellePruefung.Id,
                PruefungsTitel = AktuellePruefung.Titel,
                AnzahlRichtigBeantwortet = richtigBeantwortet,
                AnzahlGesamtAufgaben = AufgabenListe.Count
            };
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void Dispose()
        {
            StopTimer();
        }
    }
}