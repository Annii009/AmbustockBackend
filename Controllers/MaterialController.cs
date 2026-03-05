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
            var materiales = await _service.GetAllAsync();
            return Ok(materiales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialDto>> GetById(int id)
        {
            var material = await _service.GetByIdAsync(id);
            return Ok(material);
        }

        [HttpGet("zona/{idZona}")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetByZonaId(int idZona)
        {
            var materiales = await _service.GetByZonaIdAsync(idZona);
            return Ok(materiales);
        }

        [HttpGet("cajon/{idCajon}")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetByCajonId(int idCajon)
        {
            var materiales = await _service.GetByCajonIdAsync(idCajon);
            return Ok(materiales);
        }

        [HttpGet("stock-bajo/{cantidadMinima}")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetByCantidadBaja(int cantidadMinima)
        {
            var materiales = await _service.GetByCantidadBajaAsync(cantidadMinima);
            return Ok(materiales);
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDto>> Create([FromBody] CreateMaterialDto dto)
        {
            var material = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = material.IdMaterial }, material);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MaterialDto>> Update(int id, [FromBody] UpdateMaterialDto dto)
        {
            var material = await _service.UpdateAsync(id, dto);
            return Ok(material);
        }

        [HttpPatch("{id}/cantidad")]
        public async Task<ActionResult> UpdateCantidad(int id, [FromBody] UpdateCantidadMaterialDto dto)
        {
            await _service.UpdateCantidadAsync(id, dto);
            return NoContent();
        }

        [HttpPost("{id}/foto")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<MaterialDto>> SubirFoto(int id, IFormFile foto)
        {
            if (foto == null || foto.Length == 0)
                return BadRequest("Debes enviar un archivo en el campo 'foto'.");

            // Validar que sea una imagen
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(foto.FileName).ToLowerInvariant();
            if (!extensionesPermitidas.Contains(extension))
                return BadRequest("Solo se permiten imágenes JPG, PNG o WebP.");

            var material = await _service.SubirFotoAsync(id, foto);
            return Ok(material);
        }

        [HttpDelete("{id}/foto")]
        public async Task<ActionResult> EliminarFoto(int id)
        {
            await _service.EliminarFotoAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}