using ConcertManagement.Core.Dtos;
using ConcertManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace ConcertManagement.Api.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IConcertService _concertService;

        public EventsController(ILogger<EventsController> logger, IConcertService concertService)
        {
            _logger = logger;
            _concertService = concertService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            _logger.LogInformation("Fetching events for eventId {id}", id);
            var result = await _concertService.GetEvent(id);
            return result != null ? Ok(result) : NotFound();
        }


        [HttpGet(Name = "venue/{venueId}")]
        public async Task<IActionResult> GetEventsByVenue(int venueId)
        {
            _logger.LogInformation("Fetching events against venueId {VenueId}", venueId);
            var events = await _concertService.GetEventsByVenue(venueId);
            return Ok(events);
        }

        [HttpPost(Name = "")]
        public async Task<IActionResult> AddEvent([FromBody] EventDto item)
        {
            if (item == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid event data");
                return BadRequest(ModelState);
            }

            var createdEvent = await _concertService.CreateEvent(item);

            _logger.LogInformation("Event created with ID: {EventId}", createdEvent.Id);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }
    }
}
