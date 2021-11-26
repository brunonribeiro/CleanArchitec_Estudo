using Application.Core;
using Application.Interfaces.Repositories;
using Application.UseCases.CompanyDeleteUseCase;
using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System.Threading;
using Xunit;

namespace Test.UseCases.CompanyDeleteUseCase
{
    public class CompanyDeleteHandlerTest
    {
        private readonly Fixture _builder;
        private readonly Company _companySave;
        private readonly MockRepository _builderMock;
        private readonly CancellationToken _cancellationToken;
        private readonly Mock<ICompanyRepositoryRedis> _companyRepositoryRedisMock;
        private readonly Mock<ICompanyRepositoryMongoDb> _companyRepositoryMongoDbMock;
        private readonly CompanyDeleteHandler _companyDeleteHandler;
        private CompanyDeleteCommand _companyDeleteCommand;

        public CompanyDeleteHandlerTest()
        {
            _builderMock = new MockRepository(MockBehavior.Strict);
            _companyRepositoryRedisMock = _builderMock.Create<ICompanyRepositoryRedis>();
            _companyRepositoryMongoDbMock = _builderMock.Create<ICompanyRepositoryMongoDb>();
            _companyDeleteHandler = new CompanyDeleteHandler(_companyRepositoryRedisMock.Object, _companyRepositoryMongoDbMock.Object);

            _builder = new Fixture();

            _companyDeleteCommand = _builder.Create<CompanyDeleteCommand>();
            _cancellationToken = _builder.Create<CancellationToken>();

            _companySave = _builder
             .Build<Company>()
             .With(x => x.Cnpj, _companyDeleteCommand.Cnpj)
             .Create();

            _companyRepositoryRedisMock.Setup(x => x.Get(_companyDeleteCommand.Cnpj)).Returns(_companySave);
            _companyRepositoryMongoDbMock.Setup(x => x.QueryByCnpj(_companyDeleteCommand.Cnpj)).Returns(_companySave);
        }

        [Fact]
        public void ShouldDeleteCompany()
        {
            _companyRepositoryRedisMock.Setup(x => x.Delete(_companySave));
            _companyRepositoryMongoDbMock.Setup(x => x.Delete(_companySave));

            var response = _companyDeleteHandler.Handle(_companyDeleteCommand, _cancellationToken).Result;

            _builderMock.VerifyAll();
            response.Result.Should().Be(Constants.MsgCompanyDeleted);
        }

        [Fact]
        public void ShouldDeleteCompanyOnlyDataBase()
        {
            _companyRepositoryRedisMock.Setup(x => x.Get(_companyDeleteCommand.Cnpj)).Returns((Company)null);
            _companyRepositoryMongoDbMock.Setup(x => x.Delete(_companySave));

            var response = _companyDeleteHandler.Handle(_companyDeleteCommand, _cancellationToken).Result;

            _builderMock.VerifyAll();
            response.Result.Should().Be(Constants.MsgCompanyDeleted);
        }

        [Fact]
        public void ShouldReturnErrorWhenCompanyNotFound()
        {
            _companyRepositoryRedisMock.Setup(x => x.Get(_companyDeleteCommand.Cnpj)).Returns((Company)null);
            _companyRepositoryMongoDbMock.Setup(x => x.QueryByCnpj(_companyDeleteCommand.Cnpj)).Returns((Company)null);

            var response = _companyDeleteHandler.Handle(_companyDeleteCommand, _cancellationToken).Result;

            response.Errors.Should().Contain(Constants.MsgCompanyNotFound);
        }

        [Fact]
        public void ShouldThrowExceptionWhenRequestNull()
        {
            _companyDeleteCommand = null;

            var response = _companyDeleteHandler.Handle(_companyDeleteCommand, _cancellationToken).Result;

            response.Errors.Should().Contain(Constants.MsgUnexpectedError);
        }
    }
}
