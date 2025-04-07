using System.Linq.Expressions;
using AutoMapper;
using ConcertManagement.Core.Dtos;
using ConcertManagement.Core.Entities;
using ConcertManagement.Data.Repositories;
using ConcertManagement.Service;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ConcertManagement.Tests
{
    public class ConcertServiceTests
    {
        private readonly Mock<IEventsRepository> _eventsRepoMock = new();
        private readonly Mock<IReservationsRepository> _reservationsRepoMock = new();
        private readonly Mock<ITicketTypesRepository> _ticketTypesRepoMock = new();
        private readonly Mock<ITicketsRepository> _ticketsRepoMock = new();
        private readonly Mock<IPaymentService> _paymentServiceRepoMock = new();
        private readonly Mock<IVenuesRepository> _venuesRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<ConcertService>> _loggerMock = new();

        private readonly ConcertService _sut;

        public ConcertServiceTests()
        {
            _sut = new ConcertService(
                _venuesRepoMock.Object,
                _eventsRepoMock.Object,
                _ticketTypesRepoMock.Object,
                _reservationsRepoMock.Object,
                _ticketsRepoMock.Object,
                _paymentServiceRepoMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetEvent_ShouldReturnEvent_WhenEventExists()
        {
            var expected = new Event { Id = 1, Name = "Test Event" };
            _eventsRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(expected);

            var act = await _sut.GetEvent(1);

            act.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetEvent_ShouldReturnNull_WhenEventDoesNotExist()
        {
            _eventsRepoMock.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Event)null);

            var act = await _sut.GetEvent(99);

            act.Should().BeNull();
        }

        [Fact]
        public async Task CreateEvent_ShouldMapAndReturnCreatedEvent()
        {
            var dto = new EventDto { Name = "Sample Event" };
            var mappedEvent = new Event { Id = 10, Name = "Sample Event" };
            _mapperMock.Setup(m => m.Map<Event>(dto)).Returns(mappedEvent);
            _eventsRepoMock.Setup(r => r.AddAsync(mappedEvent)).ReturnsAsync(mappedEvent);

            var act = await _sut.CreateEvent(dto);

            act.Should().BeEquivalentTo(mappedEvent);
        }

        [Fact]
        public async Task UpdateEvent_UpdateMappedEvent()
        {
            var dto = new EventDto { Id = 5, Name = "Updated Name" };
            var existingEvent = new Event { Id = 5, Name = "Old Name" };
            _eventsRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existingEvent);

            Func<Task> act = async () => await _sut.UpdateEvent(dto);

            await act.Should().NotThrowAsync();
            _mapperMock.Verify(m => m.Map(dto, existingEvent), Times.Once);
            _eventsRepoMock.Verify(r => r.UpdateAsync(existingEvent), Times.Once);
        }

        [Fact]
        public async Task UpdateEvent_EventNotFound_ThrowsException()
        {
            var dto = new EventDto { Id = 999 };
            _eventsRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Event)null);

            Func<Task> act = async () => await _sut.UpdateEvent(dto);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Event not found.");
        }

        [Fact]
        public async Task GetVenues_ReturnsAllVenues()
        {
            var venues = new List<Venue> { new Venue { Id = 1, Name = "Test Venue" } };
            _venuesRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(venues);

            var act = await _sut.GetVenues();

            act.Should().BeEquivalentTo(venues);
        }

        [Fact]
        public async Task GetVenue_ReturnsVenue()
        {
            var venue = new Venue { Id = 1, Name = "Found Venue" };
            _venuesRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(venue);

            var result = await _sut.GetVenue(1);

            result.Should().Be(venue);
        }

        [Fact]
        public async Task GetVenue_ReturnsNull_VenueNotFound()
        {
            _venuesRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Venue)null);
            
            var result = await _sut.GetVenue(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateVenue_AddVenue()
        {
            var dto = new VenueDto { Id = 1, Name = "New Venue" };
            var venue = new Venue { Id = 1, Name = "New Venue" };

            _mapperMock.Setup(m => m.Map<Venue>(dto)).Returns(venue);
            _venuesRepoMock.Setup(r => r.AddAsync(venue)).ReturnsAsync(venue);

            var result = await _sut.CreateVenue(dto);

            result.Should().Be(venue);
        }

        [Fact]
        public async Task CreateVenue_ThrowsException_MappingFails()
        {
            var dto = new VenueDto { Id = 1, Name = "Bad Venue" };
            _mapperMock.Setup(m => m.Map<Venue>(dto)).Throws(new Exception("Mapping failed"));

            Func<Task> act = async () => await _sut.CreateVenue(dto);

            await act.Should().ThrowAsync<Exception>().WithMessage("Mapping failed");
        }

        [Fact]
        public async Task UpdateVenue_UpdateMappedVenue()
        {
            var dto = new VenueDto { Id = 1, Name = "Updated Venue" };
            var venue = new Venue { Id = 1, Name = "Old Venue" };

            _venuesRepoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(venue);
            _mapperMock.Setup(m => m.Map(dto, venue));

            await _sut.UpdateVenue(dto);

            _venuesRepoMock.Verify(r => r.UpdateAsync(venue), Times.Once);
        }

        [Fact]
        public async Task UpdateVenue_ThrowsException_VenueNotFound()
        {
            var dto = new VenueDto { Id = 99, Name = "Not Found Venue" };
            _venuesRepoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((Venue)null);

            Func<Task> act = async () => await _sut.UpdateVenue(dto);

            await act.Should().ThrowAsync<Exception>().WithMessage("Venue not found.");
        }

        [Fact]
        public async Task UpdateVenue_ThrowsException_MappingFails()
        {
            var dto = new VenueDto { Id = 1, Name = "New Venue" };
            var venue = new Venue { Id = 1, Name = "Old Venue" };

            _venuesRepoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(venue);
            _mapperMock.Setup(m => m.Map(dto, venue)).Throws(new Exception("Map failed"));

            Func<Task> act = async () => await _sut.UpdateVenue(dto);

            await act.Should().ThrowAsync<Exception>().WithMessage("Map failed");
        }

        [Fact]
        public async Task CreateReservation_ValidRequest()
        {
            var reservationRequest = new ReservationRequest
            {
                EventId = 1,
                TicketTypeId = 1,
                Quantity = 2,
                PurchaseDate = DateTime.UtcNow
            };

            var eventObj = new Event
            {
                Id = 1,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1),
                TicketTypes = new List<TicketType> { new TicketType { Id = 1, TotalSeats = 10, Price = 50 } }
            };

            var ticketTypeObj = new TicketType
            {
                Id = 1,
                TotalSeats = 10,
                Price = 50
            };

            var paymentResponse = new PaymentResponse(transactionId: Guid.NewGuid().ToString(),
                                                      paymentStatus: "Success",
                                                      amountPaid: 250,
                                                      currency: "USD",
                                                      paymentMethod: "CC",
                                                      paymentDate: DateTime.UtcNow,
                                                      isSuccessful: true,
                                                      message: "Payment successful");

            _eventsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<Expression<Func<Event, object>>>())).ReturnsAsync(eventObj);
            _ticketTypesRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ticketTypeObj);
            _paymentServiceRepoMock.Setup(r => r.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(paymentResponse);
            _mapperMock.Setup(m => m.Map<Reservation>(It.IsAny<ReservationRequest>())).Returns(new Reservation());
            _mapperMock.Setup(m => m.Map<ReservationDto>(It.IsAny<Reservation>())).Returns(new ReservationDto
            {
                ReservationCode = "RES123",
                IsConfirmed = true
            });
            _reservationsRepoMock.Setup(r => r.AddAsync(It.IsAny<Reservation>())).ReturnsAsync(new Reservation
            {
                ReservationCode = "RES123",
                IsConfirmed = true
            });

            var result = await _sut.CreateReservation(reservationRequest);

            result.Should().NotBeNull();
            result.ReservationCode.Should().NotBeNullOrEmpty();
            result.IsConfirmed.Should().BeTrue();
            _reservationsRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact]
        public async Task CancelReservation_ReturnTrue_WhenSuccessful()
        {
            var reservationId = 1;
            var reservation = new Reservation
            {
                Id = reservationId,
                IsConfirmed = true,
                ReservationCode = "RES123",
                EventId = 2,
                Payments = new List<Payment>{ new Payment { TransactionId = "TXN123" } },
                Tickets = new List<Ticket> { new Ticket() }
            };
            var eventObj = new Event
            {
                Id = 2,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            var paymentResponse = new PaymentResponse(transactionId: Guid.NewGuid().ToString(),
                                                      paymentStatus: "Success",
                                                      amountPaid: 250,
                                                      currency: "USD",
                                                      paymentMethod: "CC",
                                                      paymentDate: DateTime.UtcNow,
                                                      isSuccessful: true,
                                                      message: "Payment successful");

            _reservationsRepoMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync(reservation);
            _eventsRepoMock.Setup(e => e.GetByIdAsync(reservation.EventId, It.IsAny<Expression<Func<Event, object>>[]>())).ReturnsAsync(eventObj);
            _paymentServiceRepoMock.Setup(p => p.RefundPaymentAsync(It.IsAny<string>())).ReturnsAsync(paymentResponse);
            _ticketsRepoMock.Setup(t => t.RemoveAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);
            _reservationsRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var result = await _sut.CancelReservation(reservationId);

            Assert.True(result);
        }
    }
}
