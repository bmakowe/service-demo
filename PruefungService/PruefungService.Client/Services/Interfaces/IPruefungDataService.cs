using PruefungService.Client.Models;

namespace PruefungService.Client.Services.Interfaces
{
    public interface IPruefungDataService
    {
        Task<List<PruefungViewModel>> GetAllePruefungenAsync();
        Task<PruefungViewModel?> GetPruefungByIdAsync(int id);
        Task<List<AufgabeViewModel>> GetAlleAufgabenAsync();
        Task<List<AufgabeViewModel>> GetAufgabenFuerPruefungAsync(int pruefungId);
        Task<PruefungViewModel?> CreatePruefungAsync(PruefungErstellenModel pruefungDto);
        Task<PruefungViewModel?> UpdatePruefungAufgabenAsync(int pruefungId, AufgabenZuweisenModel aufgabenDto);
        Task<bool> DeletePruefungAsync(int id);
    }
}