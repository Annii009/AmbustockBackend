using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbuStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly ServicioService _service;

        public ServicioController(ServicioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetAll()
        {
            try
            {
                var servicios = await _service.GetAllAsync();
                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDto>> GetById(int id)
        {
            try
            {
                var servicio = await _service.GetByIdAsync(id);
                return Ok(servicio);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("fecha-rango")]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetByFechaRango([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                var servicios = await _service.GetByFechaRangoAsync(fechaInicio, fechaFin);
                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("responsable/{idResponsable}")]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetByResponsableId(int idResponsable)
        {
            try
            {
                var servicios = await _service.GetByResponsableIdAsync(idResponsable);
                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServicioDto>> Create([FromBody] CreateServicioDto dto)
        {
            try
            {
                var servicio = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = servicio.IdServicio }, servicio);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServicioDto>> Update(int id, [FromBody] UpdateServicioDto dto)
        {
            try
            {
                var servicio = await _service.UpdateAsync(id, dto);
                return Ok(servicio);
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
