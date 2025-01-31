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

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/events")]
    [ApiController]
    public class EventsController : Controller
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
            var evtFromRepo = await _eventHostRepository.GetEventsAsync(resourceParms);

            var evtDtos = _mapper.Map<IEnumerable<EventDetailDto>>(evtFromRepo);

            foreach (var evtDto in evtDtos)
            {
                foreach (var performanceDetailDto in evtDto.Performances)
                {
                    performanceDetailDto.TicketCount = _eventHostRepository
                        .GetPerformanceTicketCount(performanceDetailDto.Id);
                }
            }

            return Ok(evtDtos);
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

            foreach (var asset in eventFromRepo.EventAssets)
            {
                eventDetailDto.EventAssets
                    .Add(_mapper.Map<EventAssetDto>(asset));
            }
            foreach (var performance in eventFromRepo.Performances)
            {
                eventDetailDto.Performances
                    .Add(_mapper.Map<PerformanceDetailDto>(performance));
            }
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
        public async Task<ActionResult<EventDetailDto>> CreateEvent(EventForUpdateDto evtDto,
            ApiVersion version)
        {
            if (evtDto == null)
            {
                return BadRequest();
            }

            var evtToReturn = await InsertEventAsync(evtDto);

            return CreatedAtRoute("GetEventById",
                new
                {
                    evtId = evtToReturn.Id,
                    version = $"{version}"
                },
                evtToReturn);
        }


        [HttpPut("{evtId}", Name = "UpdateEvent")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateEvent(Guid evtId, EventForUpdateDto evtDto,
            ApiVersion version)
        {
            if (evtId == Guid.Empty || evtDto == null)
            {
                return BadRequest();
            }

            var evtFromRepo = await _eventHostRepository.GetEventByIdAsync(evtId);

            if (evtFromRepo == null)
            {
                var evtToReturn = await InsertEventAsync(evtDto);

                return CreatedAtRoute("GetEventById",
                    new
                    {
                        evtId = evtToReturn.Id,
                        version = $"{version}"
                    },
                    evtToReturn);
            }

            _mapper.Map(evtDto, evtFromRepo);
            //await _eventHostRepository.UpdateEventAsync(evtFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpPatch("{evtId}", Name = "PartiallyUpdateEvent")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateEvent(Guid evtId,
            JsonPatchDocument<EventForUpdateDto> patchDocument,
            ApiVersion version)
        {
            if (evtId == Guid.Empty)
            {
                return BadRequest();
            }
            var eventExists = await _eventHostRepository.EventExistsAsync(evtId);

            if (!eventExists)
            {
                var evtDto = new EventForUpdateDto();

                patchDocument.ApplyTo(evtDto, ModelState);

                if (!TryValidateModel(evtDto))
                {
                    return ValidationProblem(ModelState);
                }

                var evtToReturn = await InsertEventAsync(evtDto);

                return CreatedAtRoute("GetEventById",
                    new
                    {
                        evtId = evtToReturn.Id,
                        version = $"{version}"
                    },
                    evtToReturn);
            }

            var evtFromRepo = await _eventHostRepository.GetEventByIdAsync(evtId, false);

            var evtToPatch = _mapper.Map<EventForUpdateDto>(evtFromRepo);

            patchDocument.ApplyTo(evtToPatch, ModelState);

            if (!TryValidateModel(evtToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(evtToPatch, evtFromRepo);
            //await _eventHostRepository.UpdateEventAsync(evtFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{evtId}", Name = "DeleteEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEvent(Guid evtId,
            ApiVersion version)
        {
            if (evtId == Guid.Empty)
            {
                return BadRequest();
            }

            var evtFromRepo = await _eventHostRepository.GetEventByIdAsync(evtId, false);

            if (evtFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.SoftDeleteEvent(evtFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************
        // Event Assets
        //*************************************************************************************

        // the resource parms allow us to filter by asset type

        [HttpGet("{evtId}/assets", Name = "GetAssetsForEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventAssetDto>>> GetAssetsForEvent(Guid evtId,
            [FromQuery] AssetResourceParameters resourceParms,
            ApiVersion version)
        {
            if (evtId == Guid.Empty)
            {
                return BadRequest();
            }

            var evtAssetsFromRepo = await _eventHostRepository.GetEventAssetsAsync(evtId, resourceParms);

            return Ok(_mapper.Map<IEnumerable<EventAssetDto>>(evtAssetsFromRepo));
        }


        [HttpPost("{evtId}/assets", Name = "AddAssetToEvent")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventAssetDto>> AddAssetToEvent(Guid evtId,
            EventAssetForUpdateDto assetForUpdateDto,
            ApiVersion version)
        {
            if (evtId == Guid.Empty || assetForUpdateDto == null)
            {
                return BadRequest();
            }

            var eventAssetExists = await _eventHostRepository
                .EventAssetExistsAsync(evtId, assetForUpdateDto.AssetTypeId);

            if (eventAssetExists)
            {
                ModelState.AddModelError(nameof(EventAssetForUpdateDto),
                    $"An asset of type '{assetForUpdateDto.AssetTypeId}' currently exists for this event.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var evtAsset = _mapper.Map<EventAsset>(assetForUpdateDto);

            evtAsset.EventId = evtId;

            await _eventHostRepository.AddEventAssetAsync(evtAsset);

            await _eventHostRepository.SaveChangesAsync();

            var newEventAssetDto = _mapper.Map<EventAssetDto>(evtAsset);

            return CreatedAtRoute("GetEventAssetById",
                new
                {
                    evtAssetId = newEventAssetDto.Id,
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

        private async Task<EventDetailDto> InsertEventAsync(EventForUpdateDto evtDto)
        {
            var evt = _mapper.Map<Event>(evtDto);
            await _eventHostRepository.AddEventAsync(evt);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<EventDetailDto>(evt);
        }

        #endregion
    }
}