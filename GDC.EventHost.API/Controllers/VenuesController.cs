using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.SeatingPlan;
using GDC.EventHost.Shared.Venue;
using GDC.EventHost.Shared.VenueAsset;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using GDC.EventHost.Shared.Performance;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/venues")]
    [ApiController]
    public class VenuesController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public VenuesController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetVenues")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VenueDetailDto>>> GetVenues(
            [FromQuery] VenueResourceParameters resourceParms,
            ApiVersion version)
        {
            var venuesFromRepo = await _eventHostRepository.GetVenuesAsync(resourceParms);

            var venueDetailDtos = _mapper.Map<IEnumerable<VenueDetailDto>>(venuesFromRepo);

            foreach (var venueDetailDto in venueDetailDtos)
            {
                foreach (var performance in venueDetailDto.Performances)
                {
                    performance.TicketCount = _eventHostRepository
                        .GetPerformanceTicketCount(performance.Id);
                }
            }

            return Ok(venueDetailDtos);
        }


        [HttpGet("{venueId}", Name = "GetVenueById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VenueDetailDto>> GetVenueById(Guid venueId,
            ApiVersion version)
        {
            if (venueId == Guid.Empty)
            {
                return BadRequest();
            }

            var venueFromRepo = await _eventHostRepository.GetVenueByIdAsync(venueId);

            if (venueFromRepo == null)
            {
                return NotFound();
            }

            var venueDetailDto = _mapper.Map<VenueDetailDto>(venueFromRepo);

            foreach (var asset in venueFromRepo.VenueAssets)
            {
                venueDetailDto.VenueAssets
                    .Add(_mapper.Map<VenueAssetDto>(asset));
            }
            foreach (var seatingPlan in venueFromRepo.SeatingPlans)
            {
                venueDetailDto.SeatingPlans
                    .Add(_mapper.Map<SeatingPlanDto>(seatingPlan));
            }
            //foreach (var performance in venueFromRepo.Performances)
            //{
            //    venueDetailDto.Performances
            //        .Add(_mapper.Map<PerformanceDetailDto>(performance));
            //}
            foreach (var performance in venueDetailDto.Performances)
            {
                performance.TicketCount = _eventHostRepository
                    .GetPerformanceTicketCount(performance.Id);
            }

            return Ok(venueDetailDto);
        }


        [HttpGet("list", Name = "GetVenuesForList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VenueDto>>> GetVenuesForList(ApiVersion version)
        {
            var eventTypeFromRepo = await _eventHostRepository.GetVenuesAsync();

            return Ok(_mapper.Map<IEnumerable<VenueDto>>(eventTypeFromRepo));
        }


        [HttpPost(Name = "CreateVenue")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VenueDetailDto>> CreateVenue(VenueForUpdateDto venueDto,
            ApiVersion version)
        {
            if (venueDto == null)
            {
                return BadRequest();
            }

            var venueToReturn = await InsertVenueAsync(venueDto);

            return CreatedAtRoute("GetVenueById",
                new
                {
                    venueId = venueToReturn.Id,
                    version = $"{version}"
                },
                venueToReturn);
        }


        [HttpPut("{venueId}", Name = "UpdateVenue")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateVenue(Guid venueId, VenueForUpdateDto venueDto,
            ApiVersion version)
        {
            if (venueId == Guid.Empty || venueDto == null)
            {
                return BadRequest();
            }

            var venueFromRepo = await _eventHostRepository
                .GetVenueByIdAsync(venueId, false);

            if (venueFromRepo == null)
            {
                var venueToReturn = await InsertVenueAsync(venueDto);

                return CreatedAtRoute("GetVenueById",
                    new
                    {
                        venueId = venueToReturn.Id,
                        version = $"{version}"
                    },
                    venueToReturn);
            }

            _mapper.Map(venueDto, venueFromRepo);
            //_eventHostRepository.UpdateVenue(venueFromRepo);
            await _eventHostRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{venueId}", Name = "PartiallyUpdateVenue")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateVenue(Guid venueId,
            JsonPatchDocument<VenueForUpdateDto> patchDocument,
            ApiVersion version)
        {
            if (venueId == Guid.Empty)
            {
                return BadRequest();
            }

            var venueExists = await _eventHostRepository
                .VenueExistsAsync(venueId);

            if (!venueExists)
            {
                var venueDto = new VenueForUpdateDto();

                patchDocument.ApplyTo(venueDto, ModelState);

                if (!TryValidateModel(venueDto))
                {
                    return ValidationProblem(ModelState);
                }

                var venueToReturn = await InsertVenueAsync(venueDto);

                return CreatedAtRoute("GetVenueById",
                    new
                    {
                        venueId = venueToReturn.Id,
                        version = $"{version}"
                    },
                    venueToReturn);
            }

            var venueFromRepo = await _eventHostRepository
                .GetVenueByIdAsync(venueId, false);

            var venueToPatch = _mapper.Map<VenueForUpdateDto>(venueFromRepo);

            patchDocument.ApplyTo(venueToPatch, ModelState);

            if (!TryValidateModel(venueToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(venueToPatch, venueFromRepo);
            //_eventHostRepository.UpdateVenue(venueFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{venueId}", Name = "DeleteVenue")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVenue(Guid venueId,
            ApiVersion version)
        {
            if (venueId == Guid.Empty)
            {
                return BadRequest();
            }

            var venueFromRepo = await _eventHostRepository
                .GetVenueByIdAsync(venueId, false);

            if (venueFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.SoftDeleteVenue(venueFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************
        // Venue Assets
        //*************************************************************************************

        // the resource parms allow us to filter by asset type

        [HttpGet("{venueId}/assets", Name = "GetAssetsForVenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VenueAssetDto>>> GetAssetsForVenue(Guid venueId,
            [FromQuery] AssetResourceParameters resourceParms,
            ApiVersion version)
        {
            if (venueId == Guid.Empty)
            {
                return BadRequest();
            }

            var venueAssetsFromRepo = await _eventHostRepository
                .GetVenueAssetsAsync(venueId, resourceParms);

            return Ok(_mapper.Map<IEnumerable<VenueAssetDto>>(venueAssetsFromRepo));
        }


        [HttpPost("{venueId}/assets", Name = "AddAssetToVenue")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VenueAssetDto>> AddAssetToVenue(Guid venueId,
            VenueAssetForUpdateDto assetForUpdateDto,
            ApiVersion version)
        {
            if (venueId == Guid.Empty || assetForUpdateDto == null)
            {
                return BadRequest();
            }

            var venueAssetOfTypeExists = await _eventHostRepository
                .VenueAssetExistsAsync(venueId, assetForUpdateDto.AssetTypeId);

            if (venueAssetOfTypeExists)
            {
                ModelState.AddModelError(nameof(VenueAssetForUpdateDto),
                    $"An asset of type '{assetForUpdateDto.AssetTypeId}' currently exists for this venue.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var venueAsset = _mapper.Map<VenueAsset>(assetForUpdateDto);

            venueAsset.VenueId = venueId;

            await _eventHostRepository.AddVenueAssetAsync(venueAsset);

            await _eventHostRepository.SaveChangesAsync();

            var newVenueAssetDto = _mapper.Map<VenueAssetDto>(venueAsset);

            return CreatedAtRoute("GetVenueAssetById",
                new
                {
                    venueAssetId = newVenueAssetDto.Id,
                    version = $"{version}"
                },
                newVenueAssetDto);
        }


        [HttpGet("{venueId}/layouts", Name = "GetSeatingPlansForVenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeatingPlanDto>>> GetSeatingPlansForVenue(Guid venueId,
            ApiVersion version)
        {
            if (venueId == Guid.Empty)
            {
                return BadRequest();
            }

            var layoutsFromRepo = await _eventHostRepository
                .GetVenueSeatingPlansAsync(venueId);

            return Ok(_mapper.Map<IEnumerable<SeatingPlanDto>>(layoutsFromRepo));
        }

        //*************************************************************************************

        [HttpHead(Name = "GetVenueMetadata")]
        public ActionResult GetVenueMetadata(ApiVersion version)
        {
            return Ok();
        }

        [HttpOptions(Name = "GetVenueOptions")]
        public ActionResult GetVenueOptions(ApiVersion version)
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

        private async Task<VenueDetailDto> InsertVenueAsync(VenueForUpdateDto venueDto)
        {
            var venue = _mapper.Map<Venue>(venueDto);
            await _eventHostRepository.AddVenueAsync(venue);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<VenueDetailDto>(venue);
        }

        private async Task<VenueAssetDto> InsertVenueAssetAsync(VenueAssetForUpdateDto assetDto)
        {
            var asset = _mapper.Map<VenueAsset>(assetDto);
            await _eventHostRepository.AddVenueAssetAsync(asset);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<VenueAssetDto>(asset);
        }

        #endregion
    }
}