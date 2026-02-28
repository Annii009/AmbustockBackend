using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevisionController : ControllerBase
    {
        private readonly RevisionService _service;

        public RevisionController(RevisionService service)
        {
            _service = service;
        }

        [HttpGet("ambulancia/{idAmbulancia}")]
        public async Task<ActionResult<RevisionAmbulanciaDto>> GetRevisionPorAmbulancia(int idAmbulancia)
        {
                var revision = await _service.GetRevisionPorAmbulanciaAsync(idAmbulancia);
                return Ok(revision);
        }

        [HttpPost]
        public async Task<ActionResult> GuardarRevision([FromBody] GuardarRevisionDto dto)
        {
                await _service.GuardarRevisionAsync(dto);
                return Ok(new { message = "Revisión guardada exitosamente" });
        }

        [HttpGet("historial")]
        public async Task<ActionResult<IEnumerable<RevisionHistorialDto>>> GetHistorial()
        {
                var historial = await _service.GetHistorialAsync();
                return Ok(historial);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RevisionDetalleDto>> GetRevisionById(int id)
        {
                var revision = await _service.GetRevisionByIdAsync(id);
                if (revision == null)
                    return NotFound(new { message = "Revisión no encontrada" });
                
                return Ok(revision);
        }
    }
}
