using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;

namespace AmbustockBackend.Services
{
    public class RevisionService
    {
        private readonly IAmbulanciaRepository _ambulanciaRepository;
        private readonly IZonaRepository _zonaRepository;
        private readonly ICajonRepository _cajonRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IRevisionRepository _revisionRepository;

        public RevisionService(
            IAmbulanciaRepository ambulanciaRepository,
            IZonaRepository zonaRepository,
            ICajonRepository cajonRepository,
            IMaterialRepository materialRepository,
            IRevisionRepository revisionRepository)
        {
            _ambulanciaRepository = ambulanciaRepository;
            _zonaRepository = zonaRepository;
            _cajonRepository = cajonRepository;
            _materialRepository = materialRepository;
            _revisionRepository = revisionRepository;
        }

        public async Task<RevisionAmbulanciaDto> GetRevisionPorAmbulanciaAsync(int idAmbulancia)
        {
            var ambulancia = await _ambulanciaRepository.GetByIdAsync(idAmbulancia);
            if (ambulancia == null)
                throw new Exception($"Ambulancia con ID {idAmbulancia} no encontrada");

            var zonas = await _zonaRepository.GetByAmbulanciaIdAsync(idAmbulancia);
            
            var zonasDto = new List<ZonaRevisionDto>();

            foreach (var zona in zonas)
            {
                var cajones = await _cajonRepository.GetByZonaIdAsync(zona.IdZona);
                var materialesSinCajon = await _materialRepository.GetByZonaSinCajonAsync(zona.IdZona);
                
                var cajonesDto = new List<CajonRevisionDto>();
                
                foreach (var cajon in cajones)
                {
                    var materialesCajon = await _materialRepository.GetByCajonIdAsync(cajon.IdCajon);
                    
                    cajonesDto.Add(new CajonRevisionDto
                    {
                        IdCajon = cajon.IdCajon,
                        NombreCajon = cajon.NombreCajon,
                        Materiales = materialesCajon.Select(m => new MaterialRevisionDto
                        {
                            IdMaterial = m.IdMaterial,
                            NombreProducto = m.NombreProducto,
                            Cantidad = m.Cantidad,
                            Revisado = false
                        }).ToList()
                    });
                }
                
                zonasDto.Add(new ZonaRevisionDto
                {
                    IdZona = zona.IdZona,
                    NombreZona = zona.NombreZona,
                    Cajones = cajonesDto,
                    Materiales = materialesSinCajon.Select(m => new MaterialRevisionDto
                    {
                        IdMaterial = m.IdMaterial,
                        NombreProducto = m.NombreProducto,
                        Cantidad = m.Cantidad,
                        Revisado = false
                    }).ToList()
                });
            }

            return new RevisionAmbulanciaDto
            {
                IdAmbulancia = ambulancia.IdAmbulancia,
                NombreAmbulancia = ambulancia.Nombre,
                Matricula = ambulancia.Matricula,
                Zonas = zonasDto
            };
        }

        // NUEVO: Guardar revisión completada
        public async Task GuardarRevisionAsync(GuardarRevisionDto dto)
        {
            var ambulancia = await _ambulanciaRepository.GetByIdAsync(dto.IdAmbulancia);
            if (ambulancia == null)
                throw new Exception($"Ambulancia con ID {dto.IdAmbulancia} no encontrada");

            // Calcular totales
            int totalMateriales = 0;
            int materialesRevisados = 0;

            foreach (var zona in dto.Zonas)
            {
                if (zona.Materiales != null)
                {
                    totalMateriales += zona.Materiales.Count;
                    materialesRevisados += zona.Materiales.Count(m => m.CantidadRevisada == m.Cantidad);
                }

                if (zona.Cajones != null)
                {
                    foreach (var cajon in zona.Cajones)
                    {
                        if (cajon.Materiales != null)
                        {
                            totalMateriales += cajon.Materiales.Count;
                            materialesRevisados += cajon.Materiales.Count(m => m.CantidadRevisada == m.Cantidad);
                        }
                    }
                }
            }

            var revision = new Models.Revision
            {
                Id_ambulancia = dto.IdAmbulancia,
                Id_servicio = dto.IdServicio,
                Nombre_Responsable = dto.NombreResponsable,
                Fecha_Revision = dto.FechaRevision,
                Total_Materiales = totalMateriales,
                Materiales_Revisados = materialesRevisados,
                Estado = materialesRevisados == totalMateriales ? "completada" : "pendiente"
            };

            await _revisionRepository.AddAsync(revision);
        }

        // NUEVO: Obtener historial de revisiones
        public async Task<IEnumerable<RevisionHistorialDto>> GetHistorialAsync()
        {
            var revisiones = await _revisionRepository.GetAllAsync();

            var historialDto = new List<RevisionHistorialDto>();

            foreach (var revision in revisiones)
            {
                var ambulancia = await _ambulanciaRepository.GetByIdAsync(revision.Id_ambulancia);

                historialDto.Add(new RevisionHistorialDto
                {
                    IdRevision = revision.Id_revision,
                    NombreAmbulancia = ambulancia?.Nombre ?? "N/A",
                    Matricula = ambulancia?.Matricula ?? "N/A",
                    NombreResponsable = revision.Nombre_Responsable,
                    FechaRevision = revision.Fecha_Revision,
                    Estado = revision.Estado,
                    TotalMateriales = revision.Total_Materiales,
                    MaterialesRevisados = revision.Materiales_Revisados
                });
            }

            return historialDto.OrderByDescending(r => r.FechaRevision);
        }

        // NUEVO: Obtener detalle de revisión por ID
        public async Task<RevisionDetalleDto> GetRevisionByIdAsync(int id)
        {
            var revision = await _revisionRepository.GetByIdAsync(id);
            if (revision == null)
                return null;

            var ambulancia = await _ambulanciaRepository.GetByIdAsync(revision.Id_ambulancia);

            return new RevisionDetalleDto
            {
                IdRevision = revision.Id_revision,
                NombreAmbulancia = ambulancia?.Nombre ?? "N/A",
                Matricula = ambulancia?.Matricula ?? "N/A",
                NombreResponsable = revision.Nombre_Responsable,
                FechaRevision = revision.Fecha_Revision,
                Estado = revision.Estado,
                TotalMateriales = revision.Total_Materiales,
                MaterialesRevisados = revision.Materiales_Revisados,
                Zonas = new List<ZonaRevisionDto>()
            };
        }
    }
}
