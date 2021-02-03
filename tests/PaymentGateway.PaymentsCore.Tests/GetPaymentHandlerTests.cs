using AutoMapper;
using Moq;
using PaymentGateway.Data;
using PaymentGateway.Data.Entities;
using PaymentGateway.PaymentsCore.Handlers;
using PaymentGateway.PaymentsCore.Tests.Helpers;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.PaymentsCore.Tests
{
    public class GetPaymentHandlerTests
    {

        [Fact]
        public async Task HandleAsync_NullGetPaymentCommandPassed_ReturnsArgumentNullException()
        {
            // Arrange
            var gatewayDbContextMock = GatewayDbContextHelper.GetGatewayContext();
            var transactionRepositoryMock = new Mock<Repository<Data.Entities.Transaction>>(gatewayDbContextMock);
            var mapperMock = new Mock<IMapper>();

            var getPaymentDetailsCommandHandlerUnderTest =
                new GetPaymentHandler(transactionRepositoryMock.Object, mapperMock.Object);

            // Act 
            var exceptionThrown = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await getPaymentDetailsCommandHandlerUnderTest.Handle(null, CancellationToken.None));


            // Assert
            Assert.IsType<ArgumentNullException>(exceptionThrown);
        }

        [Fact]
        public async Task HandleAsync_PaymentDoesNotExist_ReturnsNull()
        {
            // Arrange
            var gatewayDbContextMock = GatewayDbContextHelper.GetGatewayContext();
            var transactionRepositoryMock = new Mock<Repository<Data.Entities.Transaction>>(gatewayDbContextMock);

            transactionRepositoryMock.Setup(moq => moq.GetSingleByQueryAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
                .ReturnsAsync((Transaction)null);
            var mapperMock = new Mock<IMapper>();

            var getPaymentHandlerUnderTest =
                new GetPaymentHandler(transactionRepositoryMock.Object, mapperMock.Object);

            // Act 
            var transaction = await getPaymentHandlerUnderTest.Handle(new GetPayment(Guid.NewGuid(), 1), CancellationToken.None);


            // Assert
            Assert.Null(transaction);
        }

        [Fact]
        public async Task HandleAsync_PaymentShouldBelongToRequestingMerchant_ReturnsPayment()
        {
            // Arrange
            var gatewayDbContextMock = GatewayDbContextHelper.GetGatewayContext();
            var transactionRepositoryMock = new Mock<Repository<Data.Entities.Transaction>>(gatewayDbContextMock);
            transactionRepositoryMock.Setup(moq => moq.GetSingleByQueryAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
                .ReturnsAsync(new Transaction() { MerchantId = 1 });

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<PaymentDetails>(It.IsAny<Transaction>()))
                .Returns(new PaymentDetails() { MerchantId = 1 });

            var getPaymentHandlerUnderTest =
                new GetPaymentHandler(transactionRepositoryMock.Object, mapperMock.Object);

            // Act 
            var transaction = await getPaymentHandlerUnderTest.Handle(new GetPayment(Guid.NewGuid(), 1), CancellationToken.None);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(1,transaction.MerchantId);
        }
    }
}
