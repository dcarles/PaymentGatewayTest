using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentGateway.BankProcessor.Models;

namespace PaymentGateway.BankProcessor
{
    /// <summary>
    /// Defined the client responsible interacting with Acquiring bank's api
    /// </summary>
    public class PaymentClient : IPaymentClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _acquiringBankEndpoint;

        public PaymentClient(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _acquiringBankEndpoint = $"{configuration["EndPoints:AcquiringBankEndPoint"]}/Payments";
        }

        /// <summary>
        /// Requests payment from acquiring bank and generate client payment processing response based on Http Response
        /// </summary>
        /// <param name="bankPaymentRequest"></param>
        /// <returns></returns>
        public async Task<BankPaymentResponse> RequestAsync(BankPaymentRequest bankPaymentRequest)
        {
            if (bankPaymentRequest == null) throw new ArgumentNullException(nameof(bankPaymentRequest));

            var request = new HttpRequestMessage(HttpMethod.Post, _acquiringBankEndpoint)
            {
                Content = new StringContent(SerializeBankPaymentRequest(bankPaymentRequest), Encoding.UTF8,
                    "application/json")
            };
            request.Headers.Add("Accept", "application/json");

            // Authentication codes will be passed here

            // Make the call!
            using (var client = _clientFactory.CreateClient())
            {
                var httpResponse = await client.SendAsync(request);
                var bankPaymentResponse = await DeserializeBankResponseMessage(httpResponse);
                return bankPaymentResponse;
            }
        }

        private static async Task<BankPaymentResponse> DeserializeBankResponseMessage(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BankPaymentResponse>(jsonString);
        }

        private static string SerializeBankPaymentRequest(BankPaymentRequest request)
        {
            return JsonConvert.SerializeObject(request);
        }
    }
}