using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using PaymentGateway.BankProcessor;
using PaymentGateway.BankProcessor.Models;
using Xunit;

namespace PaymentGateway.BankProcessor.Tests
{
    public class PaymentProcessorTests
    {
        [Fact]
        public async Task ProcessProcessAsync_NullCreatePaymentCommandPassed_ReturnsArgumentNullException()
        {
            // Arrange
            var paymentClientMock = new Mock<IPaymentClient>();
            var mapperMock = new Mock<IMapper>();
            var paymentProcessorUnderTest = new PaymentProcessor(paymentClientMock.Object, mapperMock.Object);

            // Act 
            var exceptionThrown = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await paymentProcessorUnderTest.ProcessAsync(null));


            // Assert
            Assert.IsType<ArgumentNullException>(exceptionThrown);
        }

        // refactor, check this is not sensible
        [Fact]
        public async Task ProcessProcessAsync_ValidPaymentProcessingRequestPassedButPaymentClientReturnedNull_ReturnsArgumentNullException()
        {
            // Arrange
            var paymentClientMock = new Mock<IPaymentClient>();
            var mapperMock = new Mock<IMapper>();

            var paymentProcessorUnderTest = new PaymentProcessor(paymentClientMock.Object, mapperMock.Object);

            paymentClientMock.Setup(
                client => client.RequestAsync(It.IsAny<BankPaymentRequest>())).ReturnsAsync((BankPaymentResponse)null);

            // Act 
            var exceptionThrown = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await paymentProcessorUnderTest.ProcessAsync(new PaymentProcessingRequest()));

            // Assert
            Assert.IsType<ArgumentNullException>(exceptionThrown);
        }

        [Fact]
        public async Task ProcessProcessAsync_ValidPaymentProcessingRequestPassed_ReturnsApproved()
        {
            // Arrange
            var paymentClientMock = new Mock<IPaymentClient>();
            var mapperMock = new Mock<IMapper>();

            var paymentProcessorUnderTest = new PaymentProcessor(paymentClientMock.Object, mapperMock.Object);
            // accepted response from bank
            var bankPaymentResponse = new BankPaymentResponse() { ResponseCode = "1000" };
            // setup payment client mock to return a successful response
            paymentClientMock.Setup(
                client => client.RequestAsync(It.IsAny<BankPaymentRequest>())).ReturnsAsync(bankPaymentResponse);

            // Act 
            var bankingPaymentResponse = await paymentProcessorUnderTest.ProcessAsync(new PaymentProcessingRequest());


            // Assert
            Assert.True(bankingPaymentResponse.Approved);
        }
        [Fact]
        public async Task ProcessProcessAsync_ValidPaymentProcessingRequestPassed_ReturnsDeclined()
        {
            // Arrange
            var paymentClientMock = new Mock<IPaymentClient>();
            var mapperMock = new Mock<IMapper>();
            var paymentProcessorUnderTest = new PaymentProcessor(paymentClientMock.Object, mapperMock.Object);
            // accepted response from bank
            var bankPaymentResponse = new BankPaymentResponse() { ResponseCode = "2000" };
            // setup payment client mock to return a successful response
            paymentClientMock.Setup(
                client => client.RequestAsync(It.IsAny<BankPaymentRequest>())).ReturnsAsync(bankPaymentResponse);

            // Act 
            var bankingPaymentResponse = await paymentProcessorUnderTest.ProcessAsync(new PaymentProcessingRequest());


            // Assert
            Assert.False(bankingPaymentResponse.Approved);
        }


    }
}