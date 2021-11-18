using Application.Interfaces.Services;
using Application.UseCases;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.UseCases
{
    public class CompanyReceiverUseCaseTest
    {
        private CompanyReceiverUseCase _receiver;
        private readonly Mock<IRabbitService> _rabbitServiceMock;

        public CompanyReceiverUseCaseTest()
        {
            _rabbitServiceMock = new Mock<IRabbitService>();
        }

        [Fact]
        public void ShouldCallRabbit()
        {
            _receiver = new CompanyReceiverUseCase(_rabbitServiceMock.Object);

            _receiver.Execute();

            _rabbitServiceMock.Verify(x => x.Read());
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
