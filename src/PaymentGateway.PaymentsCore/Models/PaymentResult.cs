namespace PaymentGateway.PaymentsCore
{
    public class PaymentResult
    {
        public PaymentResult(string gatewayReferenceId, string responseCode)
        {
            GatewayReferenceId = gatewayReferenceId;
            ResponseCode = responseCode;
        }
        public string GatewayReferenceId { get; set; }
        public string ResponseCode { get; set; }
        public class Success : PaymentResult
        {
            public Success(string gatewayReferenceId, string responseCode) : base(gatewayReferenceId, responseCode)
            {
            }
        }
        public class Declined : PaymentResult
        {
            public string ResponseMessage { get; set; }

            public Declined(string gatewayReferenceId, string responseCode) : base(gatewayReferenceId, responseCode)
            {
            }
        }
        public class Failed : PaymentResult
        {
            public string ResponseMessage { get; set; }
            public Failed(string gatewayReferenceId, string responseCode) : base(gatewayReferenceId, responseCode)
            {
            }
        }
    }

}
