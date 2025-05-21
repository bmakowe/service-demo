using AufgabenService.Application.DTOs;

namespace AufgabenService.Application.Interfaces
{
    public interface IAufgabenService
    {
        Task<IEnumerable<AufgabeDto>> GetAlleAufgabenAsync();
        Task<AufgabeDto?> GetAufgabeByIdAsync(int id);
        Task<AufgabeDto> ErstelleAufgabeAsync(AufgabeErstellenDto aufgabeDto);
        Task<AufgabeDto?> AktualisiereAufgabeAsync(int id, AufgabeAktualisierenDto aufgabeDto);
        Task<bool> LoescheAufgabeAsync(int id);
    }
}