using Application.Core;
using Application.UseCases.CompanySaveUseCase;
using AutoFixture;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Test.UseCases.CompanySaveUseCase
{
    public class CompanySaveValidatorTest
    {
        private readonly Fixture _builder;
        private readonly Faker _faker;
        private CompanySaveValidator _validator;

        public CompanySaveValidatorTest()
        {
            _builder = new Fixture();
            _faker = new Faker("pt_BR");
            _validator = new CompanySaveValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenNameIsEmpty(string nameInvalid)
        {
            var model = _builder
                .Build<CompanySaveCommand>()
                .With(x => x.Name, nameInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsBiggerThan100()
        {
            var nameInvalid = _faker.Random.AlphaNumeric(Constantes.QuantidadeDeCaracteres100 + 1);

            var model = _builder
                .Build<CompanySaveCommand>()
                .With(x => x.Name, nameInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("emailinvalido")]
        [InlineData("dwqjiod838449320@")]
        public void ShouldHaveErrorWhenEmailInvalid(string emailInvalid)
        {
            var model = _builder
                .Build<CompanySaveCommand>()
                 .With(x => x.Email, emailInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("13213465465465")]
        [InlineData("shauishauishuiashu")]
        public void ShouldHaveErrorWhenCnpjInvalid(string cnpjInvalid)
        {
            var model = _builder
                .Build<CompanySaveCommand>()
                 .With(x => x.Cnpj, cnpjInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Cnpj);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("16/15/2001")]
        [InlineData("19/08/2999")]
        [InlineData("datainvalida")]
        public void ShouldHaveErrorWhenFoundationDateInvalid(string foundationDateInvalid)
        {
            var model = _builder
                .Build<CompanySaveCommand>()
                 .With(x => x.FoundationDate, foundationDateInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.FoundationDate);
        }

        [Fact]
        public void ShouldNotHaveErrorWhenIsValid()
        {
            var model = _builder
                 .Build<CompanySaveCommand>()
                 .With(x => x.Email, _faker.Person.Email)
                 .With(x => x.Cnpj, _faker.Company.Cnpj())
                 .With(x => x.FoundationDate, _faker.Date.Past(30).ToShortDateString())
                 .Create();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
