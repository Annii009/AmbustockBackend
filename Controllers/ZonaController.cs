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
            var zonas = await _service.GetAllAsync();
            return Ok(zonas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ZonaDto>> GetById(int id)
        {
            var zona = await _service.GetByIdAsync(id);
            return Ok(zona);
        }

        [HttpGet("ambulancia/{idAmbulancia}")]
        public async Task<ActionResult<IEnumerable<ZonaDto>>> GetByAmbulanciaId(int idAmbulancia)
        {
            var zonas = await _service.GetByAmbulanciaIdAsync(idAmbulancia);
            return Ok(zonas);
        }

        [HttpPost]
        public async Task<ActionResult<ZonaDto>> Create([FromBody] CreateZonaDto dto)
        {
            var zona = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = zona.IdZona }, zona);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ZonaDto>> Update(int id, [FromBody] UpdateZonaDto dto)
        {
            var zona = await _service.UpdateAsync(id, dto);
            return Ok(zona);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
