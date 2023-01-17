using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingRequestProcessor roomBookingRequestProcessor;

        public RoomBookingController(IRoomBookingRequestProcessor roomBookingRequestProcessor)
        {
            this.roomBookingRequestProcessor = roomBookingRequestProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> BookRoom(RoomBookingRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = roomBookingRequestProcessor.BookRoom(request);
                if (result.Flag == BookingResultFlag.Sucess)
                    return Ok(result);

                ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No room available for given date");
            }

            return BadRequest(ModelState);
        }
    }
}
