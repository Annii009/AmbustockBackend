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
            var serviciosAmb = await _service.GetAllAsync();
            return Ok(serviciosAmb);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioAmbulanciaDto>> GetById(int id)
        {
            var servicioAmb = await _service.GetByIdAsync(id);
            return Ok(servicioAmb);
        }

        [HttpGet("ambulancia/{idAmbulancia}")]
        public async Task<ActionResult<IEnumerable<ServicioAmbulanciaDto>>> GetByAmbulanciaId(int idAmbulancia)
        {
            var serviciosAmb = await _service.GetByAmbulanciaIdAsync(idAmbulancia);
            return Ok(serviciosAmb);
        }

        [HttpGet("servicio/{idServicio}")]
        public async Task<ActionResult<IEnumerable<ServicioAmbulanciaDto>>> GetByServicioId(int idServicio)
        {
            var serviciosAmb = await _service.GetByServicioIdAsync(idServicio);
            return Ok(serviciosAmb);
        }

        [HttpPost]
        public async Task<ActionResult<ServicioAmbulanciaDto>> Create([FromBody] CreateServicioAmbulanciaDto dto)
        {
            var servicioAmb = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = servicioAmb.IdServicioAmbulancia }, servicioAmb);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServicioAmbulanciaDto>> Update(int id, [FromBody] UpdateServicioAmbulanciaDto dto)
        {
            var servicioAmb = await _service.UpdateAsync(id, dto);
            return Ok(servicioAmb);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
