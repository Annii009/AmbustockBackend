using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReposicionController : ControllerBase
    {
        private readonly ReposicionService _service;

        public ReposicionController(ReposicionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReposicionDto>>> GetAll()
        {
            try
            {
                var reposiciones = await _service.GetAllAsync();
                return Ok(reposiciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReposicionDto>> GetById(int id)
        {
            try
            {
                var reposicion = await _service.GetByIdAsync(id);
                return Ok(reposicion);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("correo/{idCorreo}")]
        public async Task<ActionResult<IEnumerable<ReposicionDto>>> GetByCorreoId(int idCorreo)
        {
            try
            {
                var reposiciones = await _service.GetByCorreoIdAsync(idCorreo);
                return Ok(reposiciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReposicionDto>> Create([FromBody] CreateReposicionDto dto)
        {
            try
            {
                var reposicion = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = reposicion.IdReposicion }, reposicion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReposicionDto>> Update(int id, [FromBody] UpdateReposicionDto dto)
        {
            try
            {
                var reposicion = await _service.UpdateAsync(id, dto);
                return Ok(reposicion);
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
