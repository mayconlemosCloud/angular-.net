using Application.Dtos;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AutorService : IAutorService
    {
        private readonly IBaseRepository<Autor> _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<Autor> _validator;

        public AutorService(IBaseRepository<Autor> repository, IMapper mapper, IValidator<Autor> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<AutorResponseDto>> GetAllAsync()
        {
            var autores = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AutorResponseDto>>(autores);
        }

        public async Task<AutorResponseDto?> GetByIdAsync(int id)
        {
            var autor = await _repository.GetByIdAsync(id);
            if (autor == null) return null;
            return _mapper.Map<AutorResponseDto>(autor);
        }

        public async Task<AutorResponseDto> AddAsync(AutorRequestDto request)
        {
            var autor = _mapper.Map<Autor>(request);
            var validationResult = await _validator.ValidateAsync(autor);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.AddAsync(autor);
            return _mapper.Map<AutorResponseDto>(autor);
        }

        public async Task<AutorResponseDto?> UpdateAsync(int id, AutorRequestDto request)
        {
            var autor = await _repository.GetByIdAsync(id);
            if (autor == null) return null;
            _mapper.Map(request, autor);
            var validationResult = await _validator.ValidateAsync(autor);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.UpdateAsync(autor);
            return _mapper.Map<AutorResponseDto>(autor);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var autor = await _repository.GetByIdAsync(id);
            if (autor == null) return false;
            var validationResult = await _validator.ValidateAsync(autor);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.DeleteAsync(autor.CodAu);
            return true;
        }
    }
}
