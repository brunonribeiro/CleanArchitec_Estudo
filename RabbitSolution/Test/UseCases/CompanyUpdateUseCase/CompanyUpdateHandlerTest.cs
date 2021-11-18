using Application.Core;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.UseCases.CompanyUpdateUseCase;
using AutoFixture;
using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace Test.UseCases.CompanyUpdateUseCase
{
    public class CompanyUpdateHandlerTest
    {
        private Faker _faker;
        private Fixture _builder;
        private CompanyUpdateCommand _companyUpdateCommand;
        private Company _companySave;
        private CancellationToken _cancellationToken;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly CompanyUpdateHandler _companyUpdateHandler;

        public CompanyUpdateHandlerTest()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _companyUpdateHandler = new CompanyUpdateHandler(_companyRepositoryMock.Object);
        }

        private void Setup()
        {
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

            _companyRepositoryMock
                .Setup(x => x.GetByCnpj(_companyUpdateCommand.Cnpj))
                .Returns(_companySave);

            _cancellationToken = _builder.Create<CancellationToken>();
        }

        [Fact]
        public void ShouldUpdateCompany()
        {
            Setup();

            var response = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken).Result;

            response.Result.Should().Be(Constantes.EmpresaAlterada);
        }

        [Fact]
        public void ShouldCallRepository()
        {
            Setup();

            var response = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken).Result;

            _companyRepositoryMock.Verify(x => x.Save(It.IsAny<Company>()));
        }

        [Fact]
        public void ShouldUpdateCompanyName()
        {
            Setup();

            _ = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken);

            _companySave.Name.Should().Be(_companyUpdateCommand.Name);
        }

        [Fact]
        public void ShouldUpdateCompanyEmail()
        {
            Setup();

            _ = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken);

            _companySave.Email.Should().Be(_companyUpdateCommand.Email);
        }

        [Fact]
        public void ShouldUpdateCompanyFoundationDate()
        {
            Setup();

            _ = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken);

            _companySave.FoundationDate.Should().Be(_companyUpdateCommand.FoundationDate.ToDate());
        }

        [Fact]
        public void NotShouldUpdateCompanyCnpj()
        {
            Setup();
            _companyUpdateCommand.Cnpj = _faker.Company.Cnpj();

            _ = _companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken);

            _companySave.Cnpj.Should().NotBe(_companyUpdateCommand.Cnpj);
        }

        [Fact]
        public void ShouldReturnErrorWhenCompanyNotFound()
        {
            Setup();
            _companyRepositoryMock.Setup(x => x.GetByCnpj(_companyUpdateCommand.Cnpj)).Returns<Company>(null);

           var response =_companyUpdateHandler.Handle(_companyUpdateCommand, _cancellationToken).Result;

            response.Errors.Should().Contain(Constantes.EmpresaNaoEncontrada);
        }

        [Fact]
        public void ShouldThrowExceptionWhenRequestNull()
        {
            Setup();
            _companyUpdateCommand = null;

            var response = _companyUpdateHandler.Invoking(x => x.Handle(_companyUpdateCommand, _cancellationToken));

            response.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
