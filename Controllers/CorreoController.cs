using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CorreoController : ControllerBase
    {
        private readonly CorreoService _service;

        public CorreoController(CorreoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CorreoDto>>> GetAll()
        {
            try
            {
                var correos = await _service.GetAllAsync();
                return Ok(correos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CorreoDto>> GetById(int id)
        {
            try
            {
                var correo = await _service.GetByIdAsync(id);
                return Ok(correo);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<CorreoDto>>> GetByUsuarioId(int idUsuario)
        {
            try
            {
                var correos = await _service.GetByUsuarioIdAsync(idUsuario);
                return Ok(correos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("material/{idMaterial}")]
        public async Task<ActionResult<IEnumerable<CorreoDto>>> GetByMaterialId(int idMaterial)
        {
            try
            {
                var correos = await _service.GetByMaterialIdAsync(idMaterial);
                return Ok(correos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CorreoDto>> Create([FromBody] CreateCorreoDto dto)
        {
            try
            {
                var correo = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = correo.IdCorreo }, correo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CorreoDto>> Update(int id, [FromBody] UpdateCorreoDto dto)
        {
            try
            {
                var correo = await _service.UpdateAsync(id, dto);
                return Ok(correo);
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
