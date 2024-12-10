using GDC.EventHost.DTO.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace GDC.EventHost.API.Controllers
{
    [Route("api/events/{eventId}/tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<TicketDto>> GetTickets(Guid eventId)
        {
            var theEvent = EventsDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (theEvent == null)
            {
                return NotFound();
            }

            return Ok(theEvent.Tickets);
        }

        [HttpGet("{ticketId}")]
        public ActionResult GetTicket(Guid eventId, Guid ticketId)
        {
            // find the event
            var theEvent = EventsDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (theEvent == null)
            {
                return NotFound();
            }

            // find the ticket
            var ticket = theEvent
                .Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }
    }
}
