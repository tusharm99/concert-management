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
    public class VenuesControllerTests
    {
        private readonly Mock<ILogger<VenuesController>> _mockLogger;
        private readonly Mock<IConcertService> _mockService;
        private readonly VenuesController _controller;

        public VenuesControllerTests()
        {
            _mockLogger = new Mock<ILogger<VenuesController>>();
            _mockService = new Mock<IConcertService>();
            _controller = new VenuesController(_mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task GetVenuesByVenue_ReturnsVenueList()
        {
            var mockVenues = new List<Venue>
            {
                new Venue { Id = 1, Name = "MyVenue X" },
                new Venue { Id = 2, Name = "MyVenue Y" }
            };
            _mockService.Setup(s => s.GetVenues()).ReturnsAsync(mockVenues);

            var act = await _controller.GetVenues();

            var okResult = act as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(mockVenues);
        }

        [Fact]
        public async Task AddVenue_ValidVenue()
        {
            var inputVenue = new VenueDto { Id = 1, Name = "New Venue" };
            var outputVenue = new Venue { Id = 1, Name = "New Venue" };

            _mockService.Setup(s => s.CreateVenue(inputVenue)).ReturnsAsync(outputVenue);

            var act = await _controller.AddVenue(inputVenue);

            var createdResult = act as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(outputVenue);
        }

        [Fact]
        public async Task AddVenue_InvalidModel()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var act = await _controller.AddVenue(new VenueDto());

            act.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateVenue_UpdateVenue()
        {
            var inputVenue = new VenueDto { Id = 1, Name = "New Venue" };
            var outputVenue = new Venue { Id = 1, Name = "New Venue" };

            _mockService.Setup(s => s.GetVenue(1)).ReturnsAsync(outputVenue);
            _mockService.Setup(s => s.UpdateVenue(inputVenue)).Returns(Task.CompletedTask);

            var act = await _controller.UpdateVenue(1, inputVenue);

            act.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateVenue_VenueNotFound()
        {
            var inputVenue = new VenueDto { Id = 1, Name = "New Venue" };
            _mockService.Setup(s => s.GetVenue(inputVenue.Id)).ReturnsAsync((Venue)null);

            var result = await _controller.UpdateVenue(1, inputVenue);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
