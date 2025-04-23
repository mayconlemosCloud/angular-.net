using Application.Dtos;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutorController : ControllerBase
    {
        private readonly IAutorService _service;

        public AutorController(IAutorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var autores = await _service.GetAllAsync();
                return Ok(autores);
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
                var autor = await _service.GetByIdAsync(id);
                if (autor == null)
                {
                    return NotFound(new { message = "Autor not found" });
                }
                return Ok(autor);
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
        public async Task<IActionResult> Add([FromBody] AutorRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdAutor = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdAutor.CodAu }, createdAutor);
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
        public async Task<IActionResult> Update(int id, [FromBody] AutorRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedAutor = await _service.UpdateAsync(id, dto);
                if (updatedAutor == null)
                {
                    return NotFound(new { message = "Autor not found" });
                }
                return Ok(updatedAutor);
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
                var isDeleted = await _service.DeleteAsync(id);
                if (!isDeleted)
                {
                    return NotFound(new { message = "Autor not found" });
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
