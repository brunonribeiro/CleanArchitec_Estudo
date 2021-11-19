using Application.Core;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.UseCases.CompanySaveUseCase;
using AutoFixture;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace Test.UseCases.CompanySaveUseCase
{
    public class CompanySaveHandlerTest
    {
        private readonly Faker _faker;
        private readonly Fixture _builder;
        private readonly MockRepository _builderMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IRabbitService> _rabbitServiceMock;
        private readonly CompanySaveHandler _companySaveHandler;
        private readonly CancellationToken _cancellationToken;
        private CompanySaveCommand _companySaveCommand;

        public CompanySaveHandlerTest()
        {
            _builderMock = new MockRepository(MockBehavior.Strict);
            _companyRepositoryMock = _builderMock.Create<ICompanyRepository>();
            _rabbitServiceMock = _builderMock.Create<IRabbitService>();
            _companySaveHandler = new CompanySaveHandler(_companyRepositoryMock.Object, _rabbitServiceMock.Object);

            _builder = new Fixture();
            _faker = new Faker("pt_BR");

            _companySaveCommand = _builder
                .Build<CompanySaveCommand>()
                .With(x => x.FoundationDate, _faker.Date.Past().ToShortDateString())
                .Create();

            _cancellationToken = _builder.Create<CancellationToken>();
        }

        private void SetupMocks()
        {
            _rabbitServiceMock.Setup(x => x.Post(_companySaveCommand));
            _companyRepositoryMock.Setup(x => x.Save(It.Is<Company>(x => x.Cnpj == _companySaveCommand.Cnpj)));
        }

        [Fact]
        public void ShouldCreateCompany()
        {
            SetupMocks();

            var response = _companySaveHandler.Handle(_companySaveCommand, _cancellationToken).Result;

            response.Result.Should().Be(Constants.MsgCompanyRegistered);
        }

        [Fact]
        public void ShouldCallServices()
        {
            SetupMocks();

            _ = _companySaveHandler.Handle(_companySaveCommand, _cancellationToken);

            _builderMock.VerifyAll();
        }

        [Fact]
        public void ShouldReturnErrorWhenRequestNull()
        {
            SetupMocks();
            _companySaveCommand = null;

            var response = _companySaveHandler.Handle(_companySaveCommand, _cancellationToken).Result;

            response.Errors.Should().Contain(Constants.MsgUnexpectedError);
        }
    }
}
