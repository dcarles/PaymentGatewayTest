using System;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Merchant>().HasData(
                new Merchant
                {
                    MerchantId = 1,
                    ApiKey = "testMerchant1Key3264",
                    EmailAddress = "danielcarles@gmail.com"
                }, new Merchant
                {
                    MerchantId = 2,
                    ApiKey = "testMerchant2Key007",
                    EmailAddress = "daniel.carles@gmail.com"
                }
            );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    CardNumber = "123451234456123456",
                    Amount = 10.999m,
                    Currency = "EUR",
                    Cvv = "123",
                    ExpiryMonth = 11,
                    ExpiryYear = 2021,
                    MerchantId = 1,
                    Status = TransactionStatus.Approved,
                    TransactionId = Guid.NewGuid(),
                    BankReferenceId = "pay_f8c6166f-a50f-447b-b33d-920a6f7bbf37"
                }
            );


        }
    }
}