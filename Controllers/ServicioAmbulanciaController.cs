using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioAmbulanciaController : ControllerBase
    {
        private readonly ServicioAmbulanciaService _service;

        public ServicioAmbulanciaController(ServicioAmbulanciaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioAmbulanciaDto>>> GetAll()
        {
            try
            {
                var serviciosAmb = await _service.GetAllAsync();
                return Ok(serviciosAmb);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioAmbulanciaDto>> GetById(int id)
        {
            try
            {
                var servicioAmb = await _service.GetByIdAsync(id);
                return Ok(servicioAmb);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("ambulancia/{idAmbulancia}")]
        public async Task<ActionResult<IEnumerable<ServicioAmbulanciaDto>>> GetByAmbulanciaId(int idAmbulancia)
        {
            try
            {
                var serviciosAmb = await _service.GetByAmbulanciaIdAsync(idAmbulancia);
                return Ok(serviciosAmb);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("servicio/{idServicio}")]
        public async Task<ActionResult<IEnumerable<ServicioAmbulanciaDto>>> GetByServicioId(int idServicio)
        {
            try
            {
                var serviciosAmb = await _service.GetByServicioIdAsync(idServicio);
                return Ok(serviciosAmb);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServicioAmbulanciaDto>> Create([FromBody] CreateServicioAmbulanciaDto dto)
        {
            try
            {
                var servicioAmb = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = servicioAmb.IdServicioAmbulancia }, servicioAmb);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServicioAmbulanciaDto>> Update(int id, [FromBody] UpdateServicioAmbulanciaDto dto)
        {
            try
            {
                var servicioAmb = await _service.UpdateAsync(id, dto);
                return Ok(servicioAmb);
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
