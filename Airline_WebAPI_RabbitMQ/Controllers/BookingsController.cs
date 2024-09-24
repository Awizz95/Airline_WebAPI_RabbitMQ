using Airline.API.Models;
using Airline.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Airline.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger<BookingsController> _logger;
        private readonly IMessageProducer _messageProducer;

        public static readonly List<Booking> _bookings = new(); //in-memory db

        public BookingsController(IMessageProducer messageProducer, ILogger<BookingsController> logger)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public  IActionResult CreatingBooking(Booking newBooking)
        {
            if (!ModelState.IsValid) return BadRequest();

            _bookings.Add(newBooking);

            _messageProducer.SendingMessage<Booking>(newBooking);

            return Ok();
        }
    }
}
