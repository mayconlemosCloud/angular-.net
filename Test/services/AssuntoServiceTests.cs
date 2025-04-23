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

public class AssuntoServiceTests
{
    private readonly Mock<IBaseRepository<Assunto>> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<Assunto>> _validatorMock = new();
    private readonly AssuntoService _service;

    public AssuntoServiceTests()
    {
        _service = new AssuntoService(_repoMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        var assuntos = new List<Assunto>
        {
            new Assunto { CodAs = 1, Descricao = "Assunto 1" },
            new Assunto { CodAs = 2, Descricao = "Assunto 2" },
            new Assunto { CodAs = 3, Descricao = "Assunto 3" }
        };
        var dtos = new List<AssuntoResponseDto>
        {
            new AssuntoResponseDto { CodAs = 1, Descricao = "Assunto 1" },
            new AssuntoResponseDto { CodAs = 2, Descricao = "Assunto 2" },
            new AssuntoResponseDto { CodAs = 3, Descricao = "Assunto 3" }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(assuntos);
        _mapperMock.Setup(m => m.Map<IEnumerable<AssuntoResponseDto>>(assuntos)).Returns(dtos);

        var result = await _service.GetAllAsync();

        result.Should().BeEquivalentTo(dtos);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task GetByIdAsync_Found_ReturnsDto()
    {
        var assunto = new Assunto { CodAs = 10, Descricao = "Assunto X" };
        var dto = new AssuntoResponseDto { CodAs = 10, Descricao = "Assunto X" };
        _repoMock.Setup(r => r.GetByIdAsync(assunto.CodAs)).ReturnsAsync(assunto);
        _mapperMock.Setup(m => m.Map<AssuntoResponseDto>(assunto)).Returns(dto);

        var result = await _service.GetByIdAsync(assunto.CodAs);

        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Assunto)null);

        var result = await _service.GetByIdAsync(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_Valid_Success()
    {
        var request = new AssuntoRequestDto { Descricao = "Novo Assunto" };
        var assunto = new Assunto { CodAs = 5, Descricao = "Novo Assunto" };
        var dto = new AssuntoResponseDto { CodAs = 5, Descricao = "Novo Assunto" };
        _mapperMock.Setup(m => m.Map<Assunto>(request)).Returns(assunto);
        _validatorMock.Setup(v => v.ValidateAsync(assunto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<AssuntoResponseDto>(assunto)).Returns(dto);

        var result = await _service.AddAsync(request);

        _repoMock.Verify(r => r.AddAsync(assunto), Times.Once);
        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task AddAsync_Invalid_ThrowsValidationException()
    {
        var request = new AssuntoRequestDto { Descricao = "Invalido" };
        var assunto = new Assunto { CodAs = 0, Descricao = "Invalido" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Descricao", "Erro") };
        _mapperMock.Setup(m => m.Map<Assunto>(request)).Returns(assunto);
        _validatorMock.Setup(v => v.ValidateAsync(assunto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.AddAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_Found_Valid_Success()
    {
        var id = 1;
        var request = new AssuntoRequestDto { Descricao = "Atualizado" };
        var assunto = new Assunto { CodAs = id, Descricao = "Antigo" };
        var dto = new AssuntoResponseDto { CodAs = id, Descricao = "Atualizado" };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(assunto);
        _mapperMock.Setup(m => m.Map(request, assunto));
        _validatorMock.Setup(v => v.ValidateAsync(assunto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<AssuntoResponseDto>(assunto)).Returns(dto);

        var result = await _service.UpdateAsync(id, request);

        _repoMock.Verify(r => r.UpdateAsync(assunto), Times.Once);
        result.Should().BeEquivalentTo(dto);
        Snapshot.Match(result);
    }

    [Fact]
    public async Task UpdateAsync_NotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Assunto)null);

        var result = await _service.UpdateAsync(1, new AssuntoRequestDto());

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Invalid_ThrowsValidationException()
    {
        var id = 1;
        var request = new AssuntoRequestDto { Descricao = "Invalido" };
        var assunto = new Assunto { CodAs = id, Descricao = "Invalido" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Descricao", "Erro") };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(assunto);
        _mapperMock.Setup(m => m.Map(request, assunto));
        _validatorMock.Setup(v => v.ValidateAsync(assunto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateAsync(id, request));
    }

    [Fact]
    public async Task DeleteAsync_Found_Valid_Success()
    {
        var id = 1;
        var assunto = new Assunto { CodAs = id, Descricao = "Assunto" };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(assunto);
        _validatorMock.Setup(v => v.ValidateAsync(assunto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var result = await _service.DeleteAsync(id);

        _repoMock.Verify(r => r.DeleteAsync(assunto.CodAs), Times.Once);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsFalse()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Assunto)null);

        var result = await _service.DeleteAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_Invalid_ThrowsValidationException()
    {
        var id = 1;
        var assunto = new Assunto { CodAs = id, Descricao = "Invalido" };
        var errors = new List<FluentValidation.Results.ValidationFailure> { new("Descricao", "Erro") };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(assunto);
        _validatorMock.Setup(v => v.ValidateAsync(assunto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteAsync(id));
    }
}
