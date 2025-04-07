namespace ConcertManagement.Core.Dtos
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        public string Expiry { get; set; } // MM/YY
        public string Cvc { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardHolderName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingCountry { get; set; }

        public PaymentRequest(string cardNumber,
                              string expiry,
                              string cvc,
                              decimal amount,
                              string currency,
                              string cardHolderName)
        {
            CardNumber = cardNumber;
            Expiry = expiry;
            Cvc = cvc;
            Amount = amount;
            Currency = currency;
            CardHolderName = cardHolderName;
        }
    }
}