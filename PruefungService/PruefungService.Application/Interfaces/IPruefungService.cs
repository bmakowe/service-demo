using System.Collections.Generic;
using System.Threading.Tasks;
using PruefungService.Application.DTOs;

namespace PruefungService.Application.Interfaces
{
    public interface IPruefungService
    {
        Task<IEnumerable<PruefungDto>> GetAllPruefungenAsync();
        Task<PruefungDto> GetPruefungByIdAsync(int id);
        Task<IEnumerable<AufgabeDto>> GetAufgabenForPruefungAsync(int pruefungId);
        Task<PruefungDto> CreatePruefungAsync(PruefungErstellenDto pruefungDto);
        Task<PruefungDto> UpdatePruefungAsync(int id, PruefungAktualisierenDto pruefungDto);
        Task<PruefungDto> UpdatePruefungAufgabenAsync(int id, AufgabenZuweisenDto aufgabenDto);
        Task<bool> DeletePruefungAsync(int id);
    }
}