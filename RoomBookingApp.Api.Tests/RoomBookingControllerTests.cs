using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Api.Controllers;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Api.Tests
{
    public class RoomBookingControllerTests
    {
        private Mock<IRoomBookingRequestProcessor> roomBookingRequestProcessor;
        private RoomBookingController controller;
        private RoomBookingRequest request;
        private RoomBookingResult result;

        public RoomBookingControllerTests()
        {
            roomBookingRequestProcessor = new Mock<IRoomBookingRequestProcessor>();
            controller = new RoomBookingController(roomBookingRequestProcessor.Object);
            request = new RoomBookingRequest();
            result = new RoomBookingResult();

            roomBookingRequestProcessor
                .Setup(x => x.BookRoom(request))
                .Returns(result);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult), BookingResultFlag.Sucess)]
        [InlineData(0, false, typeof(BadRequestObjectResult), BookingResultFlag.Failure)]
        public async Task Should_Call_Booking_Method_When_Valid(int expectedMethodCalls,
            bool isModelValid, Type expectedResultType, BookingResultFlag bookingResultFlag)
        {
            //Arrange
            if (!isModelValid)
                controller.ModelState.AddModelError("Key", "ErrorMessage");

            result.Flag = bookingResultFlag;

            //Act
            var response = await controller.BookRoom(request);

            //Assert
            response.ShouldBeOfType(expectedResultType);
            roomBookingRequestProcessor.Verify(x => x.BookRoom(request), Times.Exactly(expectedMethodCalls));

        }

    }
}
