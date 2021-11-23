using AutoFixture;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Test.Entities
{
    public class CompanyTest
    {
        private readonly Fixture _builder;
        private readonly Faker _faker;

        public CompanyTest()
        {
            _builder = new Fixture();
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void ShouldCreateCompany()
        {
            var model = _builder.Build<Company>().Without(x => x.Id).Create();

            var company = new Company(model.Name, model.Cnpj, model.Email, model.FoundationDate);

            company.Should().BeEquivalentTo(model);
        }

        [Fact]
        public void ShouldUpdateCompanyName()
        {
            var name = _faker.Person.FullName;
            var company = _builder.Create<Company>();

            company.UpdateName(name);

            company.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NotShouldUpdateCompanyNameWhenInvalidName(string invalidName)
        {
            var company = _builder.Create<Company>();
            var originalName = company.Name;

            company.UpdateName(invalidName);

            company.Name.Should().NotBe(invalidName);
            company.Name.Should().Be(originalName);
        }

        [Fact]
        public void ShouldUpdateCompanyEmail()
        {
            var email = _faker.Person.Email;
            var company = _builder.Create<Company>();

            company.UpdateEmail(email);

            company.Email.Should().Be(email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NotShouldUpdateCompanyEmailWhenInvalidEmail(string invalidEmail)
        {
            var company = _builder.Build<Company>().Create();
            var originalEmail = company.Email;

            company.UpdateEmail(invalidEmail);

            company.Email.Should().NotBe(invalidEmail);
            company.Email.Should().Be(originalEmail);
        }

        [Fact]
        public void ShouldUpdateCompanyFoundationDate()
        {
            var date = _faker.Date.Past();
            var company = _builder.Create<Company>();

            company.UpdateFoundationDate(date);

            company.FoundationDate.Should().Be(date);
        }

        [Fact]
        public void NotShouldUpdateCompanyFoundationDateWhenNullDate()
        {
            var company = _builder.Create<Company>();
            var originalDate = company.FoundationDate;
            DateTime? invalidDate = null;

            company.UpdateFoundationDate(invalidDate);

            company.FoundationDate.Should().NotBe(invalidDate);
            company.FoundationDate.Should().Be(originalDate);
        }
    }
}