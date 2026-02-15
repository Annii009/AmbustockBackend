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
            try
            {
                var revision = await _service.GetRevisionPorAmbulanciaAsync(idAmbulancia);
                return Ok(revision);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> GuardarRevision([FromBody] GuardarRevisionDto dto)
        {
            try
            {
                await _service.GuardarRevisionAsync(dto);
                return Ok(new { message = "Revisión guardada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("historial")]
        public async Task<ActionResult<IEnumerable<RevisionHistorialDto>>> GetHistorial()
        {
            try
            {
                var historial = await _service.GetHistorialAsync();
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RevisionDetalleDto>> GetRevisionById(int id)
        {
            try
            {
                var revision = await _service.GetRevisionByIdAsync(id);
                if (revision == null)
                    return NotFound(new { message = "Revisión no encontrada" });
                
                return Ok(revision);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
