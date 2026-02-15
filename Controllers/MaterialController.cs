using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;

namespace AmbuStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private readonly MaterialService _service;

        public MaterialController(MaterialService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetAll()
        {
            try
            {
                var materiales = await _service.GetAllAsync();
                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialDto>> GetById(int id)
        {
            try
            {
                var material = await _service.GetByIdAsync(id);
                return Ok(material);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("zona/{idZona}")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetByZonaId(int idZona)
        {
            try
            {
                var materiales = await _service.GetByZonaIdAsync(idZona);
                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("cajon/{idCajon}")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetByCajonId(int idCajon)
        {
            try
            {
                var materiales = await _service.GetByCajonIdAsync(idCajon);
                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("stock-bajo/{cantidadMinima}")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetByCantidadBaja(int cantidadMinima)
        {
            try
            {
                var materiales = await _service.GetByCantidadBajaAsync(cantidadMinima);
                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDto>> Create([FromBody] CreateMaterialDto dto)
        {
            try
            {
                var material = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = material.IdMaterial }, material);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MaterialDto>> Update(int id, [FromBody] UpdateMaterialDto dto)
        {
            try
            {
                var material = await _service.UpdateAsync(id, dto);
                return Ok(material);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/cantidad")]
        public async Task<ActionResult> UpdateCantidad(int id, [FromBody] UpdateCantidadMaterialDto dto)
        {
            try
            {
                await _service.UpdateCantidadAsync(id, dto);
                return NoContent();
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
