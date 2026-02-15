using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IServicioRepository
    {
        Task<IEnumerable<Servicio>> GetAllAsync();
        Task<Servicio?> GetByIdAsync(int id);
        Task<Servicio> AddAsync(Servicio servicio);
        Task UpdateAsync(Servicio servicio);
        Task DeleteAsync(int id);
        Task<IEnumerable<Servicio>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<Servicio>> GetByResponsableIdAsync(int idResponsable);
    }
}