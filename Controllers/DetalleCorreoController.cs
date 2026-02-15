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
            try
            {
                var detalles = await _service.GetAllAsync();
                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleCorreoDto>> GetById(int id)
        {
            try
            {
                var detalle = await _service.GetByIdAsync(id);
                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("correo/{idCorreo}")]
        public async Task<ActionResult<IEnumerable<DetalleCorreoDto>>> GetByCorreoId(int idCorreo)
        {
            try
            {
                var detalles = await _service.GetByCorreoIdAsync(idCorreo);
                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("material/{idMaterial}")]
        public async Task<ActionResult<IEnumerable<DetalleCorreoDto>>> GetByMaterialId(int idMaterial)
        {
            try
            {
                var detalles = await _service.GetByMaterialIdAsync(idMaterial);
                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<DetalleCorreoDto>> Create([FromBody] CreateDetalleCorreoDto dto)
        {
            try
            {
                var detalle = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = detalle.IdDetalleCorreo }, detalle);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DetalleCorreoDto>> Update(int id, [FromBody] UpdateDetalleCorreoDto dto)
        {
            try
            {
                var detalle = await _service.UpdateAsync(id, dto);
                return Ok(detalle);
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
