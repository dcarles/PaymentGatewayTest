using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data;
using System;

namespace PaymentGateway.PaymentsCore.Tests.Helpers
{
    public class GatewayDbContextHelper
    {
        public static GatewayDbContext GetGatewayContext()
        {
            var options = new DbContextOptionsBuilder<GatewayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new GatewayDbContext(options);

            return context;
        }
    }
}
