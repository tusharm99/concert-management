﻿using ConcertManagement.Core.Dtos;

namespace ConcertManagement.Service
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        Task<PaymentResponse> RefundPaymentAsync(string transactionId);
    }
}