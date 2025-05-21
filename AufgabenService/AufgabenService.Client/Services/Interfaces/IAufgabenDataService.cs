using System.Collections.Generic;
using System.Threading.Tasks;
using AufgabenService.Client.Models;

namespace AufgabenService.Client.Services.Interfaces
{
    public interface IAufgabenDataService
    {
        Task<IEnumerable<AufgabeViewModel>> GetAllAufgabenAsync();
        Task<AufgabeViewModel> GetAufgabeByIdAsync(int id);
        Task<AufgabeViewModel> CreateAufgabeAsync(AufgabeErstellenModel aufgabe);
        Task<AufgabeViewModel> UpdateAufgabeAsync(int id, AufgabeErstellenModel aufgabe);
        Task<bool> DeleteAufgabeAsync(int id);
    }
}