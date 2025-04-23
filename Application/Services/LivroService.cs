using Application.Dtos;

using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Application.Services
{
    public class LivroService : ILivroService
    {
        private readonly IBaseRepository<Livro> _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<Livro> _validator;

        public LivroService(IBaseRepository<Livro> repository, IMapper mapper, IValidator<Livro> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<LivroResponseDto>> GetAllAsync()
        {
            var livros = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<LivroResponseDto>>(livros);
        }

        public async Task<LivroResponseDto?> GetByIdAsync(int id)
        {
            var livro = await _repository.GetByIdAsync(id);
            if (livro == null) return null;
            return _mapper.Map<LivroResponseDto>(livro);
        }

        public async Task<LivroResponseDto> AddAsync(LivroRequestDto request)
        {
            var livro = _mapper.Map<Livro>(request);
            var validationResult = await _validator.ValidateAsync(livro);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.AddAsync(livro);
            return _mapper.Map<LivroResponseDto>(livro);
        }

        public async Task<LivroResponseDto?> UpdateAsync(int id, LivroRequestDto request)
        {
            var livro = await _repository.GetByIdAsync(id);
            if (livro == null) return null;
            _mapper.Map(request, livro);
            var validationResult = await _validator.ValidateAsync(livro);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.UpdateAsync(livro);
            return _mapper.Map<LivroResponseDto>(livro);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var livro = await _repository.GetByIdAsync(id);
            if (livro == null) return false;
            var validationResult = await _validator.ValidateAsync(livro);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.DeleteAsync(livro.Codl);
            return true;
        }
    }
}
