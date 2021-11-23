using Application.Core;
using Application.Interfaces.Repositories;
using Application.UseCases.CompanyUpdateUseCase;
using AutoFixture;
using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System.Threading;
using Xunit;

namespace Test.UseCases.CompanyUpdateUseCase
{
    public class CompanyUpdateHandlerTest
    {
        private readonly Faker _faker;
        private readonly Fixture _builder;
        private readonly Company _companySave;
        private readonly CancellationToken _cancellationToken;
        private readonly Mock<ICompanyRepositoryRedis> _companyRepositoryRedisMock;
        private readonly Mock<ICompanyRepositoryMongoDb> _companyRepositoryMongoDbMock;
        private readonly CompanyUpdateHandler _companyUpdateHandler;
        private CompanyUpdateCommand _companyUpdateCommand;

        public CompanyUpdateHandlerTest()
        {
            _companyRepositoryRedisMock = new Mock<ICompanyRepositoryRedis>(MockBehavior.Strict);
            _companyRepositoryMongoDbMock = new Mock<ICompanyRepositoryMongoDb>(MockBehavior.Strict);
            _companyUpdateHandler = new CompanyUpdateHandler(_companyRepositoryRedisMock.Object, _companyRepositoryMongoDbMock.Object);

            _builder = new Fixture();
            _faker = new Faker("pt_BR");

            _companyUpdateCommand = _builder
                .Build<CompanyUpdateCommand>()
                .With(x => x.FoundationDate, _faker.Date.Past().ToShortDateString())
                .Create();

            _companySave = _builder
                .Build<Company>()
                .With(x => x.Cnpj, _companyUpdateCommand.Cnpj)
                .Create();

            _companyRepositoryRedisMock
                .Setup(x => x.GetByCnpj(_companyUpdateCommand.Cnpj))
                .Returns(_companySave);

            _cancellationToken = _builder.Create<CancellationToken>();
        }

        [Fact]
        public void ShouldUpdateCompany()
        {
            _companyRepositoryRedisMock.Setup(x => x.Save(_companySave));

            var response = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken).Result;

            _companyRepositoryRedisMock.VerifyAll();
            response.Result.Should().Be(Constants.MsgCompanyChanged);
        }

        [Fact]
        public void ShouldUpdateCompanyName()
        {
            _ = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken);

            _companySave.Name.Should().Be(_companyUpdateCommand.Name);
        }

        [Fact]
        public void ShouldUpdateCompanyEmail()
        {
            _ = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken);

            _companySave.Email.Should().Be(_companyUpdateCommand.Email);
        }

        [Fact]
        public void ShouldUpdateCompanyFoundationDate()
        {
            _ = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken);

            _companySave.FoundationDate.Should().Be(_companyUpdateCommand.FoundationDate.ToDate());
        }

        [Fact]
        public void NotShouldUpdateCompanyCnpj()
        {
            _companyUpdateCommand.Cnpj = _faker.Company.Cnpj();

            _ = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken);

            _companySave.Cnpj.Should().NotBe(_companyUpdateCommand.Cnpj);
        }

        [Fact]
        public void ShouldReturnErrorWhenCompanyNotFound()
        {
            _companyRepositoryRedisMock.Setup(x => x.GetByCnpj(_companyUpdateCommand.Cnpj)).Returns<Company>(null);

            var response = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken).Result;

            response.Errors.Should().Contain(Constants.MsgCompanyNotFound);
        }

        [Fact]
        public void ShouldThrowExceptionWhenRequestNull()
        {
            _companyUpdateCommand = null;

            var response = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken).Result;

            response.Errors.Should().Contain(Constants.MsgUnexpectedError);
        }
    }
}