using AufgabenService.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AufgabenService.Application.Interfaces
{
    public interface IAufgabenService
    {
        Task<List<AufgabeDto>> GetAlleAufgabenAsync();
        Task<AufgabeDto?> GetAufgabeByIdAsync(int id);
        Task<AufgabeDto> ErstelleAufgabeAsync(AufgabeErstellenDto aufgabeDto);
        Task<AufgabeDto?> AktualisiereAufgabeAsync(int id, AufgabeAktualisierenDto aufgabeDto);
        Task<bool> LoescheAufgabeAsync(int id);
    }
}