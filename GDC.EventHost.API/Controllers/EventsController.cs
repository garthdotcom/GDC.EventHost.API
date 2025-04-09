using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Event;
using GDC.EventHost.Shared.EventAsset;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using GDC.EventHost.Shared.Performance;
using GDC.EventHost.Shared.SeatingPlan;
using GDC.EventHost.Shared.Venue;
using GDC.EventHost.Shared.VenueAsset;
using GDC.EventHost.Shared;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public EventsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetEvents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDetailDto>>> GetEvents(
            [FromQuery] EventResourceParameters resourceParms,
            ApiVersion version)
        {
            var eventFromRepo = await _eventHostRepository.GetEventsAsync(resourceParms);

            var eventDtos = _mapper.Map<IEnumerable<EventDetailDto>>(eventFromRepo);

            foreach (var eventDto in eventDtos)
            {
                foreach (var performanceDetailDto in eventDto.Performances)
                {
                    performanceDetailDto.TicketCount = _eventHostRepository
                        .GetPerformanceTicketCount(performanceDetailDto.Id);
                }
            }

            return Ok(eventDtos);
        }


        [HttpGet("{eventId}", Name = "GetEventById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventDetailDto>> GetEventById(Guid eventId,
            ApiVersion version)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventFromRepo = await _eventHostRepository.GetEventByIdAsync(eventId);

            if (eventFromRepo == null)
            {
                return NotFound();
            }

            var eventDetailDto = _mapper.Map<EventDetailDto>(eventFromRepo);

            foreach (var performanceDetailDto in eventDetailDto.Performances)
            {
                performanceDetailDto.TicketCount = _eventHostRepository
                    .GetPerformanceTicketCount(performanceDetailDto.Id);
            }

            return Ok(eventDetailDto);
        }


        [HttpPost(Name = "CreateEvent")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventDetailDto>> CreateEvent(EventForUpdateDto eventDto,
            ApiVersion version)
        {
            if (eventDto == null)
            {
                return BadRequest();
            }

            var eventToReturn = await InsertEventAsync(eventDto);

            return CreatedAtRoute("GetEventById",
                new
                {
                    eventId = eventToReturn.Id,
                    version = $"{version}"
                },
                eventToReturn);
        }


        [HttpPut("{eventId}", Name = "UpdateEvent")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateEvent(Guid eventId, EventForUpdateDto eventDto,
            ApiVersion version)
        {
            if (eventId == Guid.Empty || eventDto == null)
            {
                return BadRequest();
            }

            var eventFromRepo = await _eventHostRepository.GetEventByIdAsync(eventId);

            if (eventFromRepo == null)
            {
                var eventToReturn = await InsertEventAsync(eventDto);

                return CreatedAtRoute("GetEventById",
                    new
                    {
                        eventId = eventToReturn.Id,
                        version = $"{version}"
                    },
                    eventToReturn);
            }

            _mapper.Map(eventDto, eventFromRepo);

            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpPatch("{eventId}", Name = "PartiallyUpdateEvent")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateEvent(Guid eventId,
            JsonPatchDocument<EventForUpdateDto> patchDocument,
            ApiVersion version)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }
            var eventExists = await _eventHostRepository.EventExistsAsync(eventId);

            if (!eventExists)
            {
                var eventDto = new EventForUpdateDto();

                patchDocument.ApplyTo(eventDto, ModelState);

                if (!TryValidateModel(eventDto))
                {
                    return ValidationProblem(ModelState);
                }

                var eventToReturn = await InsertEventAsync(eventDto);

                return CreatedAtRoute("GetEventById",
                    new
                    {
                        eventId = eventToReturn.Id,
                        version = $"{version}"
                    },
                    eventToReturn);
            }

            var eventFromRepo = await _eventHostRepository.GetEventByIdAsync(eventId, false);

            var eventToPatch = _mapper.Map<EventForUpdateDto>(eventFromRepo);

            patchDocument.ApplyTo(eventToPatch, ModelState);

            if (!TryValidateModel(eventToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(eventToPatch, eventFromRepo);

            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{eventId}", Name = "DeleteEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEvent(Guid eventId,
            ApiVersion version)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventFromRepo = await _eventHostRepository.GetEventByIdAsync(eventId, false);

            if (eventFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.SoftDeleteEvent(eventFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************
        // Event Assets
        //*************************************************************************************

        // the resource parms allow us to filter by asset type

        [HttpGet("{eventId}/assets", Name = "GetAssetsForEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventAssetDto>>> GetAssetsForEvent(Guid eventId,
            [FromQuery] AssetResourceParameters resourceParms,
            ApiVersion version)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventAssetsFromRepo = await _eventHostRepository.GetEventAssetsAsync(eventId, resourceParms);

            return Ok(_mapper.Map<IEnumerable<EventAssetDto>>(eventAssetsFromRepo));
        }


        [HttpPost("{eventId}/assets", Name = "AddAssetToEvent")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventAssetDto>> AddAssetToEvent(Guid eventId,
            EventAssetForUpdateDto assetForUpdateDto,
            ApiVersion version)
        {
            if (eventId == Guid.Empty || assetForUpdateDto == null)
            {
                return BadRequest();
            }

            var eventAssetExists = await _eventHostRepository
                .EventAssetExistsAsync(eventId, assetForUpdateDto.AssetTypeId);

            if (eventAssetExists)
            {
                ModelState.AddModelError(nameof(EventAssetForUpdateDto),
                    $"An asset of type '{assetForUpdateDto.AssetTypeId}' currently exists for this event.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var eventAsset = _mapper.Map<EventAsset>(assetForUpdateDto);

            eventAsset.EventId = eventId;

            await _eventHostRepository.AddEventAssetAsync(eventAsset);

            await _eventHostRepository.SaveChangesAsync();

            var newEventAssetDto = _mapper.Map<EventAssetDto>(eventAsset);

            return CreatedAtRoute("GetEventAssetById",
                new
                {
                    eventAssetId = newEventAssetDto.Id,
                    version = $"{version}"
                },
                newEventAssetDto);
        }


        //*************************************************************************************


        [HttpHead(Name = "GetEventMetadata")]
        public ActionResult GetEventMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetEventOptions")]
        public ActionResult GetEventOptions(ApiVersion version)
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,PATCH,DELETE,OPTIONS");
            return Ok();
        }


        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value
                .InvalidModelStateResponseFactory(ControllerContext);
        }


        #region Private Methods

        private async Task<EventDetailDto> InsertEventAsync(EventForUpdateDto eventDto)
        {
            var eventToPersist = _mapper.Map<Event>(eventDto);
            eventToPersist.StatusId = Enums.StatusEnum.Pending;
            await _eventHostRepository.AddEventAsync(eventToPersist);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<EventDetailDto>(eventToPersist);
        }

        #endregion
    }
}