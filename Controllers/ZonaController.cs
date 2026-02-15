using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZonaController : ControllerBase
    {
        private readonly ZonaService _service;

        public ZonaController(ZonaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZonaDto>>> GetAll()
        {
            try
            {
                var zonas = await _service.GetAllAsync();
                return Ok(zonas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ZonaDto>> GetById(int id)
        {
            try
            {
                var zona = await _service.GetByIdAsync(id);
                return Ok(zona);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("ambulancia/{idAmbulancia}")]
        public async Task<ActionResult<IEnumerable<ZonaDto>>> GetByAmbulanciaId(int idAmbulancia)
        {
            try
            {
                var zonas = await _service.GetByAmbulanciaIdAsync(idAmbulancia);
                return Ok(zonas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ZonaDto>> Create([FromBody] CreateZonaDto dto)
        {
            try
            {
                var zona = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = zona.IdZona }, zona);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ZonaDto>> Update(int id, [FromBody] UpdateZonaDto dto)
        {
            try
            {
                var zona = await _service.UpdateAsync(id, dto);
                return Ok(zona);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
