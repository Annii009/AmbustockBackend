using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbuStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CajonController : ControllerBase
    {
        private readonly CajonService _service;

        public CajonController(CajonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CajonDto>>> GetAll()
        {
            try
            {
                var cajones = await _service.GetAllAsync();
                return Ok(cajones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CajonDto>> GetById(int id)
        {
            try
            {
                var cajon = await _service.GetByIdAsync(id);
                return Ok(cajon);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("zona/{idZona}")]
        public async Task<ActionResult<IEnumerable<CajonDto>>> GetByZonaId(int idZona)
        {
            try
            {
                var cajones = await _service.GetByZonaIdAsync(idZona);
                return Ok(cajones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CajonDto>> Create([FromBody] CreateCajonDto dto)
        {
            try
            {
                var cajon = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = cajon.IdCajon }, cajon);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CajonDto>> Update(int id, [FromBody] UpdateCajonDto dto)
        {
            try
            {
                var cajon = await _service.UpdateAsync(id, dto);
                return Ok(cajon);
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
