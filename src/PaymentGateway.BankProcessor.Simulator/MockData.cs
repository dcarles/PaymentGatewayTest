using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway.BankProcessor.Simulator
{
    public static class MockData
    {
        private static readonly List<Card> Cards =
            new List<Card>()
            {
                // Visa Valid
                new Card(){CardNumber = "4234123412341234", Cvv = "100", ResponseCode = "1000"},
                new Card(){CardNumber = "4543474002249996", Cvv = "956", ResponseCode = "1000"},
                // Visa Invalid
                new Card(){CardNumber = "4012888888881881", Cvv = "234", ResponseCode = "2000", ResponseMessage = "Insufficient funds"},
                new Card(){CardNumber = "4242424242424241", Cvv = "100", ResponseCode = "2000", ResponseMessage = "Security violation"},
                // Amex Valid
                new Card(){CardNumber = "345377901232564", Cvv = "1051", ResponseCode = "1000"},
                new Card(){CardNumber = "378282246310005", Cvv = "4000", ResponseCode = "1000"},
                // Amex Invalid
                new Card(){CardNumber = "371449635398431", Cvv = "2123", ResponseCode = "2000", ResponseMessage = "Declined - Do not honour"},
                new Card(){CardNumber = "34343434343434", Cvv = "7890", ResponseCode = "2000", ResponseMessage = "Gateway reject - Card number blacklisted"},
                
                // Master Valid
                new Card(){CardNumber = "5436031030606378", Cvv = "257", ResponseCode = "1000"},
                new Card(){CardNumber = "5223000010479399", Cvv = "289", ResponseCode = "1000"},
                // Master Invalid
                new Card(){CardNumber = "5199992312641465", Cvv = "019", ResponseCode = "2000", ResponseMessage = "Restricted card"},
                new Card(){CardNumber = "5353535353535353", Cvv = "123", ResponseCode = "2000", ResponseMessage = "Issuer initiated a stop payment (revocation order) for this authorization"},

            };

        public static CardResponse ValidateCard(string cardNumber, string cvv)
        {
            var card = Cards.FirstOrDefault(c => c.CardNumber == cardNumber && c.Cvv == cvv);
            if (card == null)
                return new CardResponse("2000", "Invalid card number");
            return new CardResponse(card.ResponseCode, card.ResponseMessage);
        }
    }

    public class Card
    {
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class CardResponse
    {
        public CardResponse(string responseCode, string responseMessage)
        {
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
        }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }

    }

}
