using PruefungService.Application.DTOs;

namespace PruefungService.Application.Interfaces
{
    public interface IPruefungAppService
    {
        Task<IEnumerable<PruefungDto>> GetAllePruefungenAsync();
        Task<PruefungDto?> GetPruefungByIdAsync(int id);
        Task<IEnumerable<AufgabeDto>> GetAufgabenFuerPruefungAsync(int pruefungId);
        Task<IEnumerable<AufgabeDto>> GetAlleAufgabenAsync();
        Task<PruefungDto> ErstellePruefungAsync(PruefungErstellenDto pruefungDto);
        Task<PruefungDto?> AktualisierePruefungAsync(int id, PruefungAktualisierenDto pruefungDto);
        Task<PruefungDto?> WeiseAufgabenZuAsync(int id, AufgabenZuweisenDto aufgabenDto);
        Task<bool> LoeschePruefungAsync(int id);
    }
}