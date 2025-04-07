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

        [HttpGet("{eventId}", Name = "GetEventById")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventById(int eventId)
        {
            _logger.LogInformation("Fetching events for eventId {id}", eventId);
            var result = await _concertService.GetEvent(eventId);
            return result != null ? Ok(result) : NotFound();
        }


        [HttpGet(Name = "GetEventsByVenue")]
        [ProducesResponseType(typeof(IEnumerable<EventDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventsByVenue(int venueId)
        {
            _logger.LogInformation("Fetching events against venueId {VenueId}", venueId);
            var events = await _concertService.GetEventsByVenue(venueId);
            return Ok(events);
        }

        [HttpPost(Name = "AddEvent")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [HttpPut("{eventId}", Name = "UpdateEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEvent(int eventId, [FromBody] EventDto item)
        {
            if (item == null || eventId != item.Id || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Event data");
                return BadRequest(ModelState);
            }

            var existingEvent = await _concertService.GetEvent(eventId);
            if (existingEvent == null)
            {
                _logger.LogWarning("Event not found with ID {EventId}", eventId);
                return NotFound();
            }

            await _concertService.UpdateEvent(item);

            _logger.LogInformation("Event updated with ID {EventId}", eventId);
            return NoContent();
        }

        [HttpGet("{eventId}/ticket-types", Name = "GetEventWithTicketTypes")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventWithTicketTypes(int eventId)
        {
            _logger.LogInformation("Fetching events for eventId {id} with Ticket Types", eventId);
            var result = await _concertService.GetEvent(eventId, includeTicketTypes: true);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("{eventId}/ticket-types", Name = "AddTicketType")]
        [ProducesResponseType(typeof(TicketTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTicketType(int eventId, [FromBody] TicketTypeDto item)
        {
            if (item == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid TicketType data");
                return BadRequest(ModelState);
            }

            var ticketType = await _concertService.CreateTicketType(eventId, item);

            _logger.LogInformation("Ticket Type created with ID {TicketTypeId} for EventId {EventId}", ticketType.Id, eventId);
            return CreatedAtAction(nameof(GetEventById), new { id = ticketType.Id }, ticketType);
        }
    }
}
