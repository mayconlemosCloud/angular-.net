using Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAutorService
    {
        Task<IEnumerable<AutorResponseDto>> GetAllAsync();
        Task<AutorResponseDto?> GetByIdAsync(int id);
        Task<AutorResponseDto> AddAsync(AutorRequestDto request);
        Task<AutorResponseDto?> UpdateAsync(int id, AutorRequestDto request);
        Task<bool> DeleteAsync(int id);
    }
}
