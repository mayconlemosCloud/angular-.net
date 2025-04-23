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

public class AutorServiceTests
{
    private readonly Mock<IBaseRepository<Autor>> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<Autor>> _validatorMock = new();
    private readonly AutorService _service;

    public AutorServiceTests()
    {
        _service = new AutorService(_repoMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        var autores = new List<Autor>
        {
            new Autor { CodAu = 1, Nome = "Autor 1" },
            new Autor { CodAu = 2, Nome = "Autor 2" },
            new Autor { CodAu = 3, Nome = "Autor 3" }
        };
        var dtos = new List<AutorResponseDto>
        {
            new AutorResponseDto { CodAu = 1, Nome = "Autor 1" },
            new AutorResponseDto { CodAu = 2, Nome = "Autor 2" },
            new AutorResponseDto { CodAu = 3, Nome = "Autor 3" }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(autores);
        _mapperMock.Setup(m => m.Map<IEnumerable<AutorResponseDto>>(autores)).Returns(dtos);

        var result = await _service.GetAllAsync();

        result.Should().BeEquivalentTo(dtos);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task GetByIdAsync_Found_ReturnsDto()
    {
        var autor = new Autor { CodAu = 10, Nome = "Autor X" };
        var dto = new AutorResponseDto { CodAu = 10, Nome = "Autor X" };
        _repoMock.Setup(r => r.GetByIdAsync(autor.CodAu)).ReturnsAsync(autor);
        _mapperMock.Setup(m => m.Map<AutorResponseDto>(autor)).Returns(dto);

        var result = await _service.GetByIdAsync(autor.CodAu);

        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Autor)null);

        var result = await _service.GetByIdAsync(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_Valid_Success()
    {
        var request = new AutorRequestDto { Nome = "Novo Autor" };
        var autor = new Autor { CodAu = 5, Nome = "Novo Autor" };
        var dto = new AutorResponseDto { CodAu = 5, Nome = "Novo Autor" };
        _mapperMock.Setup(m => m.Map<Autor>(request)).Returns(autor);
        _validatorMock.Setup(v => v.ValidateAsync(autor, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<AutorResponseDto>(autor)).Returns(dto);

        var result = await _service.AddAsync(request);

        _repoMock.Verify(r => r.AddAsync(autor), Times.Once);
        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task AddAsync_Invalid_ThrowsValidationException()
    {
        var request = new AutorRequestDto { Nome = "Invalido" };
        var autor = new Autor { CodAu = 0, Nome = "Invalido" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Nome", "Erro") };
        _mapperMock.Setup(m => m.Map<Autor>(request)).Returns(autor);
        _validatorMock.Setup(v => v.ValidateAsync(autor, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.AddAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_Found_Valid_Success()
    {
        var id = 1;
        var request = new AutorRequestDto { Nome = "Atualizado" };
        var autor = new Autor { CodAu = id, Nome = "Antigo" };
        var dto = new AutorResponseDto { CodAu = id, Nome = "Atualizado" };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(autor);
        _mapperMock.Setup(m => m.Map(request, autor));
        _validatorMock.Setup(v => v.ValidateAsync(autor, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<AutorResponseDto>(autor)).Returns(dto);

        var result = await _service.UpdateAsync(id, request);

        _repoMock.Verify(r => r.UpdateAsync(autor), Times.Once);
        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task UpdateAsync_NotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Autor)null);

        var result = await _service.UpdateAsync(1, new AutorRequestDto());

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Invalid_ThrowsValidationException()
    {
        var id = 1;
        var request = new AutorRequestDto { Nome = "Invalido" };
        var autor = new Autor { CodAu = id, Nome = "Invalido" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Nome", "Erro") };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(autor);
        _mapperMock.Setup(m => m.Map(request, autor));
        _validatorMock.Setup(v => v.ValidateAsync(autor, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateAsync(id, request));
    }

    [Fact]
    public async Task DeleteAsync_Found_Valid_Success()
    {
        var id = 1;
        var autor = new Autor { CodAu = id, Nome = "Autor" };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(autor);
        _validatorMock.Setup(v => v.ValidateAsync(autor, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var result = await _service.DeleteAsync(id);

        _repoMock.Verify(r => r.DeleteAsync(autor.CodAu), Times.Once);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsFalse()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Autor)null);

        var result = await _service.DeleteAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_Invalid_ThrowsValidationException()
    {
        var id = 1;
        var autor = new Autor { CodAu = id, Nome = "Invalido" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Nome", "Erro") };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(autor);
        _validatorMock.Setup(v => v.ValidateAsync(autor, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteAsync(id));
    }
}
