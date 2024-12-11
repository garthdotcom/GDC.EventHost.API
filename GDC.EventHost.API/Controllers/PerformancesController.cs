using GDC.EventHost.DTO.Performance;
using Microsoft.AspNetCore.Mvc;

namespace GDC.EventHost.API.Controllers
{
    [Route("api/events/{eventId}/performances")]
    [ApiController]
    public class PerformancesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PerformanceDto>> GetPerformances(Guid eventId)
        {
            var theEvent = EventHostDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (theEvent == null)
            {
                return NotFound();
            }

            return Ok(theEvent.Performances);
        }

        [HttpGet("{performanceId}")]
        public ActionResult GetPerformance(Guid eventId, Guid performanceId)
        {
            // find the event
            var theEvent = EventHostDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (theEvent == null)
            {
                return NotFound();
            }

            // find the performance
            var performance = theEvent
                .Performances.FirstOrDefault(t => t.Id == performanceId);

            if (performance == null)
            {
                return NotFound();
            }

            return Ok(performance);
        }
    }
}
