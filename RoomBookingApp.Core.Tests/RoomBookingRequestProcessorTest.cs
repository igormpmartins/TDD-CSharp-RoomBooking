using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {

        [Fact]
        public void Should_Return_Room_Booking_Respose_With_Request_Values()
        {
            //Arrange
            var request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "teste@request.com",
                Date = new DateTime(2021, 10, 20)
            };

            var processor = new RoomBookingRequestProcessor();

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
    }
}
