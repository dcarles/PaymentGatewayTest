using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PaymentGateway.Api.Authentication;
using PaymentGateway.Api.Core;
using PaymentGateway.BankProcessor;
using PaymentGateway.BankProcessor.Helpers;
using PaymentGateway.Data;
using PaymentGateway.PaymentsCore;
using PaymentGateway.PaymentsCore.Handlers;

namespace PaymentGateway.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters(serializerOptions =>
                {
                    serializerOptions.NullValueHandling = NullValueHandling.Ignore;
                    serializerOptions.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            // Db Context and Repositories registration

            services.AddDbContext<GatewayDbContext>(c =>
                c.UseSqlServer(_configuration.GetConnectionString("GatewayDbConnectionString")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            // Register Mediator
            services.AddMediatR(typeof(CardPaymentHandler).Assembly);
            // Add AutoMapper
            services.AddAutoMapper(typeof(ApiMappingProfile), typeof(PaymentsMappingProfile),
                typeof(BankProcessorMappingProfile));

            // Transaction Processor Service Registrations
            services.AddTransient<IPaymentProcessor, PaymentProcessor>();
            services.AddTransient<IPaymentClient, PaymentClient>();
            // Http Client Registration
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<MerchantAuthenticationMiddleware>();
            app.UseMvc();
        }
    }
}