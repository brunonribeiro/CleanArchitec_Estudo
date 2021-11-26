using Application.Core;
using Application.UseCases.CompanyDeleteUseCase;
using AutoFixture;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentValidation.TestHelper;
using Xunit;

namespace Test.UseCases.CompanyDeleteUseCase
{
    public class CompanyDeleteValidatorTest
    {
        private readonly Fixture _builder;
        private readonly Faker _faker;
        private readonly CompanyDeleteValidator _validator;

        public CompanyDeleteValidatorTest()
        {
            _builder = new Fixture();
            _faker = new Faker("pt_BR");
            _validator = new CompanyDeleteValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("13213465465465")]
        [InlineData("shauishauishuiashu")]
        public void ShouldHaveErrorWhenCnpjInvalid(string cnpjInvalid)
        {
            var model = _builder
                .Build<CompanyDeleteCommand>()
                 .With(x => x.Cnpj, cnpjInvalid)
                .Create();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Cnpj);
        }

        [Fact]
        public void ShouldNotHaveErrorWhenIsValid()
        {
            var model = _builder
                 .Build<CompanyDeleteCommand>()
                 .With(x => x.Cnpj, _faker.Company.Cnpj())
                 .Create();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
