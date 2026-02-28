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
                var correos = await _service.GetAllAsync();
                return Ok(correos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CorreoDto>> GetById(int id)
        {
                var correo = await _service.GetByIdAsync(id);
                return Ok(correo);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<CorreoDto>>> GetByUsuarioId(int idUsuario)
        {
                var correos = await _service.GetByUsuarioIdAsync(idUsuario);
                return Ok(correos);
        }

        [HttpGet("material/{idMaterial}")]
        public async Task<ActionResult<IEnumerable<CorreoDto>>> GetByMaterialId(int idMaterial)
        {
                var correos = await _service.GetByMaterialIdAsync(idMaterial);
                return Ok(correos);
        }

        [HttpPost]
        public async Task<ActionResult<CorreoDto>> Create([FromBody] CreateCorreoDto dto)
        {
                var correo = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = correo.IdCorreo }, correo);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CorreoDto>> Update(int id, [FromBody] UpdateCorreoDto dto)
        {
                var correo = await _service.UpdateAsync(id, dto);
                return Ok(correo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
                await _service.DeleteAsync(id);
                return NoContent();
        }
    }
}
