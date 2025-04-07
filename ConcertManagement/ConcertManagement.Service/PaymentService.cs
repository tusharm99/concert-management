using System.Net.Http.Json;
using AutoMapper;
using ConcertManagement.Core.Dtos;
using Microsoft.Extensions.Logging;

namespace ConcertManagement.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        public PaymentService(IHttpClientFactory httpClientFactory,
                              IMapper mapper,
                              ILogger<PaymentService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("PaymentService");
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            // implement the logic to process the payment
            // simulating payment processing...
            var response = await _httpClient.PostAsJsonAsync("payments/process", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PaymentResponse>();
            }
            return null;
        }

        public async Task<PaymentResponse> RefundPaymentAsync(string transactionId)
        {
            // implement the logic to refund the payment
            // simulating payment refund...
            var response = await _httpClient.PostAsJsonAsync("payments/refund", transactionId);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PaymentResponse>();
            }
            return null;
        }
    }
}
