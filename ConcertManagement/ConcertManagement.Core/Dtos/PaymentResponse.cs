namespace ConcertManagement.Core.Dtos
{
    public class PaymentResponse
    {
        public string TransactionId { get; set; }
        public string PaymentStatus { get; set; }
        public decimal AmountPaid { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

        public PaymentResponse(string transactionId,
                               string paymentStatus,
                               decimal amountPaid,
                               string currency,
                               string paymentMethod,
                               DateTime paymentDate,
                               bool isSuccessful,
                               string message)
        {
            TransactionId = transactionId;
            PaymentStatus = paymentStatus;
            AmountPaid = amountPaid;
            Currency = currency;
            PaymentMethod = paymentMethod;
            PaymentDate = paymentDate;
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }

}