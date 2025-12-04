using Events.api.Components.MessageBroker;
using Microsoft.AspNetCore.Mvc;

namespace Events.api.Controllers
{
    [ApiController]
    [Route("/api/events")]
    public class EventsController(IEventProducer producer) : Controller
    {
        [HttpPost("user")]
        [HttpGet("user")]
        public async Task<IActionResult> UserAsync()
        {
            await producer.PublishUserEventAsync("USER");
            return Ok();
        }
        [HttpPost("movie")]
        public async Task<IActionResult> MovieAsync()
        {
            await producer.PublishMovieEventAsync("MOVIE");
            return Ok();
        }
        [HttpPost("payment")]
        public async Task<IActionResult> PaymentAsync()
        {
            await producer.PublishPaymentEventAsync("PAYMENT");
            return Ok();
        }

    }
}
