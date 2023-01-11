using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor
    {
        public RoomBookingRequestProcessor()
        {
        }

        public RoomBookingResult BookRoom(RoomBookingRequest request)
            => new RoomBookingResult
            {
                FullName = request.FullName,
                Date = request.Date,
                Email = request.Email
            };

    }
}