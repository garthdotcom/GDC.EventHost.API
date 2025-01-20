using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.EventAsset;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GDC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/eventassets")]
    [ApiController]
    public class EventAssetsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public EventAssetsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet("{eventAssetId}", Name = "GetEventAssetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<EventAssetDto>> GetEventAssetById(Guid eventAssetId,
            ApiVersion version)
        {
            if (eventAssetId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventAssetsFromRepo = await _eventHostRepository.GetEventAssetByIdAsync(eventAssetId);

            return Ok(_mapper.Map<EventAssetDto>(eventAssetsFromRepo));
        }


        [HttpPut("{eventAssetId}", Name = "UpdateEventAsset")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateEventAsset(Guid eventAssetId,
            EventAssetForUpdateDto eventAssetDto,
            ApiVersion version)
        {
            if (eventAssetId == Guid.Empty || eventAssetDto == null)
            {
                return BadRequest();
            }

            if (await _eventHostRepository.EventAssetExistsAsync(eventAssetDto.EventId, eventAssetDto.AssetTypeId))
            {
                ModelState.AddModelError(nameof(EventAssetForUpdateDto),
                    $"An asset of type '{eventAssetDto.AssetTypeId}' currently exists for this event.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var eventAssetFromRepo = await _eventHostRepository.GetEventAssetByIdAsync(eventAssetId);

            if (eventAssetFromRepo == null)
            {
                var eventAssetToReturn = await InsertEventAssetAsync(eventAssetDto);

                return CreatedAtRoute("GetEventAssetById",
                    new
                    {
                        eventAssetId = eventAssetToReturn.Id,
                        version = $"{version}"
                    },
                    eventAssetToReturn);
            }

            _mapper.Map(eventAssetDto, eventAssetFromRepo);
            //await _eventHostRepository.UpdateEventAssetAsync(eventAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{eventAssetId}", Name = "PartiallyUpdateEventAsset")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateEventAssetAsync(Guid eventAssetId,
            JsonPatchDocument<EventAssetForUpdateDto> patchDocument,
            ApiVersion version)
        {
            if (eventAssetId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventAssetFromRepo = await _eventHostRepository.GetEventAssetByIdAsync(eventAssetId);

            if (eventAssetFromRepo == null)
            {
                return NotFound();
            }

            var eventToPatch = _mapper.Map<EventAssetForUpdateDto>(eventAssetFromRepo);

            patchDocument.ApplyTo(eventToPatch, ModelState);

            if (!TryValidateModel(eventToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(eventToPatch, eventAssetFromRepo);
            //await _eventHostRepository.UpdateEventAssetAsync(eventAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{eventAssetId}", Name = "DeleteEventAsset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEventAssetAsync(Guid eventAssetId,
            ApiVersion version)
        {
            if (eventAssetId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventAssetFromRepo = await _eventHostRepository.GetEventAssetByIdAsync(eventAssetId);

            if (eventAssetFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteEventAsset(eventAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************

        [HttpHead(Name = "GetEventAssetMetadata")]
        public ActionResult GetEventAssetMetadata(ApiVersion version)
        {
            return Ok();
        }

        [HttpOptions(Name = "GetEventAssetOptions")]
        public ActionResult GetEventAssetOptions(ApiVersion version)
        {
            Response.Headers.Add("Allow", "GET,PUT,PATCH,DELETE,OPTIONS");
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

        private async Task<EventAssetDto> InsertEventAssetAsync(EventAssetForUpdateDto eventAssetDto)
        {
            var eventAsset = _mapper.Map<EventAsset>(eventAssetDto);
            await _eventHostRepository.AddEventAssetAsync(eventAsset);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<EventAssetDto>(eventAsset);
        }

        #endregion
    }
}