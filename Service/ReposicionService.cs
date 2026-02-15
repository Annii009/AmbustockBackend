using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;
using AmbustockBackend.Models;
using AmbustockBackend.Service;

namespace AmbustockBackend.Service
{
    public class ReposicionService
    {
        private readonly IReposicionRepository _repository;
        private readonly IEmailService _emailService;
        private readonly ILogger<ReposicionService> _logger;

        public ReposicionService(
            IReposicionRepository repository,
            IEmailService emailService,
            ILogger<ReposicionService> logger)
        {
            _repository = repository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IEnumerable<ReposicionDto>> GetAllAsync()
        {
            var reposiciones = await _repository.GetAllAsync();
            return reposiciones.Select(r => new ReposicionDto
            {
                IdReposicion = r.IdReposicion,
                IdCorreo = r.IdCorreo,
                NombreMaterial = r.NombreMaterial,
                Cantidad = r.Cantidad,
                Comentarios = r.Comentarios,
                FotoEvidenciaBase64 = r.FotoEvidencia != null ? Convert.ToBase64String(r.FotoEvidencia) : null
            });
        }

        public async Task<ReposicionDto> GetByIdAsync(int id)
        {
            var reposicion = await _repository.GetByIdAsync(id);
            if (reposicion == null)
                throw new Exception($"Reposición con ID {id} no encontrada");

            return new ReposicionDto
            {
                IdReposicion = reposicion.IdReposicion,
                IdCorreo = reposicion.IdCorreo,
                NombreMaterial = reposicion.NombreMaterial,
                Cantidad = reposicion.Cantidad,
                Comentarios = reposicion.Comentarios,
                FotoEvidenciaBase64 = reposicion.FotoEvidencia != null ? Convert.ToBase64String(reposicion.FotoEvidencia) : null
            };
        }

        public async Task<IEnumerable<ReposicionDto>> GetByCorreoIdAsync(int idCorreo)
        {
            var reposiciones = await _repository.GetByCorreoIdAsync(idCorreo);
            return reposiciones.Select(r => new ReposicionDto
            {
                IdReposicion = r.IdReposicion,
                IdCorreo = r.IdCorreo,
                NombreMaterial = r.NombreMaterial,
                Cantidad = r.Cantidad,
                Comentarios = r.Comentarios,
                FotoEvidenciaBase64 = r.FotoEvidencia != null ? Convert.ToBase64String(r.FotoEvidencia) : null
            });
        }

        public async Task<ReposicionDto> CreateAsync(CreateReposicionDto dto)
        {
            var reposicion = new Reposicion
            {
                IdCorreo = dto.IdCorreo,
                NombreMaterial = dto.NombreMaterial,
                Cantidad = dto.Cantidad,
                Comentarios = dto.Comentarios,
                FotoEvidencia = !string.IsNullOrEmpty(dto.FotoEvidenciaBase64) 
                    ? Convert.FromBase64String(dto.FotoEvidenciaBase64) 
                    : null
            };

            var created = await _repository.AddAsync(reposicion);

            // 👇 NUEVO: Enviar email al admin
            try
            {
                await _emailService.EnviarCorreoReposicionCompletadaAsync(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email después de crear reposición");
            }

            return new ReposicionDto
            {
                IdReposicion = created.IdReposicion,
                IdCorreo = created.IdCorreo,
                NombreMaterial = created.NombreMaterial,
                Cantidad = created.Cantidad,
                Comentarios = created.Comentarios,
                FotoEvidenciaBase64 = created.FotoEvidencia != null ? Convert.ToBase64String(created.FotoEvidencia) : null
            };
        }

        public async Task<ReposicionDto> UpdateAsync(int id, UpdateReposicionDto dto)
        {
            var reposicion = await _repository.GetByIdAsync(id);
            if (reposicion == null)
                throw new Exception($"Reposición con ID {id} no encontrada");

            reposicion.IdCorreo = dto.IdCorreo;
            reposicion.NombreMaterial = dto.NombreMaterial;
            reposicion.Cantidad = dto.Cantidad;
            reposicion.Comentarios = dto.Comentarios;
            reposicion.FotoEvidencia = !string.IsNullOrEmpty(dto.FotoEvidenciaBase64) 
                ? Convert.FromBase64String(dto.FotoEvidenciaBase64) 
                : null;

            await _repository.UpdateAsync(reposicion);

            return new ReposicionDto
            {
                IdReposicion = reposicion.IdReposicion,
                IdCorreo = reposicion.IdCorreo,
                NombreMaterial = reposicion.NombreMaterial,
                Cantidad = reposicion.Cantidad,
                Comentarios = reposicion.Comentarios,
                FotoEvidenciaBase64 = reposicion.FotoEvidencia != null ? Convert.ToBase64String(reposicion.FotoEvidencia) : null
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
