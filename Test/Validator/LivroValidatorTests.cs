using Xunit;
using Application.Validations;
using Domain.Entities;
using FluentAssertions;
using Bogus;
using Snapshooter.Xunit;
using System;

namespace Test.Validator
{
    public class LivroValidatorTests
    {
        private readonly LivroValidator _validator = new();

        private Livro CreateValidLivro() =>
            new Livro
            {
                Titulo = "Livro Teste",
                Editora = "Editora Teste",
                Edicao = 1,
                AnoPublicacao = DateTime.Now.Year.ToString()
            };

        [Fact]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            var livro = CreateValidLivro();
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Fail_When_Titulo_Is_Empty(string titulo)
        {
            var livro = CreateValidLivro();
            livro.Titulo = titulo;
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Titulo");
        }

        [Fact]
        public void Should_Fail_When_Titulo_Exceeds_MaxLength()
        {
            var livro = CreateValidLivro();
            livro.Titulo = new string('a', 41);
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Titulo_Has_Special_Characters()
        {
            var livro = CreateValidLivro();
            livro.Titulo = "Livro@123";
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Fail_When_Editora_Is_Empty(string editora)
        {
            var livro = CreateValidLivro();
            livro.Editora = editora;
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Editora");
        }

        [Fact]
        public void Should_Fail_When_Editora_Exceeds_MaxLength()
        {
            var livro = CreateValidLivro();
            livro.Editora = new string('a', 41);
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Editora_Has_Special_Characters()
        {
            var livro = CreateValidLivro();
            livro.Editora = "Editora!";
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_Edicao_Is_Not_Greater_Than_Zero(int edicao)
        {
            var livro = CreateValidLivro();
            livro.Edicao = edicao;
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Edicao");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Fail_When_AnoPublicacao_Is_Empty(string ano)
        {
            var livro = CreateValidLivro();
            livro.AnoPublicacao = ano;
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "AnoPublicacao");
        }

        [Fact]
        public void Should_Fail_When_AnoPublicacao_Exceeds_MaxLength()
        {
            var livro = CreateValidLivro();
            livro.AnoPublicacao = "20231";
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("abcd")]
        [InlineData("20a3")]
        [InlineData("123")]
        public void Should_Fail_When_AnoPublicacao_Is_Not_4_Digits(string ano)
        {
            var livro = CreateValidLivro();
            livro.AnoPublicacao = ano;
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("1899")]
        [InlineData("3000")]
        public void Should_Fail_When_AnoPublicacao_Is_Out_Of_Range(string ano)
        {
            var livro = CreateValidLivro();
            livro.AnoPublicacao = ano;
            var result = _validator.Validate(livro);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Snapshot_Valid_Livro()
        {
            var livro = CreateValidLivro();
            var result = _validator.Validate(livro);
            Snapshot.Match(result);
        }
    }
}
