using Application.Core.Validators;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Test.Validators
{
    public class DateValidatorTest
    {
        private readonly Faker _faker;

        public DateValidatorTest()
        {
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void ShouldReturnValid()
        {
            var validDate = _faker.Date.Past(30).ToShortDateString();

            var result = DateValidator.Valid(validDate);

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnInvalidWhenFutureDate()
        {
            var invalidDate = _faker.Date.Future().ToShortDateString();

            var result = DateValidator.Valid(invalidDate);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("invalidDate")]
        [InlineData("00/00/0000")]
        [InlineData("40/01/2000")]
        [InlineData("10/35/2000")]
        [InlineData("65465467987984")]
        public void ShouldReturnInvalidWhenInvalidFormat(string invalidDate)
        {
            var result = DateValidator.Valid(invalidDate);

            result.Should().BeFalse();
        }
    }
}