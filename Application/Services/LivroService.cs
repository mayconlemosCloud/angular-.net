using Application.Dtos;

using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System.Linq;

namespace Application.Services
{
    public class LivroService : ILivroService
    {
        private readonly IBaseRepository<Livro> _repository;

        private readonly IBaseRepository<Autor> _AutorRepository;

        private readonly IBaseRepository<Assunto> _AssuntoRepository;
        private readonly IBaseRepository<BookTransaction> _repositoryTransaction;
        private readonly IMapper _mapper;
        private readonly IValidator<Livro> _validator;
        private readonly IValidator<BookTransaction> _validatorTransaction;

        public LivroService(IBaseRepository<Livro> repository, IMapper mapper, IValidator<Livro> validator, IBaseRepository<BookTransaction> repositoryTransaction, IValidator<BookTransaction> validatorTransaction, IBaseRepository<Autor> autorRepository, IBaseRepository<Assunto> assuntoRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _repositoryTransaction = repositoryTransaction;
            _validatorTransaction = validatorTransaction;
            _AutorRepository = autorRepository;
            _AssuntoRepository = assuntoRepository;
            _AutorRepository = autorRepository;
            _AssuntoRepository = assuntoRepository;
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _repositoryTransaction = repositoryTransaction;
            _validatorTransaction = validatorTransaction;
            _AutorRepository = autorRepository;
            _AssuntoRepository = assuntoRepository;
        }
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
        public async Task<BookTransactionResponseDto> AddTransacaoAsync(BookTransactionRequestDto request)
        {
            var transaction = _mapper.Map<BookTransaction>(request);
            var validationResult = _validatorTransaction.Validate(transaction);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _repositoryTransaction.AddAsync(transaction);
            return _mapper.Map<BookTransactionResponseDto>(transaction);
        }
        


        public async Task<IEnumerable<LivroRelatorioDto>> GetRelatorioAsync()
        {
            var livros = await _repository.GetAllAsync();

            var totalLivros = livros.ToList().Count(); 


            var Autores = await _AutorRepository.GetAllAsync();
            var totalAutores = Autores.Count();   

            var Assuntos = await _AssuntoRepository.GetAllAsync();
            var totalAssuntos = Assuntos.Count();     


            var livroMaisAntigo = livros.OrderBy(l => l.AnoPublicacao).FirstOrDefault();
            var livroMaisRecente = livros.OrderByDescending(l => l.AnoPublicacao).FirstOrDefault();

         

            int livrosComMultiplosAutores = livros.Count(l => (l.LivroAutores?.Count() ?? 0) > 1);

            var transactions = await _repositoryTransaction.GetAllAsync();
            var totalCompras = transactions.Count();

            var relatorio = new LivroRelatorioDto
            {
                TotalLivros = totalLivros,
                TotalAutores = totalAutores,          
                LivroMaisAntigo = livroMaisAntigo?.Titulo,
                AnoLivroMaisAntigo = livroMaisAntigo?.AnoPublicacao,
                LivroMaisRecente = livroMaisRecente?.Titulo,
                AnoLivroMaisRecente = livroMaisRecente?.AnoPublicacao,
                LivrosComMultiplosAutores = livrosComMultiplosAutores,
                TotalCompras = totalCompras 
            };

            return new List<LivroRelatorioDto> { relatorio };
        }
    }
}
