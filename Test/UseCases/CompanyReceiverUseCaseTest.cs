using Application.Interfaces.Services;
using Application.UseCases;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Test.UseCases
{
    public class CompanyReceiverUseCaseTest
    {
        private CompanyReceiverUseCase _receiver;
        private readonly Mock<IRabbitService> _rabbitServiceMock;

        public CompanyReceiverUseCaseTest()
        {
            _rabbitServiceMock = new Mock<IRabbitService>(MockBehavior.Strict);
        }

        [Fact]
        public void ShouldCallRabbit()
        {
            _rabbitServiceMock.Setup(x => x.Read());
            _receiver = new CompanyReceiverUseCase(_rabbitServiceMock.Object);

            _receiver.Execute();

            _rabbitServiceMock.VerifyAll();
        }

        [Fact]
        public void ShouldThrowExceptionWhenRequestNull()
        {
            _receiver = new CompanyReceiverUseCase(null);

            var response = _receiver.Invoking(x => x.Execute());

            response.Should().Throw<ArgumentNullException>();
        }
    }
}