using AufgabenService.Client.Models;

namespace AufgabenService.Client.Services.Interfaces
{
    public interface IAufgabenDataService
    {
        Task<List<AufgabenViewModel>> GetAlleAufgabenAsync();
        Task<AufgabenViewModel?> GetAufgabeByIdAsync(int id);
        Task<AufgabenViewModel?> CreateAufgabeAsync(AufgabeErstellenModel aufgabeDto);
        Task<AufgabenViewModel?> UpdateAufgabeAsync(int id, AufgabeErstellenModel aufgabeDto);
        Task<bool> DeleteAufgabeAsync(int id);
    }
}