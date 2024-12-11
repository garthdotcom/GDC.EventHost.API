using GDC.EventHost.DTO.Event;
using Microsoft.AspNetCore.Mvc;

namespace GDC.EventHost.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<EventDto>> GetEvents()
        {
            return Ok(EventHostDataStore.Current.Events);
        }

        [HttpGet("{id}")]
        public ActionResult<EventDto> GetEvent(Guid id)
        {
            var eventToReturn = EventHostDataStore.Current.Events.FirstOrDefault(e => e.Id == id);

            if (eventToReturn == null)
            {
                return NotFound();
            }

            return Ok(eventToReturn);
        }
    }
}
