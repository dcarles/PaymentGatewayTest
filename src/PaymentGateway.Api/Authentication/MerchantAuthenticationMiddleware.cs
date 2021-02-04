using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PaymentGateway.Data;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Api.Authentication
{
    public class MerchantAuthenticationMiddleware
    {
        public MerchantAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        private readonly RequestDelegate _next;

        private string _merchantApiKeyHeader = "MerchantApiKey";
        public async Task Invoke(HttpContext context, IRepository<Merchant> merchantRepository)
        {
            if (context.Request.Path.StartsWithSegments(new PathString("/api")))
            {
                //Let's check if it has an api key in header
                if (context.Request.Headers.Keys.Contains(_merchantApiKeyHeader, StringComparer.InvariantCultureIgnoreCase))
                {
                    // Check that the API key is valid
                    var headerKey = context.Request.Headers[_merchantApiKeyHeader].FirstOrDefault();
                    await CheckMerchantApiKey(merchantRepository, context, _next, headerKey);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Unauthorized access to api. Merchant Api key is not present");
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }


        private async Task CheckMerchantApiKey(IRepository<Merchant> merchantRepository, HttpContext context, RequestDelegate next, string key)
        {
            // find Merchant by key
            var merchant = await merchantRepository.GetSingleByQueryAsync(m => m.ApiKey == key);
            
            if (merchant == null) //  key doesn't exists
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid Merchant API Key");
            }
            else
            {
                var identity = new GenericIdentity(merchant.MerchantId.ToString());
                var principal = new GenericPrincipal(identity, new[] { "Merchant" });
                context.User = principal;
                await next(context);
            }
        }
    }

}
