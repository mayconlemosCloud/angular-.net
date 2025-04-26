using Xunit;
using Moq;
using FluentAssertions;
using Snapshooter.Xunit;
using Application.Services;
using Application.Dtos;
using Domain.Entities;
using Domain.Repositories;
using AutoMapper;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

public class LivroServiceTests
{
    private readonly Mock<IBaseRepository<Livro>> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<Livro>> _validatorMock = new();
    private readonly Mock<IValidator<BookTransaction>> _validatorTransactionMock = new();
    private readonly Mock<IBaseRepository<BookTransaction>> _repositoryTransactionMock = new();

    private readonly Mock<IBaseRepository<Autor>> _repositoryAutor = new();

    private readonly Mock<IBaseRepository<Assunto>> _repositoryAssunto = new();
    private readonly LivroService _service;

    public LivroServiceTests()
    {
        _service = new LivroService(
            _repoMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _repositoryTransactionMock.Object,
            _validatorTransactionMock.Object,
            _repositoryAutor.Object,
            _repositoryAssunto.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        var livros = new List<Livro>
        {
            new Livro { Codl = 1, Titulo = "Livro 1", Editora = "Editora 1", Edicao = 1, AnoPublicacao = "2020", Preco = 0.0m },
            new Livro { Codl = 2, Titulo = "Livro 2", Editora = "Editora 2", Edicao = 2, AnoPublicacao = "2021", Preco = 0.0m },
            new Livro { Codl = 3, Titulo = "Livro 3", Editora = "Editora 3", Edicao = 3, AnoPublicacao = "2022", Preco = 0.0m }
        };
        var dtos = new List<LivroResponseDto>
        {
            new LivroResponseDto { Codl = 1, Titulo = "Livro 1", Editora = "Editora 1", Edicao = 1, AnoPublicacao = "2020", Preco = 0.0m },
            new LivroResponseDto { Codl = 2, Titulo = "Livro 2", Editora = "Editora 2", Edicao = 2, AnoPublicacao = "2021", Preco = 0.0m },
            new LivroResponseDto { Codl = 3, Titulo = "Livro 3", Editora = "Editora 3", Edicao = 3, AnoPublicacao = "2022", Preco = 0.0m }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(livros);
        _mapperMock.Setup(m => m.Map<IEnumerable<LivroResponseDto>>(livros)).Returns(dtos);

        var result = await _service.GetAllAsync();

        result.Should().BeEquivalentTo(dtos);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task GetByIdAsync_Found_ReturnsDto()
    {
        var livro = new Livro { Codl = 10, Titulo = "Livro X", Editora = "Editora X", Edicao = 1, AnoPublicacao = "2023", Preco = 0.0m  };
        var dto = new LivroResponseDto { Codl = 10, Titulo = "Livro X", Editora = "Editora X", Edicao = 1, AnoPublicacao = "2023", Preco = 0.0m };
        _repoMock.Setup(r => r.GetByIdAsync(livro.Codl)).ReturnsAsync(livro);
        _mapperMock.Setup(m => m.Map<LivroResponseDto>(livro)).Returns(dto);

        var result = await _service.GetByIdAsync(livro.Codl);

        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Livro)null);

        var result = await _service.GetByIdAsync(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_Valid_Success()
    {
        var request = new LivroRequestDto { Titulo = "Novo Livro", Editora = "Editora Nova", Edicao = 1, AnoPublicacao = "2024", Preco = 0.0m };
        var livro = new Livro { Codl = 5, Titulo = "Novo Livro", Editora = "Editora Nova", Edicao = 1, AnoPublicacao = "2024", Preco = 0.0m };
        var dto = new LivroResponseDto { Codl = 5, Titulo = "Novo Livro", Editora = "Editora Nova", Edicao = 1, AnoPublicacao = "2024", Preco = 0.0m };
        _mapperMock.Setup(m => m.Map<Livro>(request)).Returns(livro);
        _validatorMock.Setup(v => v.ValidateAsync(livro, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<LivroResponseDto>(livro)).Returns(dto);

        var result = await _service.AddAsync(request);

        _repoMock.Verify(r => r.AddAsync(livro), Times.Once);
        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task AddAsync_Invalid_ThrowsValidationException()
    {
        var request = new LivroRequestDto { Titulo = "Invalido", Editora = "Editora", Edicao = 1, AnoPublicacao = "2024" };
        var livro = new Livro { Codl = 0, Titulo = "Invalido", Editora = "Editora", Edicao = 1, AnoPublicacao = "2024" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Titulo", "Erro") };
        _mapperMock.Setup(m => m.Map<Livro>(request)).Returns(livro);
        _validatorMock.Setup(v => v.ValidateAsync(livro, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.AddAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_Found_Valid_Success()
    {
        var id = 1;
        var request = new LivroRequestDto { Titulo = "Atualizado", Editora = "Editora", Edicao = 2, AnoPublicacao = "2025", Preco = 0.0m };
        var livro = new Livro { Codl = id, Titulo = "Antigo", Editora = "Editora", Edicao = 1, AnoPublicacao = "2020", Preco = 0.0m };
        var dto = new LivroResponseDto { Codl = id, Titulo = "Atualizado", Editora = "Editora", Edicao = 2, AnoPublicacao = "2025", Preco = 0.0m };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(livro);
        _mapperMock.Setup(m => m.Map(request, livro));
        _validatorMock.Setup(v => v.ValidateAsync(livro, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<LivroResponseDto>(livro)).Returns(dto);

        var result = await _service.UpdateAsync(id, request);

        _repoMock.Verify(r => r.UpdateAsync(livro), Times.Once);
        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task UpdateAsync_NotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Livro)null);

        var result = await _service.UpdateAsync(1, new LivroRequestDto());

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Invalid_ThrowsValidationException()
    {
        var id = 1;
        var request = new LivroRequestDto { Titulo = "Invalido", Editora = "Editora", Edicao = 1, AnoPublicacao = "2024" };
        var livro = new Livro { Codl = id, Titulo = "Invalido", Editora = "Editora", Edicao = 1, AnoPublicacao = "2024" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Titulo", "Erro") };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(livro);
        _mapperMock.Setup(m => m.Map(request, livro));
        _validatorMock.Setup(v => v.ValidateAsync(livro, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateAsync(id, request));
    }

    [Fact]
    public async Task DeleteAsync_Found_Valid_Success()
    {
        var id = 1;
        var livro = new Livro { Codl = id, Titulo = "Livro", Editora = "Editora", Edicao = 1, AnoPublicacao = "2024" };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(livro);
        _validatorMock.Setup(v => v.ValidateAsync(livro, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var result = await _service.DeleteAsync(id);

        _repoMock.Verify(r => r.DeleteAsync(livro.Codl), Times.Once);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsFalse()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Livro)null);

        var result = await _service.DeleteAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_Invalid_ThrowsValidationException()
    {
        var id = 1;
        var livro = new Livro { Codl = id, Titulo = "Invalido", Editora = "Editora", Edicao = 1, AnoPublicacao = "2024" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Titulo", "Erro") };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(livro);
        _validatorMock.Setup(v => v.ValidateAsync(livro, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteAsync(id));
    }
}
