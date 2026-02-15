using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IZonaRepository
    {
        Task<IEnumerable<Zonas>> GetAllAsync();
        Task<Zonas?> GetByIdAsync(int id);
        Task<Zonas> AddAsync(Zonas zona);
        Task UpdateAsync(Zonas zona);
        Task DeleteAsync(int id);
        Task<IEnumerable<Zonas>> GetByAmbulanciaIdAsync(int idAmbulancia);
    }
}
