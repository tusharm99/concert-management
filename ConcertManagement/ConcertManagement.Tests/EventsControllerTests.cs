using ConcertManagement.Api.Controllers;
using ConcertManagement.Core.Dtos;
using ConcertManagement.Core.Entities;
using ConcertManagement.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ConcertManagement.Tests
{
    public class EventsControllerTests
    {
        private readonly Mock<ILogger<EventsController>> _mockLogger;
        private readonly Mock<IConcertService> _mockService;
        private readonly EventsController _controller;

        public EventsControllerTests()
        {
            _mockLogger = new Mock<ILogger<EventsController>>();
            _mockService = new Mock<IConcertService>();
            _controller = new EventsController(_mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task GetEventById_ValidEvent()
        {
            Event eventObj = new Event { Id = 1, Name = "Test Event" };
            _mockService.Setup(s => s.GetEvent(eventObj.Id)).ReturnsAsync(eventObj);

            var act = await _controller.GetEventById(1);

            var okResult = act as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(eventObj);
        }

        [Fact]
        public async Task GetEventById_EventNotFound()
        {
            int eventId = 123;
            _mockService.Setup(s => s.GetEvent(eventId)).ReturnsAsync((Event)null);

            var act = await _controller.GetEventById(eventId);

            act.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetEventsByVenue_ReturnsEventList()
        {
            int venueId = 10;
            var mockEvents = new List<Event>
            {
                new Event { Id = 1, Name = "MyEvent X", VenueId = 10 },
                new Event { Id = 2, Name = "MyEvent Y", VenueId = 10 }
            };
            _mockService.Setup(s => s.GetEventsByVenue(venueId)).ReturnsAsync(mockEvents);

            var act = await _controller.GetEventsByVenue(venueId);

            var okResult = act as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<Event>>().Which.All(e => e.VenueId == venueId);
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(mockEvents);
        }

        [Fact]
        public async Task AddEvent_ValidEvent()
        {
            var inputEvent = new EventDto { Id = 1, Name = "New Event", VenueId = 5, VenueName = "Test Venue" };
            var outputEvent = new Event { Id = 1, Name = "New Event", VenueId = 5, Venue = new Venue { Name = "Test Venue" } };
            
            _mockService.Setup(s => s.CreateEvent(inputEvent)).ReturnsAsync(outputEvent);

            var act = await _controller.AddEvent(inputEvent);

            var createdResult = act as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(outputEvent);
        }

        [Fact]
        public async Task AddEvent_InvalidModel()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var act = await _controller.AddEvent(new EventDto());

            act.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateEvent_UpdateEvent()
        {
            var inputEvent = new EventDto { Id = 1, Name = "Updated", VenueId = 10 };
            var outputEvent = new Event { Id = 1, Name = inputEvent.Name, VenueId = inputEvent.VenueId, Venue = new Venue { Name = "Test Venue" } };

            _mockService.Setup(s => s.GetEvent(1)).ReturnsAsync(outputEvent);
            _mockService.Setup(s => s.UpdateEvent(inputEvent)).Returns(Task.CompletedTask);

            var act = await _controller.UpdateEvent(1, inputEvent);

            act.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateEvent_EventNotFound()
        {
            var inputEvent = new EventDto { Id = 1, Name = "Updated", VenueId = 10 };
            _mockService.Setup(s => s.GetEvent(inputEvent.Id)).ReturnsAsync((Event)null);

            var result = await _controller.UpdateEvent(1, inputEvent);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
