using Xunit;
using Application.Validations;
using Domain.Entities;
using FluentAssertions;
using FluentValidation.Results;
using System;

namespace Test.Validator
{
    public class BookTransactionValidatorTests
    {
        private readonly BookTransactionValidator _validator = new();

        [Fact]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            var transaction = new BookTransaction
            {
                LivroCodl = 1,
                CriadoEm = DateTime.Now.AddMinutes(-1), 
                FormaDeCompra = "balcão"
            };

            var result = _validator.Validate(transaction);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_LivroCodl_Is_Invalid()
        {
            var transaction = new BookTransaction
            {
                LivroCodl = 0, // Invalid LivroCodl
                CriadoEm = DateTime.Now,
                FormaDeCompra = "balcão"
            };

            var result = _validator.Validate(transaction);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "LivroCodl" && e.ErrorMessage.Contains("maior que zero"));
        }

    }
}
