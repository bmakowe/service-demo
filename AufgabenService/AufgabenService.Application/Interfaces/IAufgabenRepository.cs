using System.Collections.Generic;
using System.Threading.Tasks;
using AufgabenService.Domain.Entities;

namespace AufgabenService.Application.Interfaces
{
    public interface IAufgabenRepository
    {
        Task<IEnumerable<Aufgabe>> GetAllAsync();
        Task<Aufgabe> GetByIdAsync(int id);
        Task<Aufgabe> AddAsync(Aufgabe aufgabe);
        Task<Aufgabe> UpdateAsync(Aufgabe aufgabe);
        Task<bool> DeleteAsync(int id);
    }
}