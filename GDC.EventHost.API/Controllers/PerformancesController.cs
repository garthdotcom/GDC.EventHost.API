using GDC.EventHost.API.Services;
using GDC.EventHost.DTO.Performance;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GDC.EventHost.API.Controllers
{
    [Route("api/events/{eventId}/performances")]
    [ApiController]
    public class PerformancesController : ControllerBase
    {
        private readonly ILogger<PerformancesController> _logger;
        private readonly IMailService _mailService;
        private readonly EventHostDataStore _eventHostDataStore;

        public PerformancesController(ILogger<PerformancesController> logger, 
            IMailService mailService,
            EventHostDataStore eventHostDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _eventHostDataStore = eventHostDataStore ?? throw new ArgumentNullException(nameof(eventHostDataStore));
        }


        [HttpGet]
        public ActionResult<IEnumerable<PerformanceDto>> GetPerformances(Guid eventId)
        {
            try
            {
                var eventFromStore = _eventHostDataStore
                    .Events.FirstOrDefault(e => e.Id == eventId);

                if (eventFromStore == null)
                {
                    _logger.LogInformation("Event with id {EventId} was not found when trying to get all performances.", 
                        eventId);
                    return NotFound();
                }

                return Ok(eventFromStore.Performances);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("An exception occurred when getting performances for event id {EventId}: {Ex}", 
                    eventId, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "A problem occurred while handling your request.");
            }
        }

        [HttpGet("{performanceId}", Name = "GetPerformance")]
        public ActionResult GetPerformance(Guid eventId, Guid performanceId)
        {
            var eventFromStore = _eventHostDataStore
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to get a performance with id {PerformanceId}.", eventId, performanceId);
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
            var eventFromStore = _eventHostDataStore
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to create a new performance.", eventId);
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
            var eventFromStore = _eventHostDataStore
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to update the performance with id {PerformanceId}.", eventId, performanceId);
                return NotFound();
            }

            var performanceFromStore = _eventHostDataStore
                .Performances.FirstOrDefault(p => p.Id == performanceId);

            if (performanceFromStore == null)
            {
                _logger.LogInformation("Performance with id {PerformanceId} was not found when trying to update the performance.", performanceFromStore?.Id);
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
            var eventFromStore = _eventHostDataStore
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to patch a performance with id {PerformanceId}.", eventId, performanceId);
                return NotFound();
            }

            var performanceFromStore = _eventHostDataStore
                .Performances.FirstOrDefault(p => p.Id == performanceId);

            if (performanceFromStore == null)
            {
                _logger.LogInformation("Performance with id {PerformanceId} was not found when trying to patch the performance.", performanceFromStore?.Id);
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
                _logger.LogInformation("An issue was found in the patch document when trying to patch id {PerformanceId}.", performanceFromStore?.Id);
                return BadRequest(ModelState);
            }

            // check for broken validation rules on the model
            if (!TryValidateModel(performanceToPatch))
            {
                _logger.LogInformation("Validation issue(s) was/were found when trying to patch id {PerformanceId}.", performanceFromStore?.Id);
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
            var eventFromStore = _eventHostDataStore
                .Events.FirstOrDefault(e => e.Id == eventId);

            if (eventFromStore == null)
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to delete a performance with id {PerformanceId}.", eventId, performanceId);
                return NotFound();
            }

            var performanceFromStore = _eventHostDataStore
                .Performances.FirstOrDefault(p => p.Id == performanceId);

            if (performanceFromStore == null)
            {
                _logger.LogInformation("Performance with id {PerformanceId} was not found when trying to delete the performance.", performanceFromStore?.Id);
                return NotFound();
            }

            eventFromStore.Performances.Remove(performanceFromStore);
            _mailService.Send($"Performance Deleted: {performanceFromStore.Title}", 
                $"A performance called '{performanceFromStore.Title}' for event '{eventFromStore.Title}' was deleted.");

            return NoContent();
        }
    }
}
