using Events.api.Components.MessageBroker;
using Microsoft.AspNetCore.Mvc;

namespace Events.api.Controllers
{
    [ApiController]
    [Route("/api/events")]
    public class EventsController(IEventProducer producer) : Controller
    {
        [HttpPost("user")]
        public async Task<IActionResult> UserAsync()
        {
            await producer.PublishEventAsync("USER");
            return Ok();
        }
        [HttpPost("movie")]
        public async Task<IActionResult> MovieAsync()
        {
            await producer.PublishEventAsync("MOVIE");
            return Ok();
        }
        [HttpPost("payment")]
        public async Task<IActionResult> PaymentAsync()
        {
            await producer.PublishEventAsync("PAYMENT");
            return Ok();
        }

    }
}
