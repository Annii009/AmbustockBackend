using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IReposicionRepository
    {
        Task<IEnumerable<Reposicion>> GetAllAsync();
        Task<Reposicion?> GetByIdAsync(int id);
        Task<Reposicion> AddAsync(Reposicion reposicion);
        Task UpdateAsync(Reposicion reposicion);
        Task DeleteAsync(int id);
        Task<IEnumerable<Reposicion>> GetByCorreoIdAsync(int idCorreo);
    }
}