using Xunit;
using Application.Validations;
using Domain.Entities;
using FluentAssertions;
using Snapshooter.Xunit;

namespace Test.Validator
{
    public class LivroAutorValidatorTests
    {
        private readonly LivroAutorValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Fields_Are_Valid()
        {
            var entity = new LivroAutor { LivroCodl = 1, AutorCodAu = 1 };
            var result = _validator.Validate(entity);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_LivroCodl_Is_Not_Greater_Than_Zero(int cod)
        {
            var entity = new LivroAutor { LivroCodl = cod, AutorCodAu = 1 };
            var result = _validator.Validate(entity);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "LivroCodl");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_AutorCodAu_Is_Not_Greater_Than_Zero(int cod)
        {
            var entity = new LivroAutor { LivroCodl = 1, AutorCodAu = cod };
            var result = _validator.Validate(entity);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "AutorCodAu");
        }

        [Fact]
        public void Snapshot_Valid_LivroAutor()
        {
            var entity = new LivroAutor { LivroCodl = 2, AutorCodAu = 3 };
            var result = _validator.Validate(entity);
            Snapshot.Match(result);
        }
    }
}
