using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ILivroService
    {
        Task<IEnumerable<LivroResponseDto>> GetAllAsync();
        Task<LivroResponseDto?> GetByIdAsync(int id);
        Task<LivroResponseDto> AddAsync(LivroRequestDto request);
        Task<LivroResponseDto?> UpdateAsync(int id, LivroRequestDto request);
        Task<bool> DeleteAsync(int id);
    }
}
