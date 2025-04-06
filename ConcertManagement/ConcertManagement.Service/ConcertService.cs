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
        private readonly IVenuesRepository _venuesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConcertService> _logger;

        public ConcertService(IEventsRepository eventsRepository,
                              IReservationsRepository reservationRepository,
                              IVenuesRepository venuesRepository,
                              IMapper mapper,
                              ILogger<ConcertService> logger)
        {
            _eventsRepository = eventsRepository;
            _reservationRepository = reservationRepository;
            _venuesRepository = venuesRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Events
        public async Task<IEnumerable<Event>> GetEvents()
        {
            return await _eventsRepository.GetAllAsync().ConfigureAwait(false);
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
        #endregion
    }
}
