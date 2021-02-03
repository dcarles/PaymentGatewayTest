using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using PaymentGateway.Api.Authentication;
using PaymentGateway.Data;
using PaymentGateway.Data.Entities;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Api.Tests
{
    public class MerchantAuthenticationMiddlewareTests
    {
        public static GatewayDbContext GetGatewayContext()
        {
            var options = new DbContextOptionsBuilder<GatewayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new GatewayDbContext(options);

            return context;
        }

        [Fact]
        public async Task Request_UrlIsNotApiUrl_CallsNextDelegate()
        {
            // Arrange
            var nextDelegateCalled = false;
            var gatewayDbContextMock = GetGatewayContext();
            var merchantRepositoryMock = new Mock<Repository<Merchant>>(gatewayDbContextMock);
            var middleware = new MerchantAuthenticationMiddleware(
                next: (innerHttpContext) =>
                {
                    nextDelegateCalled = true;
                    return Task.CompletedTask;
                }
            );
            // HttpContext mock
            var context = new DefaultHttpContext();
            context.Request.Path = "/payments";
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context, merchantRepositoryMock.Object);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = new StreamReader(context.Response.Body).ReadToEnd();

            //Assert
            Assert.True(nextDelegateCalled);
        }

        [Fact]
        public async Task Request_MerchantApiKeyHeaderIsNotPresent_ReturnsUnauthorizedStatusCode()
        {
            // Arrange
            var nextDelegateCalled = false;
            var gatewayDbContextMock = GetGatewayContext();
            var merchantRepositoryMock = new Mock<Repository<Merchant>>(gatewayDbContextMock);
            var middleware = new MerchantAuthenticationMiddleware(
                next: (innerHttpContext) =>
                {
                    nextDelegateCalled = true;
                    return Task.CompletedTask;
                }
            );
            // HttpContext mock
            var context = new DefaultHttpContext();
            context.Request.Path = "/api";
            context.Response.Body = new MemoryStream();
            //Act
            await middleware.Invoke(context, merchantRepositoryMock.Object);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = new StreamReader(context.Response.Body).ReadToEnd();

            //Assert
            Assert.False(nextDelegateCalled);
            Assert.Equal((int)HttpStatusCode.Unauthorized, context.Response.StatusCode);
            Assert.Equal("Unauthorized access to api. Merchant Api key is not present", body);
        }

        [Fact]
        public async Task Request_MerchantExistsForGivenApiKey_CallsNextDelegate()
        {
            // Arrange
            var nextDelegateCalled = false;
            var gatewayDbContextMock = GetGatewayContext();
            var merchantRepositoryMock = new Mock<Repository<Merchant>>(gatewayDbContextMock);
            var mockMerchant = new Merchant() { MerchantId = 1 };
            merchantRepositoryMock.Setup(moq => moq.GetSingleByQueryAsync(It.IsAny<Expression<Func<Merchant, bool>>>()))
                .ReturnsAsync(mockMerchant);
            var middleware = new MerchantAuthenticationMiddleware(
                next: (innerHttpContext) =>
                {
                    nextDelegateCalled = true;
                    return Task.CompletedTask;
                }
            );
            // HttpContext mock
            var context = new DefaultHttpContext();
            context.Request.Path = "/api";
            context.Response.Body = new MemoryStream();
            context.Request.Headers.Add("MerchantApiKey", "test_key");
            //Act
            await middleware.Invoke(context, merchantRepositoryMock.Object);

            //Assert
            Assert.Equal("1", context.User.Identity.Name);
            Assert.True(nextDelegateCalled);

        }
    }
}