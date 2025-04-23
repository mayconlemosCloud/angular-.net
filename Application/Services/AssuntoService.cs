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
    public class AssuntoService : IAssuntoService
    {
        private readonly IBaseRepository<Assunto> _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<Assunto> _validator;

        public AssuntoService(IBaseRepository<Assunto> repository, IMapper mapper, IValidator<Assunto> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<AssuntoResponseDto>> GetAllAsync()
        {
            var assuntos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AssuntoResponseDto>>(assuntos);
        }

        public async Task<AssuntoResponseDto?> GetByIdAsync(int id)
        {
            var assunto = await _repository.GetByIdAsync(id);
            if (assunto == null) return null;
            return _mapper.Map<AssuntoResponseDto>(assunto);
        }

        public async Task<AssuntoResponseDto> AddAsync(AssuntoRequestDto request)
        {
            var assunto = _mapper.Map<Assunto>(request);
            var validationResult = await _validator.ValidateAsync(assunto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.AddAsync(assunto);
            return _mapper.Map<AssuntoResponseDto>(assunto);
        }

        public async Task<AssuntoResponseDto?> UpdateAsync(int id, AssuntoRequestDto request)
        {
            var assunto = await _repository.GetByIdAsync(id);
            if (assunto == null) return null;
            _mapper.Map(request, assunto);
            var validationResult = await _validator.ValidateAsync(assunto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.UpdateAsync(assunto);
            return _mapper.Map<AssuntoResponseDto>(assunto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var assunto = await _repository.GetByIdAsync(id);
            if (assunto == null) return false;
            var validationResult = await _validator.ValidateAsync(assunto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repository.DeleteAsync(assunto.CodAs);
            return true;
        }
    }
}
