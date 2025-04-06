using ConcertManagement.Core.Dtos;
using ConcertManagement.Core.Entities;

namespace ConcertManagement.Service
{
    public interface IConcertService
    {
        Task<Event> GetEvent(int id);
        Task<IEnumerable<Event>> GetEventsByVenue(int venueId);
        Task<Event> CreateEvent(EventDto item);
    }
}
