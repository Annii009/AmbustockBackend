using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IResponsableRepository
    {
        Task<IEnumerable<Responsable>> GetAllAsync();
        Task<Responsable?> GetByIdAsync(int id);
        Task<Responsable> AddAsync(Responsable responsable);
        Task UpdateAsync(Responsable responsable);
        Task DeleteAsync(int id);
        Task<IEnumerable<Responsable>> GetByServicioIdAsync(int idServicio);
        Task<IEnumerable<Responsable>> GetByUsuarioIdAsync(int idUsuario);
    }
}