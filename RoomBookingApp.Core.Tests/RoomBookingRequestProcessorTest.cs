using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor processor;
        private RoomBookingRequest request;
        private Mock<IRoomBookingService> roomBookingServiceMock;
        private List<Room> availableRooms;

        public RoomBookingRequestProcessorTest()
        {
            request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "teste@request.com",
                Date = new DateTime(2021, 10, 20)
            };

            availableRooms = new List<Room>{ new() { Id = 1 } };

            roomBookingServiceMock = new Mock<IRoomBookingService>();
            roomBookingServiceMock
                .Setup(q => q.GetAvailableRooms(request.Date))
                .Returns(availableRooms);

            processor = new RoomBookingRequestProcessor(roomBookingServiceMock.Object);

        }

        [Fact]
        public void Should_Return_Room_Booking_Respose_With_Request_Values()
        {
            //Arrange
            //...

            //Act
            RoomBookingResult result = processor.BookRoom(request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.FullName, result.FullName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Date, result.Date);

            /*result.ShouldNotBeNull();
            result.FullName.ShouldBe(result.FullName);
            result.Email.ShouldBe(result.Email);
            result.Date.ShouldBe(result.Date);*/

        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            var exception = Should.Throw<ArgumentNullException>(() => processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");

            //or...
            //Assert.Throws<ArgumentNullException>(() => processor.BookRoom(null));

        }

        [Fact]
        public void Should_Save_Room_Booking_Request()
        {
            RoomBooking savedBooking = null;
            roomBookingServiceMock
                .Setup(q => q.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking => savedBooking = booking);

            processor.BookRoom(request);

            roomBookingServiceMock
                .Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Once);

            savedBooking.ShouldNotBeNull();
            savedBooking.FullName.ShouldBe(request.FullName);
            savedBooking.Email.ShouldBe(request.Email);
            savedBooking.Date.ShouldBe(request.Date);
            savedBooking.RoomId.ShouldBe(availableRooms.FirstOrDefault().Id);

        }

        [Fact]
        public void Should_Not_Save_Room_Booking_Request_If_None_Available()
        {
            availableRooms.Clear();
            processor.BookRoom(request);

            roomBookingServiceMock
                .Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure, false)]
        [InlineData(BookingResultFlag.Sucess, true)]
        public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingResultFlag, bool isAvailable)
        {
            if (!isAvailable)
                availableRooms.Clear();

            var result = processor.BookRoom(request);
            bookingResultFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void Should_Return_RoombookingId_In_Result(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
                availableRooms.Clear();
            else
            {
                roomBookingServiceMock
                    .Setup(q => q.Save(It.IsAny<RoomBooking>()))
                    .Callback<RoomBooking>(booking => booking.Id = roomBookingId.Value);
            }

            var result = processor.BookRoom(request);
            result.RoomBookingId.ShouldBe(roomBookingId);
        }

    }
}