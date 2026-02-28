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
            var servicios = await _service.GetAllAsync();
            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDto>> GetById(int id)
        {
            var servicio = await _service.GetByIdAsync(id);
            return Ok(servicio);
        }

        [HttpGet("fecha-rango")]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetByFechaRango([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var servicios = await _service.GetByFechaRangoAsync(fechaInicio, fechaFin);
            return Ok(servicios);
        }

        [HttpGet("responsable/{idResponsable}")]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetByResponsableId(int idResponsable)
        {
            var servicios = await _service.GetByResponsableIdAsync(idResponsable);
            return Ok(servicios);
        }

        [HttpPost]
        public async Task<ActionResult<ServicioDto>> Create([FromBody] CreateServicioDto dto)
        {
            var servicio = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = servicio.IdServicio }, servicio);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServicioDto>> Update(int id, [FromBody] UpdateServicioDto dto)
        {
            var servicio = await _service.UpdateAsync(id, dto);
            return Ok(servicio);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
