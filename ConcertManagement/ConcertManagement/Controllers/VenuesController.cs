using ConcertManagement.Core.Dtos;
using ConcertManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace ConcertManagement.Api.Controllers
{
    [Route("api/venues")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly ILogger<VenuesController> _logger;
        private readonly IConcertService _concertService;

        public VenuesController(ILogger<VenuesController> logger, IConcertService concertService)
        {
            _logger = logger;
            _concertService = concertService;
        }

        [HttpGet("", Name = "GetVenues")]
        public async Task<IActionResult> GetVenues()
        {
            _logger.LogInformation("Fetching all venues");
            var result = await _concertService.GetVenues();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetVenueById")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVenueById(int id)
        {
            _logger.LogInformation("Fetching venues for venueId {id}", id);
            var result = await _concertService.GetVenue(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost(Name = "AddVenue")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddVenue([FromBody] VenueDto item)
        {
            if (item == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid venue data");
                return BadRequest(ModelState);
            }

            var createdEvent = await _concertService.CreateVenue(item);

            _logger.LogInformation("Event created with ID: {EventId}", createdEvent.Id);
            return CreatedAtAction(nameof(GetVenueById), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpPut("{id}", Name = "UpdateVenue")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVenue(int id, [FromBody] VenueDto item)
        {
            if (item == null || id != item.Id || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid venue data");
                return BadRequest(ModelState);
            }

            var existingEvent = await _concertService.GetVenue(id);
            if (existingEvent == null)
            {
                _logger.LogWarning("Venue not found with ID {VenueId}", id);
                return NotFound();
            }

            await _concertService.UpdateVenue(item);

            _logger.LogInformation("Venue updated with ID {VenueId}", id);
            return NoContent();
        }
    }
}
