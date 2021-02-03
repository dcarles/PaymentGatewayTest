using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using PaymentGateway.BankProcessor;
using PaymentGateway.BankProcessor.Models;
using PaymentGateway.BankProcessor.Tests.Helpers;
using Xunit;

namespace PaymentGateway.BankProcessor.Tests
{
    public class PaymentClientTests
    {
        [Fact]
        public async Task RequestAsync_NullBankPaymentRequestPassed_ReturnsArgumentNullException()
        {
            // Arrange
            var clientFactoryMock = new Mock<IHttpClientFactory>();
            var configurationMock = new Mock<IConfiguration>();
            var paymentClientUnderTest = new PaymentClient(clientFactoryMock.Object, configurationMock.Object);

            // Act 
            var exceptionThrown = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await paymentClientUnderTest.RequestAsync(null));


            // Assert
            Assert.IsType<ArgumentNullException>(exceptionThrown);
        }

        [Fact]
        public async Task RequestAsync_ValidBankPaymentRequestPassed_ReturnsBankPaymentResponse()
        {
            // Arrange
            var clientFactoryMock = new Mock<IHttpClientFactory>();
            var configurationMock = new Mock<IConfiguration>();
            // setup config mock
            configurationMock.SetupGet(config =>
                config[It.Is<string>(s => s == "EndPoints:AcquiringBankEndPoint")]).Returns("https://api.danthebank.com");

            var paymentClientUnderTest = new PaymentClient(clientFactoryMock.Object, configurationMock.Object);

            // Fake HttpClient
            var fakeBankPaymentResponse = new BankPaymentResponse()
            {
                ReferenceId = "pay_b3b847a2-aa14-4b6f-aba4-b10734818b5c",
                ResponseCode = "1000",
                ResponseMessage = "Payment Processed"
            };

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(JsonConvert.SerializeObject(fakeBankPaymentResponse), Encoding.UTF8,
                    "application/json")
            });

            var faceHttpClient = new HttpClient(fakeHttpMessageHandler);

            clientFactoryMock.Setup(clientFactory =>
                clientFactory.CreateClient(It.IsAny<string>())).Returns(faceHttpClient);

            // Act 
            var bankingPaymentResponse = await paymentClientUnderTest.RequestAsync(new BankPaymentRequest());


            // Assert
            Assert.NotNull(bankingPaymentResponse);
            Assert.Equal("pay_b3b847a2-aa14-4b6f-aba4-b10734818b5c", bankingPaymentResponse.ReferenceId);
            Assert.Equal("1000", bankingPaymentResponse.ResponseCode);
            Assert.Equal("Payment Processed", bankingPaymentResponse.ResponseMessage);
        }
    }
}