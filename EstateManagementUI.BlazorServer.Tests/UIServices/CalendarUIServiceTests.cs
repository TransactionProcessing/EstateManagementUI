using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using Xunit;

namespace EstateManagementUI.BlazorServer.Tests.UIServices
{
    public class CalendarUIServiceTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly CalendarUIService _service;

        public CalendarUIServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _service = new CalendarUIService(_mockMediator.Object);
        }

        [Fact]
        public async Task GetComparisonDates_ReturnsMappedList_WhenMediatorSucceeds()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var bizList = new List<ComparisonDateModel>
            {
                new() { Date = DateTime.UtcNow.Date.AddDays(-7), Description = "Last Week" },
                new() { Date = DateTime.UtcNow.Date.AddDays(-30), Description = "Last Month" }
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(bizList));

            // Act
            var result = await _service.GetComparisonDates(CorrelationIdHelper.New(), estateId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data!.Count.ShouldBe(2);
            result.Data[0].Description.ShouldBe("Last Week");
            result.Data[1].Description.ShouldBe("Last Month");

            _mockMediator.Verify(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetComparisonDates_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("backend error"));

            // Act
            var result = await _service.GetComparisonDates(CorrelationIdHelper.New(), Guid.NewGuid());

            // Assert
            result.IsFailed.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}