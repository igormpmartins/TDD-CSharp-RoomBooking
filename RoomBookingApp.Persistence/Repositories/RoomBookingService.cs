using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private RoomBookingAppDbContext context;

        public RoomBookingService(RoomBookingAppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime date)
        {
            /*
            var unavailableRooms = context.RoomBookings
                .Where(q => q.Date == date)
                .Select(q => q.RoomId)
                .ToList();

            var availableRooms = context.Rooms.Where(q => !unavailableRooms.Contains(q.Id)).ToList();*/

            var availableRooms = context.Rooms
                .Where(q => !q.RoomBookings.Any(q => q.Date  == date))
                .ToList();

            return availableRooms;
        }

        public void Save(RoomBooking roomBooking)
        {
            context.Add(roomBooking);
            context.SaveChanges();
        }
    }
}
