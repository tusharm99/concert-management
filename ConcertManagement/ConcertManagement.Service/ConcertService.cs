using AutoMapper;
using ConcertManagement.Core.Dtos;
using ConcertManagement.Core.Entities;
using ConcertManagement.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace ConcertManagement.Service
{
    public class ConcertService : IConcertService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IReservationsRepository _reservationRepository;
        private readonly IVenuesRepository _venueRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConcertService> _logger;

        public ConcertService(IEventsRepository eventsRepository,
                              IReservationsRepository reservationRepository,
                              IVenuesRepository venueRepository,
                              IMapper mapper,
                              ILogger<ConcertService> logger)
        {
            _eventsRepository = eventsRepository;
            _reservationRepository = reservationRepository;
            _venueRepository = venueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Event> GetEvent(int id)
        {
            return await _eventsRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Event>> GetEventsByVenue(int venueId)
        {
            return await _eventsRepository.FindAsync(_ => _.Venue.Id == venueId).ConfigureAwait(false);
        }

        public async Task<Event> CreateEvent(EventDto item)
        {
            Event eventObj = null;
            try
            {
                eventObj = _mapper.Map<Event>(item);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Automapper Error: {ex.Message}");
            }
            return await _eventsRepository.AddAsync(eventObj).ConfigureAwait(false);
        }

    }
}
