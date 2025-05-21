using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AufgabenService.Domain.Entities;
using AufgabenService.Application.Interfaces;

namespace AufgabenService.Infrastructure.Persistence.Repositories
{
    public class AufgabenRepository : IAufgabenRepository
    {
        private readonly InMemoryContext _context;
        private int _nextId;

        public AufgabenRepository(InMemoryContext context)
        {
            _context = context;
            _nextId = _context.Aufgaben.Count > 0 ? _context.Aufgaben.Max(a => a.Id) + 1 : 1;
        }

        public Task<IEnumerable<Aufgabe>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Aufgabe>>(_context.Aufgaben);
        }

        public Task<Aufgabe> GetByIdAsync(int id)
        {
            var aufgabe = _context.Aufgaben.FirstOrDefault(a => a.Id == id);
            if (aufgabe == null)
            {
                throw new KeyNotFoundException($"Aufgabe mit ID {id} wurde nicht gefunden.");
            }
            return Task.FromResult(aufgabe);
        }

        public Task<Aufgabe> AddAsync(Aufgabe aufgabe)
        {
            aufgabe.SetId(_nextId++);
            _context.Aufgaben.Add(aufgabe);
            return Task.FromResult(aufgabe);
        }

        public Task<Aufgabe> UpdateAsync(Aufgabe aufgabe)
        {
            var existingIndex = _context.Aufgaben.FindIndex(a => a.Id == aufgabe.Id);
            if (existingIndex >= 0)
            {
                _context.Aufgaben[existingIndex] = aufgabe;
                return Task.FromResult(aufgabe);
            }
            
            throw new KeyNotFoundException($"Aufgabe mit ID {aufgabe.Id} wurde nicht gefunden.");
        }

        public Task<bool> DeleteAsync(int id)
        {
            var aufgabe = _context.Aufgaben.FirstOrDefault(a => a.Id == id);
            if (aufgabe != null)
            {
                _context.Aufgaben.Remove(aufgabe);
                return Task.FromResult(true);
            }
            
            return Task.FromResult(false);
        }
    }
}