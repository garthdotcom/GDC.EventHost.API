using AutoMapper;
using GDC.EventHost.API.Services;
using GDC.EventHost.DTO.Performance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GDC.EventHost.API.Controllers
{
    [Authorize]
    //[Authorize(Policy = "MustBeAdministrator")]
    [Route("api/events/{eventId}/performances")]
    [ApiController]
    public class PerformancesController : ControllerBase
    {
        private readonly ILogger<PerformancesController> _logger;
        private readonly IMailService _mailService;
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public PerformancesController(ILogger<PerformancesController> logger, 
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
        public async Task<ActionResult<IEnumerable<PerformanceDto>>> GetPerformances(Guid eventId,
            string? title, string? searchQuery,
            int pageNumber = 1, int pageSize = 10,
            bool includeDetail = false)
        {
            try
            {
                if (pageSize > maxPageSize)
                {
                    pageSize = maxPageSize;
                }

                if (!await _eventHostRepository.EventExistsAsync(eventId))
                {
                    _logger.LogInformation("Event with id {EventId} was not found when trying to get all performances.", 
                        eventId);
                    return NotFound();
                }

                var performanceEntities = await _eventHostRepository
                    .GetPerformancesForEventAsync(eventId, includeDetail);

                if (includeDetail)
                {
                    return Ok(_mapper.Map<IEnumerable<PerformanceDetailDto>>(performanceEntities));
                }

                return Ok(_mapper.Map<IEnumerable<PerformanceDto>>(performanceEntities));
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
        public async Task<ActionResult> GetPerformance(Guid eventId, Guid performanceId, bool includeDetail)
        {
            if (!await _eventHostRepository.EventExistsAsync(eventId))
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to get all performances.",
                    eventId);
                return NotFound();
            }

            var performanceEntity = await _eventHostRepository.GetPerformanceForEventAsync(eventId, performanceId, includeDetail);

            if (performanceEntity == null)
            {
                _logger.LogInformation("The requested performance id {PerfId} was not for event id {EventId}", performanceId, eventId);
                return NotFound();
            }

            if (includeDetail)
            {
                return Ok(_mapper.Map<PerformanceDetailDto>(performanceEntity));
            }

            return Ok(_mapper.Map<PerformanceDto>(performanceEntity));
        }


        [HttpPost]
        public async Task<ActionResult<PerformanceDto>> CreatePerformance(
            Guid eventId,
            [FromBody] PerformanceForUpdateDto performanceForUpdateDto)
        {
            if (!await _eventHostRepository.EventExistsAsync(eventId))
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to create a performance.",
                    eventId);
                return NotFound();
            }

            var newPerformanceEntity = _mapper.Map<Entities.Performance>(performanceForUpdateDto);

            await _eventHostRepository.AddPerformanceToEventAsync(eventId, newPerformanceEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                var performanceToReturnDto = _mapper.Map<PerformanceDto>(newPerformanceEntity);

                return CreatedAtRoute("GetPerformance",
                    new
                    {
                        eventId = performanceToReturnDto.EventId,
                        performanceId = performanceToReturnDto.Id
                    },
                    performanceToReturnDto);
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to create a new performance for event id {eventId}.");
        }


        [HttpPut("{performanceId}")]
        public async Task<ActionResult> UpdatePerformance(
            Guid eventId,
            Guid performanceId,
            PerformanceForUpdateDto performanceForUpdateDto)
        {
            if (!await _eventHostRepository.EventExistsAsync(eventId))
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to update a performance.",
                    eventId);
                return NotFound();
            }

            var performanceEntity = await _eventHostRepository
                .GetPerformanceForEventAsync(eventId, performanceId, false);

            if (performanceEntity == null)
            {
                _logger.LogInformation("Performance with id {PerformanceId} was not found when trying to update the performance.", performanceId);
                return NotFound();
            }

            // overwrite the entity with the corresponding values in the dto
            _mapper.Map(performanceForUpdateDto, performanceEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to update the performance with id {performanceId} for event id {eventId}.");
        }


        [HttpPatch("{performanceId}")]
        public async Task<ActionResult> PartiallyUpdatePerformance(
            Guid eventId,
            Guid performanceId,
            JsonPatchDocument<PerformanceForUpdateDto> patchDocument)
        {
            if (!await _eventHostRepository.EventExistsAsync(eventId))
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to partially update a performance.",
                    eventId);
                return NotFound();
            }

            var performanceEntity = await _eventHostRepository
                .GetPerformanceForEventAsync(eventId, performanceId, false);

            if (performanceEntity == null)
            {
                _logger.LogInformation("Performance with id {PerformanceId} was not found when trying to partially update the performance.", performanceId);
                return NotFound();
            }

            var performanceToPatchDto = _mapper.Map<PerformanceForUpdateDto>(performanceEntity);

            patchDocument.ApplyTo(performanceToPatchDto, ModelState);

            // check for any errors in the patch document
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("An issue was found in the patch document when trying to patch id {PerformanceId}.", performanceId);
                return BadRequest(ModelState);
            }

            // check for broken validation rules on the model
            if (!TryValidateModel(performanceToPatchDto))
            {
                _logger.LogInformation("Validation issue(s) was/were found when trying to patch id {PerformanceId}.", performanceId);
                return BadRequest(ModelState);
            }

            // overwrite the entity with the corresponding dto values
            _mapper.Map(performanceToPatchDto, performanceEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to partially update the performance with id {performanceId} for event id {eventId}.");

        }


        [HttpDelete("{performanceId}")]
        public async Task<ActionResult> DeletePerformance(Guid eventId, Guid performanceId)
        {
            if (!await _eventHostRepository.EventExistsAsync(eventId))
            {
                _logger.LogInformation("Event with id {EventId} was not found when trying to delete a performance with id {PerformanceId}.",
                    eventId, performanceId);
                return NotFound();
            }

            var performanceEntity = await _eventHostRepository
                .GetPerformanceForEventAsync(eventId, performanceId, false);

            if (performanceEntity == null)
            {
                _logger.LogInformation("Performance with id {PerformanceId} was not found when trying to partially update the performance.", performanceId);
                return NotFound();
            }

            _eventHostRepository.DeletePerformance(performanceEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                _mailService.Send($"Performance Deleted: {performanceEntity.Title}",
                $"A performance called '{performanceEntity.Title}' for event id '{eventId}' was deleted.");
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to delete the performance '{performanceEntity.Title}' with id {performanceId} for event id {eventId}.");
        }
    }
}
