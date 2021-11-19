using Application.Core;
using Application.UseCases.CompanyUpdateUseCase;
using AutoFixture;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentValidation.TestHelper;
using Xunit;

namespace Test.UseCases.CompanyUpdateUseCase
{
    public class CompanyUpdateValidatorTest
    {
        private readonly Fixture _builder;
        private readonly Faker _faker;
        private CompanyUpdateValidator _validator;

        public CompanyUpdateValidatorTest()
        {
            _builder = new Fixture();
            _faker = new Faker("pt_BR");
            _validator = new CompanyUpdateValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenNameIsEmpty(string nameInvalid)
        {
            var model = _builder
                .Build<CompanyUpdateCommand>()
                .With(x => x.Name, nameInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsBiggerThan100()
        {
            var nameInvalid = _faker.Random.String(minLength: Constants.NumberOfCharacters100 + 1, maxLength: 1000);

            var model = _builder
                .Build<CompanyUpdateCommand>()
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
                .Build<CompanyUpdateCommand>()
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
                .Build<CompanyUpdateCommand>()
                 .With(x => x.Cnpj, cnpjInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Cnpj);
        }

        [Theory]
        [InlineData("16/15/2001")]
        [InlineData("19/08/2999")]
        [InlineData("datainvalida")]
        public void ShouldHaveErrorWhenFoundationDateInvalid(string foundationDateInvalid)
        {
            var model = _builder
                .Build<CompanyUpdateCommand>()
                 .With(x => x.FoundationDate, foundationDateInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.FoundationDate);
        }

        [Fact]
        public void ShouldNotHaveErrorWhenIsValid()
        {
            var model = _builder
                 .Build<CompanyUpdateCommand>()
                 .With(x => x.Email, _faker.Person.Email)
                 .With(x => x.Cnpj, _faker.Company.Cnpj())
                 .With(x => x.FoundationDate, _faker.Date.Past(30).ToShortDateString())
                 .Create();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
