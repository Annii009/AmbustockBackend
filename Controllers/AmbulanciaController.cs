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
            try
            {
                var ambulancias = await _service.GetAllAsync();
                return Ok(ambulancias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AmbulanciaDto>> GetById(int id)
        {
            try
            {
                var ambulancia = await _service.GetByIdAsync(id);
                return Ok(ambulancia);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("matricula/{matricula}")]
        public async Task<ActionResult<AmbulanciaDto>> GetByMatricula(string matricula)
        {
            try
            {
                var ambulancia = await _service.GetByMatriculaAsync(matricula);
                return Ok(ambulancia);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<AmbulanciaDto>> Create([FromBody] CreateAmbulanciaDto dto)
        {
            try
            {
                var ambulancia = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = ambulancia.IdAmbulancia }, ambulancia);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AmbulanciaDto>> Update(int id, [FromBody] UpdateAmbulanciaDto dto)
        {
            try
            {
                var ambulancia = await _service.UpdateAsync(id, dto);
                return Ok(ambulancia);
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
