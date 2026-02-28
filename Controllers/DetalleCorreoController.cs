using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleCorreoController : ControllerBase
    {
        private readonly DetalleCorreoService _service;

        public DetalleCorreoController(DetalleCorreoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleCorreoDto>>> GetAll()
        {
                var detalles = await _service.GetAllAsync();
                return Ok(detalles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleCorreoDto>> GetById(int id)
        {
                var detalle = await _service.GetByIdAsync(id);
                return Ok(detalle);
        }

        [HttpGet("correo/{idCorreo}")]
        public async Task<ActionResult<IEnumerable<DetalleCorreoDto>>> GetByCorreoId(int idCorreo)
        {
                var detalles = await _service.GetByCorreoIdAsync(idCorreo);
                return Ok(detalles);
        }

        [HttpGet("material/{idMaterial}")]
        public async Task<ActionResult<IEnumerable<DetalleCorreoDto>>> GetByMaterialId(int idMaterial)
        {
                var detalles = await _service.GetByMaterialIdAsync(idMaterial);
                return Ok(detalles);
        }

        [HttpPost]
        public async Task<ActionResult<DetalleCorreoDto>> Create([FromBody] CreateDetalleCorreoDto dto)
        {
                var detalle = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = detalle.IdDetalleCorreo }, detalle);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DetalleCorreoDto>> Update(int id, [FromBody] UpdateDetalleCorreoDto dto)
        {
                var detalle = await _service.UpdateAsync(id, dto);
                return Ok(detalle);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
                await _service.DeleteAsync(id);
                return NoContent();
        }
    }
}
