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

        [HttpPost(Name = "AddEvent")]
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

        [HttpPut("{id}", Name = "UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto item)
        {
            if (item == null || id != item.Id || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Event data");
                return BadRequest(ModelState);
            }

            var existingEvent = await _concertService.GetEvent(id);
            if (existingEvent == null)
            {
                _logger.LogWarning("Event not found with ID {EventId} not found", id);
                return NotFound();
            }

            await _concertService.UpdateEvent(item);

            _logger.LogInformation("Event updated with ID {EventId}", id);
            return NoContent();
        }
    }
}
