using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;
using AmbustockBackend.Service;

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
            var reposiciones = await _service.GetAllAsync();
            return Ok(reposiciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReposicionDto>> GetById(int id)
        {
            var reposicion = await _service.GetByIdAsync(id);
            return Ok(reposicion);
        }

        [HttpGet("correo/{idCorreo}")]
        public async Task<ActionResult<IEnumerable<ReposicionDto>>> GetByCorreoId(int idCorreo)
        {
            var reposiciones = await _service.GetByCorreoIdAsync(idCorreo);
            return Ok(reposiciones);
        }

        [HttpPost]
        public async Task<ActionResult<ReposicionDto>> Create([FromBody] CreateReposicionDto dto)
        {
            var reposicion = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reposicion.IdReposicion }, reposicion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReposicionDto>> Update(int id, [FromBody] UpdateReposicionDto dto)
        {
            var reposicion = await _service.UpdateAsync(id, dto);
            return Ok(reposicion);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
