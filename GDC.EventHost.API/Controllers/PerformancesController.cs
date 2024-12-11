using GDC.EventHost.DTO.Performance;
using Microsoft.AspNetCore.JsonPatch;
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
            var eventFromStore = EventHostDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                return NotFound();
            }

            return Ok(eventFromStore.Performances);
        }

        [HttpGet("{performanceId}", Name = "GetPerformance")]
        public ActionResult GetPerformance(Guid eventId, Guid performanceId)
        {
            var eventFromStore = EventHostDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                return NotFound();
            }

            var performance = eventFromStore
                .Performances.FirstOrDefault(t => t.Id == performanceId);

            if (performance == null)
            {
                return NotFound();
            }

            return Ok(performance);
        }

        [HttpPost]
        public ActionResult<PerformanceDto> CreatePerformance(
            Guid eventId,
            [FromBody] PerformanceForUpdateDto performanceForCreate)
        {
            var eventFromStore = EventHostDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                return NotFound();
            }

            var newPerformance = new PerformanceDto()
            {
                Date = performanceForCreate.Date,
                Title = performanceForCreate.Title,
                EventId = performanceForCreate.EventId,
                PerformanceTypeId = performanceForCreate.PerformanceTypeId,
                StatusId = performanceForCreate.StatusId,
                VenueId = performanceForCreate.VenueId,
                SeatingPlanId = performanceForCreate.SeatingPlanId,
                Id = Guid.NewGuid()
            };

            eventFromStore.Performances.Add(newPerformance);

            return CreatedAtRoute("GetPerformance",
                new
                {
                    eventId = newPerformance.EventId,
                    performanceId = newPerformance.Id
                },
                newPerformance);
        }

        [HttpPut("{performanceId}")]
        public ActionResult UpdatePerformance(
            Guid eventId,
            Guid performanceId,
            PerformanceForUpdateDto performanceForUpdate)
        {
            var eventFromStore = EventHostDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                return NotFound();
            }

            var performanceFromStore = EventHostDataStore.Current
                .Performances.FirstOrDefault(p => p.Id == performanceId);

            if (performanceFromStore == null)
            {
                return NotFound();
            }

            performanceFromStore.EventId = eventId;
            performanceFromStore.Date = performanceForUpdate.Date; 
            performanceFromStore.Title = performanceForUpdate.Title;
            performanceFromStore.VenueId = performanceForUpdate.VenueId;
            performanceFromStore.StatusId = performanceForUpdate.StatusId;
            performanceFromStore.PerformanceTypeId = performanceForUpdate.PerformanceTypeId;
            performanceFromStore.SeatingPlanId = performanceForUpdate.SeatingPlanId;

            return NoContent();
        }

        [HttpPatch("{performanceId}")]
        public ActionResult PartiallyUpdatePerformance(
            Guid eventId,
            Guid performanceId,
            JsonPatchDocument<PerformanceForUpdateDto> patchDocument)
        {
            var eventFromStore = EventHostDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                return NotFound();
            }

            var performanceFromStore = EventHostDataStore.Current
                .Performances.FirstOrDefault(p => p.Id == performanceId);

            if (performanceFromStore == null)
            {
                return NotFound();
            }

            var performanceToPatch = new PerformanceForUpdateDto()
            {
                Date = performanceFromStore.Date,
                Title = performanceFromStore.Title,
                EventId = eventId,
                PerformanceTypeId = performanceFromStore.PerformanceTypeId,
                StatusId = performanceFromStore.StatusId,
                VenueId = performanceFromStore.VenueId,
                SeatingPlanId = performanceFromStore.SeatingPlanId
            };

            patchDocument.ApplyTo(performanceToPatch, ModelState);

            // check for any errors in the patch document
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check for broken validation rules on the model
            if (!TryValidateModel(performanceToPatch))
            {
                return BadRequest(ModelState);
            }

            performanceFromStore.EventId = eventId;
            performanceFromStore.Date = performanceToPatch.Date;
            performanceFromStore.VenueId = performanceToPatch.VenueId;
            performanceFromStore.StatusId = performanceToPatch.StatusId;
            performanceFromStore.PerformanceTypeId = performanceToPatch.PerformanceTypeId;
            performanceFromStore.SeatingPlanId = performanceToPatch.SeatingPlanId;

            return NoContent();
        }

        [HttpDelete("{performanceId}")]
        public ActionResult DeletePerformance(Guid eventId, Guid performanceId)
        {
            var eventFromStore = EventHostDataStore.Current
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                return NotFound();
            }

            var performanceFromStore = EventHostDataStore.Current
                .Performances.FirstOrDefault(p => p.Id == performanceId);

            if (performanceFromStore == null)
            {
                return NotFound();
            }

            eventFromStore.Performances.Remove(performanceFromStore);

            return NoContent();
        }
    }
}
