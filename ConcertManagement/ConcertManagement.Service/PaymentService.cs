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
    }
}
