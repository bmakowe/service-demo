using PruefungService.Client.Models;

namespace PruefungService.Client.Services.Interfaces
{
    public interface IPruefungDataService
    {
        Task<IEnumerable<PruefungViewModel>> GetAllePruefungenAsync();
        Task<PruefungViewModel?> GetPruefungByIdAsync(int id);
        Task<IEnumerable<AufgabeViewModel>> GetAufgabenFuerPruefungAsync(int pruefungId);
        Task<IEnumerable<AufgabeViewModel>> GetAlleAufgabenAsync();
        Task<PruefungViewModel> ErstellePruefungAsync(PruefungErstellenModel pruefung);
        Task<PruefungViewModel?> AktualisierePruefungAsync(int id, PruefungErstellenModel pruefung);
        Task<PruefungViewModel?> WeiseAufgabenZuAsync(int id, AufgabenZuweisenModel aufgaben);
        Task<bool> LoeschePruefungAsync(int id);
    }
}