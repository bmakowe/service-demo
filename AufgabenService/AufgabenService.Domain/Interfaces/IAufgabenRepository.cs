using AufgabenService.Domain.Entities;

namespace AufgabenService.Domain.Interfaces
{
    public interface IAufgabenRepository
    {
        Task<IEnumerable<Aufgabe>> GetAlleAufgabenAsync();
        Task<Aufgabe?> GetAufgabeByIdAsync(int id);
        Task<Aufgabe> ErstelleAufgabeAsync(Aufgabe aufgabe);
        Task<Aufgabe?> AktualisiereAufgabeAsync(Aufgabe aufgabe);
        Task<bool> LoescheAufgabeAsync(int id);
    }
}