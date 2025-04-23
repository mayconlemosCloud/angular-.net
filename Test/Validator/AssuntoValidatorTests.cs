using Xunit;
using Application.Validations;
using Domain.Entities;
using FluentAssertions;
using Snapshooter.Xunit;

namespace Test.Validator
{
    public class AssuntoValidatorTests
    {
        private readonly AssuntoValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Descricao_Is_Valid()
        {
            var assunto = new Assunto { Descricao = "Direito Civil" };
            var result = _validator.Validate(assunto);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Fail_When_Descricao_Is_Empty(string descricao)
        {
            var assunto = new Assunto { Descricao = descricao };
            var result = _validator.Validate(assunto);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Descricao");
        }

        [Fact]
        public void Should_Fail_When_Descricao_Exceeds_MaxLength()
        {
            var descricao = new string('a', 21);
            var assunto = new Assunto { Descricao = descricao };
            var result = _validator.Validate(assunto);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Descricao" && e.ErrorMessage.Contains("20"));
        }

        [Fact]
        public void Snapshot_Valid_Assunto()
        {
            var assunto = new Assunto { Descricao = "Processo Penal" };
            var result = _validator.Validate(assunto);
            Snapshot.Match(result);
        }
    }
}
