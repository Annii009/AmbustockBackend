using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface ICajonRepository
    {
        Task<IEnumerable<Cajones>> GetAllAsync();
        Task<Cajones?> GetByIdAsync(int id);
        Task<Cajones> AddAsync(Cajones cajon);
        Task UpdateAsync(Cajones cajon);
        Task DeleteAsync(int id);
        Task<IEnumerable<Cajones>> GetByZonaIdAsync(int idZona);
    }
}