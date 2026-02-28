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
                var cajones = await _service.GetAllAsync();
                return Ok(cajones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CajonDto>> GetById(int id)
        {
                var cajon = await _service.GetByIdAsync(id);
                return Ok(cajon);
        }

        [HttpGet("zona/{idZona}")]
        public async Task<ActionResult<IEnumerable<CajonDto>>> GetByZonaId(int idZona)
        {
                var cajones = await _service.GetByZonaIdAsync(idZona);
                return Ok(cajones);
        }

        [HttpPost]
        public async Task<ActionResult<CajonDto>> Create([FromBody] CreateCajonDto dto)
        {
                var cajon = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = cajon.IdCajon }, cajon);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CajonDto>> Update(int id, [FromBody] UpdateCajonDto dto)
        {
                var cajon = await _service.UpdateAsync(id, dto);
                return Ok(cajon);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
                await _service.DeleteAsync(id);
                return NoContent();
        }
    }
}
