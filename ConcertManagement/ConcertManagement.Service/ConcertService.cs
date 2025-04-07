using AutoMapper;
using ConcertManagement.Core.Dtos;
using ConcertManagement.Core.Entities;
using ConcertManagement.Data.Repositories;
using ConcertManagement.Infrastructure;
using Microsoft.Extensions.Logging;

namespace ConcertManagement.Service
{
    public class ConcertService : IConcertService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly IVenuesRepository _venuesRepository;
        private readonly ITicketTypesRepository _ticketTypesRepository;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<ConcertService> _logger;

        public ConcertService(IVenuesRepository venuesRepository,
                              IEventsRepository eventsRepository,
                              ITicketTypesRepository ticketTypesRepository,
                              IReservationsRepository reservationsRepository,
                              ITicketsRepository ticketsRepository,
                              IPaymentService paymentService,
                              IMapper mapper,
                              ILogger<ConcertService> logger)
        {
            _eventsRepository = eventsRepository;
            _reservationsRepository = reservationsRepository;
            _venuesRepository = venuesRepository;
            _ticketTypesRepository = ticketTypesRepository;
            _ticketsRepository = ticketsRepository;
            _paymentService = paymentService;
            _mapper = mapper;
            _logger = logger;
        }

        #region Events
        public async Task<IEnumerable<Event>> GetEvents()
        {
            return await _eventsRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<Event> GetEvent(int id, bool includeTicketTypes = false)
        {
            if (includeTicketTypes)
            {
                return await _eventsRepository
                    .GetByIdAsync(id, e => e.TicketTypes)
                    .ConfigureAwait(false);
            }

            return await _eventsRepository
                .GetByIdAsync(id)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Event>> GetEventsByVenue(int venueId)
        {
            return await _eventsRepository.FindAsync(_ => _.Venue.Id == venueId).ConfigureAwait(false);
        }

        public async Task<Event> CreateEvent(EventDto item)
        {
            try
            {
                Event eventObj = _mapper.Map<Event>(item);
                return await _eventsRepository.AddAsync(eventObj).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Automapper Error: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateEvent(EventDto item)
        {
            var existingEvent = await GetEvent(item.Id);

            if (existingEvent == null)
            {
                throw new Exception("Event not found.");
            }

            try
            {
                _mapper.Map(item, existingEvent);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Automapper Error: {ex.Message}");
                throw;
            }

            await _eventsRepository.UpdateAsync(existingEvent).ConfigureAwait(false);
        }
        #endregion

        #region Venues
        public async Task<IEnumerable<Venue>> GetVenues()
        {
            return await _venuesRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<Venue> GetVenue(int id)
        {
            return await _venuesRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<Venue> CreateVenue(VenueDto item)
        {
            Venue venueObj = null;
            try
            {
                venueObj = _mapper.Map<Venue>(item);
                return await _venuesRepository.AddAsync(venueObj).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Automapper Error: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateVenue(VenueDto item)
        {
            var existingVenue = await GetVenue(item.Id);

            if (existingVenue == null)
            {
                throw new Exception("Venue not found.");
            }

            try
            {
                _mapper.Map(item, existingVenue);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Automapper Error: {ex.Message}");
                throw;
            }

            await _venuesRepository.UpdateAsync(existingVenue).ConfigureAwait(false);
        }

        public async Task<TicketType> CreateTicketType(int eventId, TicketTypeDto item)
        {
            TicketType ticketTypeObj = null;
            try
            {
                ticketTypeObj = _mapper.Map<TicketType>(item);
                return await _ticketTypesRepository.AddAsync(ticketTypeObj).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Automapper Error: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Reservation

        public async Task<Reservation> GetReservation(int id, bool includeTickets = false)
        {
            if (includeTickets)
            {
                return await _reservationsRepository
                    .GetByIdAsync(id, e => e.Tickets)
                    .ConfigureAwait(false);
            }

            return await _reservationsRepository
                .GetByIdAsync(id)
                .ConfigureAwait(false);
        }


        /// <summary>
        /// Creates reservation for an event which defines the time window
        /// also purchase tickets for the event via payment service
        /// and reserve the tickets for the event once payment is approved 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ReservationDto> CreateReservation(ReservationRequest item)
        {
            try
            {
                Reservation reservationObj = _mapper.Map<Reservation>(item);
                reservationObj.ReservationCode = EventUtil.GenerateCode("RES", 5);

                // get event details to fetch event date
                var eventObj = await _eventsRepository.GetByIdAsync(item.EventId, e => e.TicketTypes).ConfigureAwait(false);
                if (eventObj == null)
                {
                    _logger.LogError($"Event not found for ID: {item.EventId}");
                    throw new Exception("Event not found.");
                }

                if (item.PurchaseDate < eventObj.StartDate || item.PurchaseDate > eventObj.EndDate)
                {
                    // purchase date is not within the event window
                    _logger.LogError($"Purchase date is not within the event window for Event ID: {item.EventId} and ReservationCode: {reservationObj.ReservationCode}");
                    throw new Exception("Purchase date is not within the event window");
                }

                // get ticket type details to fetch quantity and price
                var ticketTypeObj = await _ticketTypesRepository.GetByIdAsync(item.TicketTypeId).ConfigureAwait(false);
                if (ticketTypeObj == null)
                {
                    _logger.LogError($"Ticket Type not found for ID: {item.TicketTypeId}");
                    throw new Exception("Ticket Type not found.");
                }

                // check if total seats for this event and ticket type are within the quantity requested for this reservation
                if (ticketTypeObj.TotalSeats < item.Quantity)
                {
                    _logger.LogError($"Requested seats exceed the total availability for this ticket type. Type ID: {item.TicketTypeId}");
                    throw new Exception("Requested seats exceed the total availability for this ticket type.");
                }

                // fetch total quantity of confirmed reservations for this event and ticket type
                var totalConfirmedReservations = await _reservationsRepository
                    .FindAsync(r => r.Event.Id == eventObj.Id && r.TicketType.Id == ticketTypeObj.Id && r.IsConfirmed)
                    .ConfigureAwait(false);

                // check if total seats of confirmed reservations for this event and ticket type are within the quantity requested for this reservation
                var reservedSeats = totalConfirmedReservations.Sum(r => r.Quantity);
                if (ticketTypeObj.TotalSeats < item.Quantity + reservedSeats)
                {
                    _logger.LogError($"Seats are unavailable for this ticket type. Type ID: {item.TicketTypeId} | Exceeded Limit: {(item.Quantity + reservedSeats) - ticketTypeObj.TotalSeats}");
                    throw new Exception("Seats are unavailable for this ticket type.");
                }

                var totalAmount = ticketTypeObj.Price * item.Quantity;

                // make payment request
                var paymentResponse = await _paymentService.ProcessPaymentAsync(new PaymentRequest(cardNumber: "1234567890123456", expiry: "06/28", cvc: "123", amount: totalAmount, currency: "USD", cardHolderName: "John Doe"));

                if (!paymentResponse.IsSuccessful)
                {
                    reservationObj.IsConfirmed = false;
                    _logger.LogError($"Payment failed: {paymentResponse.Message} | ReservationCode: {reservationObj.ReservationCode}");
                }
                else
                {
                    reservationObj.Payments.Add(new Payment
                    {
                        AmountPaid = totalAmount,
                        PaymentDate = DateTime.UtcNow,
                        PaymentMethod = paymentResponse.PaymentMethod,
                        TransactionId = paymentResponse.TransactionId
                    });

                    for (int i = 0; i < item.Quantity; i++)
                    {
                        var ticket = new Ticket
                        {
                            PurchaseDate = item.PurchaseDate,
                            TicketCode = EventUtil.GenerateCode("TXN", 5),
                            Reservation = reservationObj
                        };
                        reservationObj.Tickets.Add(ticket);
                    }
                }

                var reservation = await _reservationsRepository.AddAsync(reservationObj).ConfigureAwait(false);
                ReservationDto reservationDto = _mapper.Map<ReservationDto>(reservation);
                return reservationDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Reservation Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Cancels a reservation if it is confirmed and within the event date range.
        /// Refunds amount if payment was made and event is not over.
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CancelReservation(int reservationId)
        {
            var reservation = await GetReservation(reservationId).ConfigureAwait(false);
            if (reservation == null)
            {
                _logger.LogError($"Reservation not found for ID: {reservationId}");
                return false;
            }

            if (!reservation.IsConfirmed)
            {
                _logger.LogError($"Reservation was not confirmed. No payments made against this reservation ID: {reservationId} | ReservationCode: {reservation.ReservationCode}");
                return false;
            }

            // get event details to fetch event date
            var eventObj = await _eventsRepository.GetByIdAsync(reservation.EventId, e => e.TicketTypes).ConfigureAwait(false);
            if (eventObj == null)
            {
                _logger.LogError($"Event not found for ID: {reservation.EventId}");
                throw new Exception("Event not found.");
            }

            if (DateTime.UtcNow > eventObj.EndDate)
            {
                _logger.LogError($"Event is over. Reservation cannot be cancelled. ReservationCode: {reservation.ReservationCode}");
                return false;
            }

            // refund payment
            var paymentResponse = await _paymentService.RefundPaymentAsync(reservation.Payments.FirstOrDefault().TransactionId);
            if (!paymentResponse.IsSuccessful)
            {
                _logger.LogError($"Refund failed: {paymentResponse.Message} | ReservationCode: {reservation.ReservationCode}");
                return false;
            }

            // remove tickets
            foreach (var ticket in reservation.Tickets)
            {
                await _ticketsRepository.RemoveAsync(ticket).ConfigureAwait(false);
            }

            reservation.IsActive = false;

            // update reservation
            await _reservationsRepository.UpdateAsync(reservation).ConfigureAwait(false);

            return true;
        }

        public async Task<List<EventSeatsAvailability>> GetEventSeatsAvailability(int eventId)
        {
            if(eventId <= 0)
            {
                _logger.LogError($"Invalid event ID: {eventId}");
                throw new ArgumentException("Invalid event ID.");
            }
            var eventObj = _eventsRepository.GetByIdAsync(eventId, e => e.TicketTypes).Result;
            if (eventObj == null)
            {
                _logger.LogError($"Event not found for ID: {eventId}");
                throw new Exception("Event not found.");
            }

            if (DateTime.UtcNow > eventObj.EndDate)
            {
                _logger.LogError($"Event is closed for booking since it is in the past");
                throw new Exception("Event is closed for booking");
            }

            var eventSeatsAvailabilityList = new List<EventSeatsAvailability>();
            foreach (var ticketType in eventObj.TicketTypes)
            {
                // fetch total quantity of confirmed reservations for this event and ticket type
                var totalConfirmedReservations = await _reservationsRepository
                    .FindAsync(r => r.Event.Id == eventObj.Id && r.TicketType.Id == ticketType.Id && r.IsConfirmed)
                    .ConfigureAwait(false);

                // get total seats with confirmed reservations for this event and ticket type
                var reservedSeats = totalConfirmedReservations.Sum(r => r.Quantity);
                var availableSeats = ticketType.TotalSeats - reservedSeats;
                eventSeatsAvailabilityList.Add(new EventSeatsAvailability
                {
                    EventId = eventId,
                    EventName = eventObj.Name,
                    TicketTypeId = ticketType.Id,
                    TicketTypeName = ticketType.Name,
                    MaxSeats = ticketType.TotalSeats,
                    AvailableTickets = ticketType.TotalSeats - availableSeats
                });
            }

            return eventSeatsAvailabilityList;
        }

        #endregion
    }
}
