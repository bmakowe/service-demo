using PruefungService.Domain.Entities;
using PruefungService.Domain.Interfaces;
using PruefungService.Infrastructure.Persistence;

namespace PruefungService.Infrastructure.Persistence.Repositories
{
    public class PruefungRepository : IPruefungRepository
    {
        private readonly InMemoryContext _context;

        public PruefungRepository(InMemoryContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Pruefung>> GetAllePruefungenAsync()
        {
            return Task.FromResult(_context.Pruefungen.AsEnumerable());
        }

        public Task<Pruefung?> GetPruefungByIdAsync(int id)
        {
            var pruefung = _context.Pruefungen.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(pruefung);
        }

        public Task<Pruefung> ErstellePruefungAsync(Pruefung pruefung)
        {
            // ID generieren (in einer echten DB würde dies automatisch erfolgen)
            var neueId = _context.Pruefungen.Count > 0 ? _context.Pruefungen.Max(p => p.Id) + 1 : 1;
            
            // Neue Prüfung erstellen mit der korrekten ID
            var neuePruefung = Pruefung.Erstellen(
                neueId,
                pruefung.Titel,
                pruefung.Datum,
                pruefung.Zeitlimit,
                pruefung.AufgabenIds
            );
            
            _context.Pruefungen.Add(neuePruefung);
            
            return Task.FromResult(neuePruefung);
        }

        public Task<Pruefung?> AktualisierePruefungAsync(Pruefung pruefung)
        {
            var existierendePruefung = _context.Pruefungen.FirstOrDefault(p => p.Id == pruefung.Id);
            if (existierendePruefung == null)
                return Task.FromResult<Pruefung?>(null);
            
            // Bestehende Prüfung entfernen
            _context.Pruefungen.Remove(existierendePruefung);
            
            // Aktualisierte Prüfung hinzufügen
            _context.Pruefungen.Add(pruefung);
            
            return Task.FromResult<Pruefung?>(pruefung);
        }

        public Task<bool> LoeschePruefungAsync(int id)
        {
            var pruefung = _context.Pruefungen.FirstOrDefault(p => p.Id == id);
            if (pruefung == null)
                return Task.FromResult(false);
            
            _context.Pruefungen.Remove(pruefung);
            return Task.FromResult(true);
        }
    }
}