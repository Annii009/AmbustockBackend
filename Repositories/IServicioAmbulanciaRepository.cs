using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IServicioAmbulanciaRepository
    {
        Task<IEnumerable<ServicioAmbulancia>> GetAllAsync();
        Task<ServicioAmbulancia?> GetByIdAsync(int id);
        Task<ServicioAmbulancia> AddAsync(ServicioAmbulancia servicioAmbulancia);
        Task UpdateAsync(ServicioAmbulancia servicioAmbulancia);
        Task DeleteAsync(int id);
        Task<IEnumerable<ServicioAmbulancia>> GetByAmbulanciaIdAsync(int idAmbulancia);
        Task<IEnumerable<ServicioAmbulancia>> GetByServicioIdAsync(int idServicio);
    }
}