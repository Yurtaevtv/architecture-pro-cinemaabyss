using System.Net;
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
            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json(new {status = "success"});
        }
        [HttpPost("movie")]
        public async Task<IActionResult> MovieAsync()
        {
            await producer.PublishMovieEventAsync("MOVIE");
            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json(new {status = "success"});
        }
        [HttpPost("payment")]
        public async Task<IActionResult> PaymentAsync()
        {
            await producer.PublishPaymentEventAsync("PAYMENT");
            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json(new {status = "success"});
        }

    }
}
