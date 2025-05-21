using System.Collections.Generic;
using System.Threading.Tasks;
using AufgabenService.Application.DTOs;

namespace AufgabenService.Application.Interfaces
{
    public interface IAufgabenService
    {
        Task<IEnumerable<AufgabeDto>> GetAllAufgabenAsync();
        Task<AufgabeDto> GetAufgabeByIdAsync(int id);
        Task<AufgabeDto> CreateAufgabeAsync(AufgabeErstellenDto aufgabeDto);
        Task<AufgabeDto> UpdateAufgabeAsync(int id, AufgabeAktualisierenDto aufgabeDto);
        Task<bool> DeleteAufgabeAsync(int id);
    }
}