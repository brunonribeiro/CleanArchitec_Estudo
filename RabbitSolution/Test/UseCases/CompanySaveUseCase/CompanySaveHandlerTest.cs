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
        private Faker _faker;
        private Fixture _builder;
        private CompanySaveCommand _companySaveCommand;
        private CancellationToken _cancellationToken;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IRabbitService> _rabbitServiceMock;
        private readonly CompanySaveHandler _companySaveHandler;

        public CompanySaveHandlerTest()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _rabbitServiceMock = new Mock<IRabbitService>();
            _companySaveHandler = new CompanySaveHandler(_companyRepositoryMock.Object, _rabbitServiceMock.Object);         
        }

        private void Setup()
        {
            _builder = new Fixture();
            _faker = new Faker("pt_BR");

            _companySaveCommand = _builder
                .Build<CompanySaveCommand>()
                .With(x => x.FoundationDate, _faker.Date.Past().ToShortDateString())
                .Create();

            _cancellationToken = _builder.Create<CancellationToken>();
        }

        [Fact]
        public void ShouldCreateCompany()
        {
            Setup();

            var response = _companySaveHandler.Handle(_companySaveCommand, _cancellationToken).Result;

            response.Result.Should().Be(Constantes.EmpresaCadastrada);
        }

        [Fact]
        public void ShouldCallRepository()
        {
            Setup();

            var response = _companySaveHandler.Handle(_companySaveCommand, _cancellationToken).Result;

            _companyRepositoryMock.Verify(x => x.Save(It.IsAny<Company>()));
        }

        [Fact]
        public void ShouldCallRabbit()
        {
            Setup();

            var response = _companySaveHandler.Handle(_companySaveCommand, _cancellationToken).Result;

            _rabbitServiceMock.Verify(x => x.Post(It.IsAny<CompanySaveCommand>()));
        }

        [Fact]
        public void ShouldThrowExceptionWhenRequestNull()
        {
            Setup();
            _companySaveCommand = null;

            var response = _companySaveHandler.Invoking(x => x.Handle(_companySaveCommand, _cancellationToken));

            response.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
