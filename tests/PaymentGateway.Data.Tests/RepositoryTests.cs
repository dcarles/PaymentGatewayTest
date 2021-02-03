using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Data.Tests
{
    public class RepositoryTests
    {


        [Fact()]
        public async Task AddAsync_ValidEntityPassed_CreatesNewEntity()
        {
            // Arrange 
            var options = new DbContextOptionsBuilder<GatewayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var transactionId = Guid.NewGuid();

            using (var dbContext = new GatewayDbContext(options))
            {
                var merchantRepository = new Repository<Merchant>(dbContext);
                var merchant = new Merchant
                {
                    MerchantId = 1,
                    ApiKey = "testMerchant1Key3264",
                    EmailAddress = "danielcarles@gmail.com"
                };
                await merchantRepository.AddAsync(merchant);
            }

            // Act

            using (var dbContext = new GatewayDbContext(options))
            {
                var transactionRepositoryUnderTest = new Repository<Transaction>(dbContext);

                var transaction = new Transaction
                {
                    TransactionId = transactionId,
                    ExpiryMonth = 4,
                    ExpiryYear = 2022,
                    Amount = 10.99m,
                    CardNumber = "2244 5566 7788 9977",
                    Cvv = "123",
                    Status = TransactionStatus.Approved,
                    Currency = "EUR"
                };
                await transactionRepositoryUnderTest.AddAsync(transaction);
            }


            // Assert
            using (var dbContext = new GatewayDbContext(options))
            {
                var transaction = dbContext.Transactions.Find(transactionId);
                Assert.NotNull(transaction);
                Assert.Equal(10.99m, transaction.Amount);
                Assert.Equal("123", transaction.Cvv);
                Assert.Equal(10.99m, transaction.Amount);
            }
        }

        [Fact()]
        public async Task UpdateAsync_ValidEntityPassed_UpdatesExistingEntity()
        {
            // Arrange 
            var options = new DbContextOptionsBuilder<GatewayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new GatewayDbContext(options))
            {
                var merchantRepository = new Repository<Merchant>(dbContext);
                var merchant = new Merchant
                {
                    MerchantId = 1,
                    ApiKey = "testMerchant1Key3264",
                    EmailAddress = "danielcarles@gmail.com"
                };
                await merchantRepository.AddAsync(merchant);
            }

            // Act

            using (var dbContext = new GatewayDbContext(options))
            {
                var merchantRepository = new Repository<Merchant>(dbContext);
                // Get Merchant to update
                var merchant = await dbContext.Merchants.FindAsync(1);
                merchant.ApiKey = "updated-key123";
                await merchantRepository.UpdateAsync(merchant);
            }


            // Assert
            using (var dbContext = new GatewayDbContext(options))
            {
                var merchant = dbContext.Merchants.Find(1);
                Assert.Equal("updated-key123", merchant.ApiKey);
                //make sure that other fields are not affected
                Assert.Equal("danielcarles@gmail.com", merchant.EmailAddress);
            }
        }


        [Fact]
        public async Task GetByIdAsync_ValidIdPassed_ReturnsFoundEntity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GatewayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var transactionId = Guid.NewGuid();

            using (var dbContext = new GatewayDbContext(options))
            {
                var transactionRepositoryUnderTest = new Repository<Transaction>(dbContext);

                var transaction = new Transaction
                {
                    TransactionId = transactionId,
                    ExpiryMonth = 4,
                    ExpiryYear = 2022,
                    Amount = 10.99m,
                    CardNumber = "2244 5566 7788 9977",
                    Cvv = "123",
                    Status = TransactionStatus.Approved,
                    Currency = "EUR"
                };
                await transactionRepositoryUnderTest.AddAsync(transaction);
                await dbContext.SaveChangesAsync();
            }

            // Act
            Transaction foundTransaction;
            using (var dbContext = new GatewayDbContext(options))
            {
                var transactionRepositoryUnderTest = new Repository<Transaction>(dbContext);
                foundTransaction = await transactionRepositoryUnderTest.GetByIdAsync(transactionId);
            }

            // Assert
            Assert.NotNull(foundTransaction);
            Assert.Equal(transactionId, foundTransaction.TransactionId);
            Assert.Equal("2244 5566 7788 9977", foundTransaction.CardNumber);
            Assert.Equal("123", foundTransaction.Cvv);
        }

        [Fact]
        public async Task GetByIdAsync_EntityDoesNotExist_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GatewayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var transactionId = Guid.NewGuid();
            // Act
            Transaction foundTransaction;
            using (var dbContext = new GatewayDbContext(options))
            {
                var transactionRepositoryUnderTest = new Repository<Transaction>(dbContext);
                foundTransaction = await transactionRepositoryUnderTest.GetByIdAsync(transactionId);
            }

            // Assert
            Assert.Null(foundTransaction);
        }



    }
}