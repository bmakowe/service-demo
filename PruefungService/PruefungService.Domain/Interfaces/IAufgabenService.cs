using PruefungService.Domain.ValueObjects;

namespace PruefungService.Domain.Interfaces
{
    // Interface für den externen AufgabenService
    public interface IAufgabenService
    {
        Task<IEnumerable<Aufgabe>> GetAufgabenAsync();
        Task<Aufgabe?> GetAufgabeByIdAsync(int id);
        Task<IEnumerable<Aufgabe>> GetAufgabenByIdsAsync(IEnumerable<int> ids);
    }
}