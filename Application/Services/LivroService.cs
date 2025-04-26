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

        private readonly IBaseRepository<BookTransaction> _repositoryTransaction;
        private readonly IMapper _mapper;
        private readonly IValidator<Livro> _validator;
        private readonly IValidator<BookTransaction> _validatorTransaction;

        public LivroService(IBaseRepository<Livro> repository, IMapper mapper, IValidator<Livro> validator, IBaseRepository<BookTransaction> repositoryTransaction, IValidator<BookTransaction> validatorTransaction)
        {
            _repositoryTransaction = repositoryTransaction;
            _validatorTransaction = validatorTransaction;
            _repositoryTransaction = repositoryTransaction;
            _validatorTransaction = validatorTransaction;
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _repositoryTransaction = repositoryTransaction;
            _validatorTransaction = validatorTransaction;
            
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
            var livros = (await _repository.GetAllAsync()).ToList();
            var totalLivros = livros.Count;

       
            var totalAutores = livros
                .SelectMany(l => l.LivroAutores ?? Enumerable.Empty<LivroAutor>())
                .Select(la => la.Autor?.CodAu)
                .Where(codAu => codAu != null)
                .Distinct()
                .Count();

            
            var livrosPorAutor = livros
                .SelectMany(l => l.LivroAutores ?? Enumerable.Empty<LivroAutor>())
                .GroupBy(la => la.Autor?.CodAu)
                .Select(g => g.Count())
                .ToList();
            double mediaLivrosPorAutor = livrosPorAutor.Count > 0 ? livrosPorAutor.Average() : 0;

           
            var autorMaisLivros = livros
                .SelectMany(l => l.LivroAutores ?? Enumerable.Empty<LivroAutor>())
                .GroupBy(la => la.Autor)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            string nomeAutorMaisLivros = autorMaisLivros?.Key?.Nome ?? "";
            int qtdAutorMaisLivros = autorMaisLivros?.Count() ?? 0;

 
            var livrosComAno = livros
                .Where(l => int.TryParse(l.AnoPublicacao, out _))
                .Select(l => new { Livro = l, Ano = int.Parse(l.AnoPublicacao) })
                .ToList();
            var livroMaisAntigo = livrosComAno.OrderBy(l => l.Ano).FirstOrDefault();
            var livroMaisRecente = livrosComAno.OrderByDescending(l => l.Ano).FirstOrDefault();

          

          
            int livrosSemAutores = livros.Count(l => l.LivroAutores == null || !l.LivroAutores.Any());

          
            int livrosComMultiplosAutores = livros.Count(l => (l.LivroAutores?.Count() ?? 0) > 1);

            

            var relatorio = new LivroRelatorioDto
            {
                TotalLivros = totalLivros,
                TotalAutores = totalAutores,
                MediaLivrosPorAutor = mediaLivrosPorAutor,
                NomeAutorMaisLivros = nomeAutorMaisLivros,
                QtdAutorMaisLivros = qtdAutorMaisLivros,
                LivroMaisAntigo = livroMaisAntigo?.Livro?.Titulo,
                AnoLivroMaisAntigo = livroMaisAntigo?.Ano,
                LivroMaisRecente = livroMaisRecente?.Livro?.Titulo,
                AnoLivroMaisRecente = livroMaisRecente?.Ano,
                LivrosSemAutores = livrosSemAutores,
                LivrosComMultiplosAutores = livrosComMultiplosAutores            
            };


            return new List<LivroRelatorioDto> { relatorio };
        }

       
    }
}
