using Microsoft.AspNetCore.Mvc;
using AmbustockBackend.Services;
using AmbustockBackend.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace AmbustockBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsableController : ControllerBase
    {
        private readonly ResponsableService _service;

        public ResponsableController(ResponsableService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsableDto>>> GetAll()
        {
                var responsables = await _service.GetAllAsync();
                return Ok(responsables);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsableDto>> GetById(int id)
        {
                var responsable = await _service.GetByIdAsync(id);
                return Ok(responsable);
        }

        [HttpGet("servicio/{idServicio}")]
        public async Task<ActionResult<IEnumerable<ResponsableDto>>> GetByServicio(int idServicio)
        {
                var responsables = await _service.GetByServicioIdAsync(idServicio);
                return Ok(responsables);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<ResponsableDto>>> GetByUsuario(int idUsuario)
        {
                var responsables = await _service.GetByUsuarioIdAsync(idUsuario);
                return Ok(responsables);
        }

        [HttpPost]
        public async Task<ActionResult<ResponsableDto>> Create([FromBody] CreateResponsableDto dto)
        {
                var responsable = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = responsable.IdResponsable }, responsable);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponsableDto>> Update(int id, [FromBody] UpdateResponsableDto dto)
        {
                var responsable = await _service.UpdateAsync(id, dto);
                return Ok(responsable);
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            
                await _service.DeleteAsync(id);
                return NoContent();
            
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ResponsableDto>>> Search([FromQuery] string q)
        {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
                return Ok(new List<ResponsableDto>());

        var results = await _service.SearchByNombreAsync(q);
        return Ok(results);
        }

    }
    
}
