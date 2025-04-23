using Xunit;
using Application.Validations;
using Domain.Entities;
using FluentAssertions;
using Bogus;
using Snapshooter.Xunit;

namespace Test.Validator
{
    public class AutorValidatorTests
    {
        private readonly AutorValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Nome_Is_Valid()
        {
            var autor = new Autor { Nome = "João da Silva" };
            var result = _validator.Validate(autor);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Fail_When_Nome_Is_Empty(string nome)
        {
            var autor = new Autor { Nome = nome };
            var result = _validator.Validate(autor);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Nome" && e.ErrorMessage.Contains("obrigatório"));
        }

        [Fact]
        public void Should_Fail_When_Nome_Exceeds_MaxLength()
        {
            var nome = new string('a', 41);
            var autor = new Autor { Nome = nome };
            var result = _validator.Validate(autor);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Nome" && e.ErrorMessage.Contains("40"));
        }

        [Theory]
        [InlineData("João@Silva")]
        [InlineData("Maria!")]
        [InlineData("José#")]
        public void Should_Fail_When_Nome_Has_Special_Characters(string nome)
        {
            var autor = new Autor { Nome = nome };
            var result = _validator.Validate(autor);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Nome" && e.ErrorMessage.Contains("caracteres especiais"));
        }

        [Fact]
        public void Should_Pass_With_Accented_Characters()
        {
            var autor = new Autor { Nome = "Érico Veríssimo" };
            var result = _validator.Validate(autor);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Snapshot_Valid_Autor()
        {
            var autor = new Autor { Nome = "Carlos Drummond de Andrade" };
            var result = _validator.Validate(autor);
            Snapshot.Match(result);
        }
    }
}
