using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services.Interfaces;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivroController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var livros = await _livroService.GetAllAsync();
                return Ok(livros);
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
                var livro = await _livroService.GetByIdAsync(id);
                if (livro == null)
                {
                    return NotFound(new { message = "Livro not found" });
                }
                return Ok(livro);
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
        public async Task<IActionResult> Create([FromBody] LivroRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdLivro = await _livroService.AddAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = createdLivro.Codl }, createdLivro);
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
        public async Task<IActionResult> Update(int id, [FromBody] LivroRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedLivro = await _livroService.UpdateAsync(id, request);
                if (updatedLivro == null)
                {
                    return NotFound(new { message = "Livro not found" });
                }
                return Ok(updatedLivro);
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
                var isDeleted = await _livroService.DeleteAsync(id);
                if (!isDeleted)
                {
                    return NotFound(new { message = "Livro not found" });
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
