using ConcertManagement.Core.Dtos;
using ConcertManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ConcertManagement.Service
{
    public interface IConcertService
    {
        Task<IEnumerable<Event>> GetEvents();
        Task<Event> GetEvent(int id, bool includeTicketTypes = false);
        Task<IEnumerable<Event>> GetEventsByVenue(int venueId);
        Task<Event> CreateEvent(EventDto item);
        Task<TicketType> CreateTicketType(int eventId, TicketTypeDto item);
        Task UpdateEvent(EventDto item);
        Task<IEnumerable<Venue>> GetVenues();
        Task<Venue> GetVenue(int id);
        Task<Venue> CreateVenue([FromBody] VenueDto item);
        Task UpdateVenue([FromBody] VenueDto item);
    }
}
