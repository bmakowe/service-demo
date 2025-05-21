using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruefungService.Domain.Entities;
using PruefungService.Application.Interfaces;

namespace PruefungService.Infrastructure.Persistence.Repositories
{
    public class PruefungRepository : IPruefungRepository
    {
        private readonly InMemoryContext _context;
        private int _nextId;

        public PruefungRepository(InMemoryContext context)
        {
            _context = context;
            _nextId = _context.Pruefungen.Count > 0 ? _context.Pruefungen.Max(p => p.Id) + 1 : 1;
        }

        public Task<IEnumerable<Pruefung>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Pruefung>>(_context.Pruefungen);
        }

        public Task<Pruefung> GetByIdAsync(int id)
        {
            var pruefung = _context.Pruefungen.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(pruefung);
        }

        public Task<Pruefung> AddAsync(Pruefung pruefung)
        {
            pruefung.SetId(_nextId++);
            _context.Pruefungen.Add(pruefung);
            return Task.FromResult(pruefung);
        }

        public Task<Pruefung> UpdateAsync(Pruefung pruefung)
        {
            var existingIndex = _context.Pruefungen.FindIndex(p => p.Id == pruefung.Id);
            if (existingIndex >= 0)
            {
                _context.Pruefungen[existingIndex] = pruefung;
                return Task.FromResult(pruefung);
            }
            
            return Task.FromResult<Pruefung>(null);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var pruefung = _context.Pruefungen.FirstOrDefault(p => p.Id == id);
            if (pruefung != null)
            {
                _context.Pruefungen.Remove(pruefung);
                return Task.FromResult(true);
            }
            
            return Task.FromResult(false);
        }
    }
}