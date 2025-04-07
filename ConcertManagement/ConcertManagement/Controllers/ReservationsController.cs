using ConcertManagement.Core.Dtos;
using ConcertManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace ConcertManagement.Api.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ILogger<ReservationsController> _logger;
        private readonly IConcertService _concertService;

        public ReservationsController(ILogger<ReservationsController> logger, IConcertService concertService)
        {
            _logger = logger;
            _concertService = concertService;
        }

        [HttpGet("{reservationId}", Name = "GetReservationById")]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReservationById(int reservationId)
        {
            _logger.LogInformation("Fetching reservations for reservationId {id}", reservationId);
            var result = await _concertService.GetReservation(reservationId);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost(Name = "ReserveTickets")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReserveTickets([FromBody] ReservationRequest item)
        {
            if (item == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid reservation data");
                return BadRequest(ModelState);
            }

            var reservation = await _concertService.CreateReservation(item);

            _logger.LogInformation("Event created with ID: {ReservationId}", reservation.Id);
            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }
    }
}
