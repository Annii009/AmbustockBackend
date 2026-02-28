using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmbulanciaController : ControllerBase
    {
        private readonly AmbulanciaService _service;

        public AmbulanciaController(AmbulanciaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmbulanciaDto>>> GetAll()
        {
                var ambulancias = await _service.GetAllAsync();
                return Ok(ambulancias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AmbulanciaDto>> GetById(int id)
        {
            var ambulancia = await _service.GetByIdAsync(id);
            return Ok(ambulancia);
        }

        [HttpGet("matricula/{matricula}")]
        public async Task<ActionResult<AmbulanciaDto>> GetByMatricula(string matricula)
        {
            var ambulancia = await _service.GetByMatriculaAsync(matricula);
            return Ok(ambulancia);

        }

        [HttpPost]
        public async Task<ActionResult<AmbulanciaDto>> Create([FromBody] CreateAmbulanciaDto dto)
        {
            var ambulancia = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = ambulancia.IdAmbulancia }, ambulancia);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AmbulanciaDto>> Update(int id, [FromBody] UpdateAmbulanciaDto dto)
        {
            var ambulancia = await _service.UpdateAsync(id, dto);
            return Ok(ambulancia);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();

        }
    }
}
