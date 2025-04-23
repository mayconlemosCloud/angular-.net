using Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAssuntoService
    {
        Task<IEnumerable<AssuntoResponseDto>> GetAllAsync();
        Task<AssuntoResponseDto?> GetByIdAsync(int id);
        Task<AssuntoResponseDto> AddAsync(AssuntoRequestDto request);
        Task<AssuntoResponseDto?> UpdateAsync(int id, AssuntoRequestDto request);
        Task<bool> DeleteAsync(int id);
    }
}
