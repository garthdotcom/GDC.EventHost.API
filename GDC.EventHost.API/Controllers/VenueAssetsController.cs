using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.VenueAsset;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/venueassets")]
    [ApiController]
    public class VenueAssetsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public VenueAssetsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet("{venueAssetId}", Name = "GetVenueAssetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<VenueAssetDto>> GetVenueAssetById(Guid venueAssetId,
            ApiVersion version)
        {
            if (venueAssetId == Guid.Empty)
            {
                return BadRequest();
            }

            var venueAssetsFromRepo = await _eventHostRepository
                .GetVenueAssetByIdAsync(venueAssetId);

            return Ok(_mapper.Map<VenueAssetDto>(venueAssetsFromRepo));
        }


        [HttpPut("{venueAssetId}", Name = "UpdateVenueAsset")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateVenueAsset(Guid venueAssetId,
            VenueAssetForUpdateDto venueAssetDto,
            ApiVersion version)
        {
            if (venueAssetId == Guid.Empty || venueAssetDto == null)
            {
                return BadRequest();
            }

            var venueAssetOfTypeExists = await _eventHostRepository
                .VenueAssetExistsAsync(venueAssetDto.VenueId, venueAssetDto.AssetTypeId);

            if (venueAssetOfTypeExists)
            {
                ModelState.AddModelError(nameof(VenueAssetForUpdateDto),
                    $"An asset of type '{venueAssetDto.AssetTypeId}' currently exists for this venue.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var venueAssetFromRepo = await _eventHostRepository
                .GetVenueAssetByIdAsync(venueAssetId);

            if (venueAssetFromRepo == null)
            {
                var venueAssetToReturn = InsertVenueAsset(venueAssetDto);

                return CreatedAtRoute("GetVenueAssetById",
                    new
                    {
                        venueAssetId = venueAssetToReturn.Id,
                        version = $"{version}"
                    },
                    venueAssetToReturn);
            }

            _mapper.Map(venueAssetDto, venueAssetFromRepo);
            //_eventHostRepository.UpdateVenueAsset(venueAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{venueAssetId}", Name = "PartiallyUpdateVenueAsset")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateVenueAsset(Guid venueAssetId,
                    JsonPatchDocument<VenueAssetForUpdateDto> patchDocument,
                    ApiVersion version)
        {
            if (venueAssetId == Guid.Empty)
            {
                return BadRequest();
            }
             
            var venueAssetFromRepo = await _eventHostRepository
                .GetVenueAssetByIdAsync(venueAssetId);

            if (venueAssetFromRepo == null)
            {
                return NotFound();
            }

            var venueToPatch = _mapper.Map<VenueAssetForUpdateDto>(venueAssetFromRepo);

            patchDocument.ApplyTo(venueToPatch, ModelState);

            if (!TryValidateModel(venueToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(venueToPatch, venueAssetFromRepo);

            //_eventHostRepository.UpdateVenueAsset(venueAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{venueAssetId}", Name = "DeleteVenueAsset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVenueAsset(Guid venueAssetId,
            ApiVersion version)
        {
            if (venueAssetId == Guid.Empty)
            {
                return BadRequest();
            }

            var venueAssetFromRepo = await _eventHostRepository
                .GetVenueAssetByIdAsync(venueAssetId);

            if (venueAssetFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteVenueAsset(venueAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************

        [HttpHead(Name = "GetVenueAssetMetadata")]
        public ActionResult GetVenueAssetMetadata(ApiVersion version)
        {
            return Ok();
        }
         
        [HttpOptions(Name = "GetVenueAssetOptions")]
        public ActionResult GetVenueAssetOptions(ApiVersion version)
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

        private async Task<VenueAssetDto> InsertVenueAsset(VenueAssetForUpdateDto venueAssetDto)
        {
            var venueAsset = _mapper.Map<VenueAsset>(venueAssetDto);
            await _eventHostRepository.AddVenueAssetAsync(venueAsset);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<VenueAssetDto>(venueAsset);
        }

        #endregion
    }
}