using Xunit;
using Application.Validations;
using Domain.Entities;
using FluentAssertions;
using Snapshooter.Xunit;

namespace Test.Validator
{
    public class LivroAssuntoValidatorTests
    {
        private readonly LivroAssuntoValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Fields_Are_Valid()
        {
            var entity = new LivroAssunto { LivroCodl = 1, AssuntoCodAs = 1 };
            var result = _validator.Validate(entity);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_LivroCodl_Is_Not_Greater_Than_Zero(int cod)
        {
            var entity = new LivroAssunto { LivroCodl = cod, AssuntoCodAs = 1 };
            var result = _validator.Validate(entity);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "LivroCodl");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_AssuntoCodAs_Is_Not_Greater_Than_Zero(int cod)
        {
            var entity = new LivroAssunto { LivroCodl = 1, AssuntoCodAs = cod };
            var result = _validator.Validate(entity);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "AssuntoCodAs");
        }

        [Fact]
        public void Snapshot_Valid_LivroAssunto()
        {
            var entity = new LivroAssunto { LivroCodl = 2, AssuntoCodAs = 3 };
            var result = _validator.Validate(entity);
            Snapshot.Match(result);
        }
    }
}
