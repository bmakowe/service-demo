using AufgabenService.Domain.Entities;
using AufgabenService.Domain.Interfaces;
using AufgabenService.Infrastructure.Persistence;

namespace AufgabenService.Infrastructure.Persistence.Repositories
{
    public class AufgabenRepository : IAufgabenRepository
    {
        private readonly InMemoryContext _context;

        public AufgabenRepository(InMemoryContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Aufgabe>> GetAlleAufgabenAsync()
        {
            return Task.FromResult(_context.Aufgaben.AsEnumerable());
        }

        public Task<Aufgabe?> GetAufgabeByIdAsync(int id)
        {
            var aufgabe = _context.Aufgaben.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(aufgabe);
        }

        public Task<Aufgabe> ErstelleAufgabeAsync(Aufgabe aufgabe)
        {
            // ID generieren (in einer echten DB würde dies automatisch erfolgen)
            var neueId = _context.Aufgaben.Count > 0 ? _context.Aufgaben.Max(a => a.Id) + 1 : 1;
            
            // Neue Aufgabe erstellen mit der korrekten ID
            var neueAufgabe = Aufgabe.Erstellen(
                neueId,
                aufgabe.Frage,
                aufgabe.Antworten.Select(a => (a.Text, a.IstRichtig)).ToList()
            );
            
            _context.Aufgaben.Add(neueAufgabe);
            
            return Task.FromResult(neueAufgabe);
        }

        public Task<Aufgabe?> AktualisiereAufgabeAsync(Aufgabe aufgabe)
        {
            var existierendeAufgabe = _context.Aufgaben.FirstOrDefault(a => a.Id == aufgabe.Id);
            if (existierendeAufgabe == null)
                return Task.FromResult<Aufgabe?>(null);
            
            // Bestehende Aufgabe entfernen
            _context.Aufgaben.Remove(existierendeAufgabe);
            
            // Aktualisierte Aufgabe hinzufügen
            _context.Aufgaben.Add(aufgabe);
            
            return Task.FromResult<Aufgabe?>(aufgabe);
        }

        public Task<bool> LoescheAufgabeAsync(int id)
        {
            var aufgabe = _context.Aufgaben.FirstOrDefault(a => a.Id == id);
            if (aufgabe == null)
                return Task.FromResult(false);
            
            _context.Aufgaben.Remove(aufgabe);
            return Task.FromResult(true);
        }
    }
}