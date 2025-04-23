using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services.Interfaces;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssuntoController : ControllerBase
    {
        private readonly IAssuntoService _assuntoService;

        public AssuntoController(IAssuntoService assuntoService)
        {
            _assuntoService = assuntoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var assuntos = await _assuntoService.GetAllAsync();
                return Ok(assuntos);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var assunto = await _assuntoService.GetByIdAsync(id);
                if (assunto == null)
                {
                    return NotFound(new { message = "Assunto not found" });
                }
                return Ok(assunto);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssuntoRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdAssunto = await _assuntoService.AddAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = createdAssunto.CodAs }, createdAssunto);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AssuntoRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedAssunto = await _assuntoService.UpdateAsync(id, request);
                if (updatedAssunto == null)
                {
                    return NotFound(new { message = "Assunto not found" });
                }
                return Ok(updatedAssunto);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _assuntoService.DeleteAsync(id);
                if (!isDeleted)
                {
                    return NotFound(new { message = "Assunto not found" });
                }
                return NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
