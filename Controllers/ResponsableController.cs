using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsableController : ControllerBase
    {
        private readonly ResponsableService _service;

        public ResponsableController(ResponsableService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsableDto>>> GetAll()
        {
            try
            {
                var responsables = await _service.GetAllAsync();
                return Ok(responsables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsableDto>> GetById(int id)
        {
            try
            {
                var responsable = await _service.GetByIdAsync(id);
                return Ok(responsable);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("servicio/{idServicio}")]
        public async Task<ActionResult<IEnumerable<ResponsableDto>>> GetByServicio(int idServicio)
        {
            try
            {
                var responsables = await _service.GetByServicioIdAsync(idServicio);
                return Ok(responsables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<ResponsableDto>>> GetByUsuario(int idUsuario)
        {
            try
            {
                var responsables = await _service.GetByUsuarioIdAsync(idUsuario);
                return Ok(responsables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponsableDto>> Create([FromBody] CreateResponsableDto dto)
        {
            try
            {
                var responsable = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = responsable.IdResponsable }, responsable);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponsableDto>> Update(int id, [FromBody] UpdateResponsableDto dto)
        {
            try
            {
                var responsable = await _service.UpdateAsync(id, dto);
                return Ok(responsable);
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
