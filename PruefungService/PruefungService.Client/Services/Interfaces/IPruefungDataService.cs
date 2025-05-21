using System.Collections.Generic;
using System.Threading.Tasks;
using PruefungService.Client.Models;

namespace PruefungService.Client.Services.Interfaces
{
    public interface IPruefungDataService
    {
        Task<IEnumerable<PruefungViewModel>> GetAllPruefungenAsync();
        Task<PruefungViewModel?> GetPruefungByIdAsync(int id);
        Task<IEnumerable<AufgabeViewModel>> GetAufgabenForPruefungAsync(int pruefungId);
        Task<IEnumerable<AufgabeViewModel>> GetAllAufgabenAsync();
        Task<PruefungViewModel?> CreatePruefungAsync(PruefungErstellenModel pruefung);
        Task<PruefungViewModel?> UpdatePruefungAsync(int id, PruefungAktualisierenModel pruefung);
        Task<PruefungViewModel?> UpdatePruefungAufgabenAsync(int id, AufgabenZuweisenModel aufgaben);
        Task<bool> DeletePruefungAsync(int id);
    }
}