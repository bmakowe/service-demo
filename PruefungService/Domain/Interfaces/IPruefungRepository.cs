using PruefungService.Domain.Entities;

namespace PruefungService.Domain.Interfaces
{
    public interface IPruefungRepository
    {
        Task<IEnumerable<Pruefung>> GetAllePruefungenAsync();
        Task<Pruefung?> GetPruefungByIdAsync(int id);
        Task<Pruefung> ErstellePruefungAsync(Pruefung pruefung);
        Task<Pruefung?> AktualisierePruefungAsync(Pruefung pruefung);
        Task<bool> LoeschePruefungAsync(int id);
    }
}