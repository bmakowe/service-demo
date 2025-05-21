using System.Collections.Generic;
using System.Threading.Tasks;
using PruefungService.Domain.Entities;

namespace PruefungService.Application.Interfaces
{
    public interface IPruefungRepository
    {
        Task<IEnumerable<Pruefung>> GetAllAsync();
        Task<Pruefung> GetByIdAsync(int id);
        Task<Pruefung> AddAsync(Pruefung pruefung);
        Task<Pruefung> UpdateAsync(Pruefung pruefung);
        Task<bool> DeleteAsync(int id);
    }
}