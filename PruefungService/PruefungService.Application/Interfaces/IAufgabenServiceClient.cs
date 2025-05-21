using System.Collections.Generic;
using System.Threading.Tasks;
using PruefungService.Application.DTOs;

namespace PruefungService.Application.Interfaces
{
    public interface IAufgabenServiceClient
    {
        Task<IEnumerable<AufgabeDto>> GetAllAufgabenAsync();
        Task<AufgabeDto> GetAufgabeByIdAsync(int id);
        Task<IEnumerable<AufgabeDto>> GetAufgabenByIdsAsync(IEnumerable<int> ids);
    }
}