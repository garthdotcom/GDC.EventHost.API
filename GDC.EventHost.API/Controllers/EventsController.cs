using AutoMapper;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.DTO.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GDC.EventHost.API.Controllers
{
    [Route("api/series/{seriesId}/events")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IMailService _mailService;
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public EventsController(ILogger<EventsController> logger,
            IMailService mailService,
            IEventHostRepository eventHostRepository,
            IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ??
                throw new ArgumentNullException(nameof(mailService));
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents(
            [FromQuery] EventResourceParameters eventResourceParameters)
        {
            if (eventResourceParameters.PageSize > maxPageSize)
            {
                eventResourceParameters.PageSize = maxPageSize;
            }

            var (eventEntities, paginationMetadata) = await _eventHostRepository
                .GetEventsAsync(eventResourceParameters);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            if (eventResourceParameters.IncludeDetail)
            {
                return Ok(_mapper.Map<IEnumerable<EventDetailDto>>(eventEntities));
            }

            return Ok(_mapper.Map<IEnumerable<EventDto>>(eventEntities));
        }


        [HttpGet("{eventId}", Name = "GetEvent")]
        public async Task<ActionResult> GetEvent(Guid eventId, bool includeDetail)
        {
            var entity = await _eventHostRepository.GetEventAsync(eventId, includeDetail);

            if (entity == null)
            {
                return NotFound();
            }

            if (includeDetail)
            {
                return Ok(_mapper.Map<EventDetailDto>(entity));
            }

            return Ok(_mapper.Map<EventDto>(entity));
        }


        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent(
            Guid seriesId,
            [FromBody] EventForUpdateDto eventForUpdateDto)
        {
            if (!await _eventHostRepository.SeriesExistsAsync(seriesId))
            {
                _logger.LogInformation("Series with id {SeriesId} was not found when trying to create a event.",
                    seriesId);
                return NotFound();
            }

            var newEventEntity = _mapper.Map<Entities.Event>(eventForUpdateDto);

            await _eventHostRepository.AddEventToSeriesAsync(seriesId, newEventEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                var eventToReturnDto = _mapper.Map<EventDto>(newEventEntity);

                return CreatedAtRoute("GetEvent",
                    new
                    {
                        seriesId = eventToReturnDto.SeriesId,
                        eventId = eventToReturnDto.Id,
                        includePerformances = false
                    },
                    eventToReturnDto);
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to create a new event for series id {seriesId}.");
        }


        [HttpPut("{eventId}")]
        public async Task<ActionResult> UpdateEvent(
            Guid seriesId,
            Guid eventId,
            EventForUpdateDto eventForUpdateDto)
        {
            if (!await _eventHostRepository.SeriesExistsAsync(seriesId))
            {
                _logger.LogInformation("Series with id {SeriesId} was not found when trying to update a event.",
                    seriesId);
                return NotFound();
            }

            var eventEntity = await _eventHostRepository
                .GetEventForSeriesAsync(seriesId, eventId, false);

            if (eventEntity == null)
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to update the event.", eventId);
                return NotFound();
            }

            // overwrite the entity with the corresponding values in the dto
            _mapper.Map(eventForUpdateDto, eventEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to update the event with id {eventId} for series id {seriesId}.");
        }


        [HttpPatch("{eventId}")]
        public async Task<ActionResult> PartiallyUpdateEvent(
            Guid seriesId,
            Guid eventId,
            JsonPatchDocument<EventForUpdateDto> patchDocument)
        {
            if (!await _eventHostRepository.SeriesExistsAsync(seriesId))
            {
                _logger.LogInformation("Series with id {SeriesId} was not found when trying to partially update a event.",
                    seriesId);
                return NotFound();
            }

            var eventEntity = await _eventHostRepository
                .GetEventForSeriesAsync(seriesId, eventId, false);

            if (eventEntity == null)
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to partially update the event.", eventId);
                return NotFound();
            }

            var eventToPatchDto = _mapper.Map<EventForUpdateDto>(eventEntity);

            patchDocument.ApplyTo(eventToPatchDto, ModelState);

            // check for any errors in the patch document
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("An issue was found in the patch document when trying to patch id {EventId}.", eventId);
                return BadRequest(ModelState);
            }

            // check for broken validation rules on the model
            if (!TryValidateModel(eventToPatchDto))
            {
                _logger.LogInformation("Validation issue(s) was/were found when trying to patch id {EventId}.", eventId);
                return BadRequest(ModelState);
            }

            // overwrite the entity with the corresponding dto values
            _mapper.Map(eventToPatchDto, eventEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to partially update the event with id {eventId} for series id {seriesId}.");

        }


        [HttpDelete("{eventId}")]
        public async Task<ActionResult> DeleteEvent(Guid seriesId, Guid eventId)
        {
            if (!await _eventHostRepository.SeriesExistsAsync(seriesId))
            {
                _logger.LogInformation("Series with id {SeriesId} was not found when trying to delete a event with id {EventId}.",
                    seriesId, eventId);
                return NotFound();
            }

            var eventEntity = await _eventHostRepository
                .GetEventForSeriesAsync(seriesId, eventId, false);

            if (eventEntity == null)
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to partially update the event.", eventId);
                return NotFound();
            }

            _eventHostRepository.DeleteEvent(eventEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                _mailService.Send($"Event Deleted: {eventEntity.Title}",
                $"A event called '{eventEntity.Title}' for series id '{seriesId}' was deleted.");
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to delete the event '{eventEntity.Title}' with id {eventId} for series id {seriesId}.");
        }
    }
}
