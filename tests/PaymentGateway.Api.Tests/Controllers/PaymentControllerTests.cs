using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models;
using PaymentGateway.PaymentsCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Api.Tests.Controllers
{
    public class PaymentControllerTests
    {
        #region Get

        [Fact]
        public async Task Get_PaymentDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PaymentsController>>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(med => med.Map<PaymentResponse>(It.IsAny<PaymentDetails>()))
                .Returns((PaymentResponse)null);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(med => med.Send(It.IsAny<GetPayment>(), CancellationToken.None))
                .ReturnsAsync((PaymentDetails)null);
            var paymentControllerUnderTest =
                new PaymentsController(loggerMock.Object, mediatorMock.Object, mapperMock.Object);

            //  Context Mock
            var user = new ClaimsPrincipal(new GenericIdentity("1"));
            paymentControllerUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Act
            var paymentResponse = await paymentControllerUnderTest.Get("674324A3-8AC5-4EA0-B0C9-255F793D91FF");
            // Assert
            Assert.IsType<NotFoundObjectResult>(paymentResponse);
        }

        [Fact]
        public async Task Get_PaymentExists_ReturnsOkResponse()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PaymentsController>>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(med => med.Map<PaymentResponse>(It.IsAny<PaymentDetails>()))
                .Returns(new PaymentResponse());

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(med => med.Send(It.IsAny<GetPayment>(), CancellationToken.None))
                .ReturnsAsync(new PaymentDetails());
            var paymentControllerUnderTest =
                new PaymentsController(loggerMock.Object, mediatorMock.Object, mapperMock.Object);

            //  Context Mock
            var user = new ClaimsPrincipal(new GenericIdentity("1"));
            paymentControllerUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Act
            var paymentResponse = await paymentControllerUnderTest.Get("674324A3-8AC5-4EA0-B0C9-255F793D91FF");
            // Assert
            Assert.IsType<OkObjectResult>(paymentResponse);
        }

        #endregion


        [Fact]
        public async Task Post_InvalidPaymentRequestPassed_ReturnsBadRequestResponse()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var loggerMock = new Mock<ILogger<PaymentsController>>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<CardPayment>(It.IsAny<PaymentRequest>()))
                .Returns(new CardPayment());

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(med => med.Send(It.IsAny<CardPayment>(), CancellationToken.None))
                .ReturnsAsync(new PaymentResult.Declined(transactionId.ToString(), "2000"));

            var paymentControllerUnderTest =
                new PaymentsController(loggerMock.Object, mediatorMock.Object, mapperMock.Object);

            //  Context Mock
            var user = new ClaimsPrincipal(new GenericIdentity("1"));
            paymentControllerUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };


            // Act
            var response = await paymentControllerUnderTest.Post(new PaymentRequest());

            // Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Post_ValidPaymentRequestPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var loggerMock = new Mock<ILogger<PaymentsController>>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<CardPayment>(It.IsAny<PaymentRequest>()))
                .Returns(new CardPayment());

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(med => med.Send(It.IsAny<CardPayment>(), CancellationToken.None))
                .ReturnsAsync(new PaymentResult.Success(transactionId.ToString(), "1000"));

            var paymentControllerUnderTest =
                new PaymentsController(loggerMock.Object, mediatorMock.Object, mapperMock.Object);

            //  Context Mock
            var user = new ClaimsPrincipal(new GenericIdentity("1"));
            paymentControllerUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var response = await paymentControllerUnderTest.Post(new PaymentRequest());

            // Assert

            Assert.NotNull(response);
            Assert.IsType<CreatedResult>(response);
        }
    }
}