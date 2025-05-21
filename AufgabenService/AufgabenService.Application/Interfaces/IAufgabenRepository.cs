using AufgabenService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AufgabenService.Application.Interfaces
{
    public interface IAufgabenRepository
    {
        Task<List<Aufgabe>> GetAlleAufgabenAsync();
        Task<Aufgabe?> GetAufgabeByIdAsync(int id);
        Task<Aufgabe> CreateAufgabeAsync(Aufgabe aufgabe);
        Task<Aufgabe?> UpdateAufgabeAsync(Aufgabe aufgabe);
        Task<bool> DeleteAufgabeAsync(int id);
    }
}