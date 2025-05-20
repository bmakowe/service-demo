using AufgabenService.Client.Models;

namespace AufgabenService.Client.Services.Interfaces
{
    public interface IAufgabenDataService
    {
        Task<IEnumerable<AufgabeViewModel>> GetAlleAufgabenAsync();
        Task<AufgabeViewModel?> GetAufgabeByIdAsync(int id);
        Task<AufgabeViewModel> ErstelleAufgabeAsync(AufgabeErstellenModel aufgabe);
        Task<AufgabeViewModel?> AktualisiereAufgabeAsync(int id, AufgabeErstellenModel aufgabe);
        Task<bool> LoescheAufgabeAsync(int id);
    }
}