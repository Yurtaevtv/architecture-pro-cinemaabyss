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
            await producer.PublishUserEventAsync("USER");
            return Created();
        }
        [HttpPost("movie")]
        public async Task<IActionResult> MovieAsync()
        {
            await producer.PublishMovieEventAsync("MOVIE");
            return Created();
        }
        [HttpPost("payment")]
        public async Task<IActionResult> PaymentAsync()
        {
            await producer.PublishPaymentEventAsync("PAYMENT");
            return Created();
        }

    }
}
