using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.SeriesAsset;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/seriesassets")]
    [ApiController]
    public class SeriesAssetsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public SeriesAssetsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet("{seriesAssetId}", Name = "GetSeriesAssetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SeriesAssetDto>> GetSeriesAssetById(Guid seriesAssetId,
            ApiVersion version)
        {
            if (seriesAssetId == Guid.Empty)
            {
                return BadRequest();
            }

            var seriesAssetsFromRepo = await _eventHostRepository.GetSeriesAssetByIdAsync(seriesAssetId);

            return Ok(_mapper.Map<SeriesAssetDto>(seriesAssetsFromRepo));
        }


        [HttpPut("{seriesAssetId}", Name = "UpdateSeriesAsset")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateSeriesAsset(Guid seriesAssetId,
            SeriesAssetForUpdateDto seriesAssetDto,
            ApiVersion version)
        {
            if (seriesAssetId == Guid.Empty || seriesAssetDto == null)
            {
                return BadRequest();
            }

            var seriesAssetOfTypeExists = await _eventHostRepository
                .SeriesAssetExistsAsync(seriesAssetDto.SeriesId, seriesAssetDto.AssetTypeId);
            
            if (seriesAssetOfTypeExists)
            {
                ModelState.AddModelError(nameof(SeriesAssetForUpdateDto),
                    $"An asset of type '{seriesAssetDto.AssetTypeId}' currently exists for this series.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var seriesAssetFromRepo = await _eventHostRepository.GetSeriesAssetByIdAsync(seriesAssetId);

            if (seriesAssetFromRepo == null)
            {
                var seriesAssetToReturn = await InsertSeriesAssetAsync(seriesAssetDto);

                return CreatedAtRoute("GetSeriesAssetById",
                    new
                    {
                        seriesAssetId = seriesAssetToReturn.Id,
                        version = $"{version}"
                    },
                    seriesAssetToReturn);
            }

            _mapper.Map(seriesAssetDto, seriesAssetFromRepo);
            //await _eventHostRepository.UpdateSeriesAssetAsync(seriesAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{seriesAssetId}", Name = "PartiallyUpdateSeriesAsset")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateSeriesAsset(Guid seriesAssetId,
                    JsonPatchDocument<SeriesAssetForUpdateDto> patchDocument,
                    ApiVersion version)
        {
            if (seriesAssetId == Guid.Empty)
            {
                return BadRequest();
            }

            var seriesAssetFromRepo = await _eventHostRepository.GetSeriesAssetByIdAsync(seriesAssetId);

            if (seriesAssetFromRepo == null)
            {
                return NotFound();
            }

            var seriesToPatch = _mapper.Map<SeriesAssetForUpdateDto>(seriesAssetFromRepo);

            patchDocument.ApplyTo(seriesToPatch, ModelState);

            if (!TryValidateModel(seriesToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(seriesToPatch, seriesAssetFromRepo);
            //await _eventHostRepository.UpdateSeriesAssetAsync(seriesAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{seriesAssetId}", Name = "DeleteSeriesAsset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteSeriesAsset(Guid seriesAssetId,
            ApiVersion version)
        {
            if (seriesAssetId == Guid.Empty)
            {
                return BadRequest();
            }

            var seriesAssetFromRepo = await _eventHostRepository
                .GetSeriesAssetByIdAsync(seriesAssetId);

            if (seriesAssetFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteSeriesAsset(seriesAssetFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************

        [HttpHead(Name = "GetSeriesAssetMetadata")]
        public ActionResult GetSeriesAssetMetadata(ApiVersion version)
        {
            return Ok();
        }

        [HttpOptions(Name = "GetSeriesAssetOptions")]
        public ActionResult GetSeriesAssetOptions(ApiVersion version)
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

        private async Task<SeriesAssetDto> InsertSeriesAssetAsync(SeriesAssetForUpdateDto seriesAssetDto)
        {
            var seriesAsset = _mapper.Map<SeriesAsset>(seriesAssetDto);
            await _eventHostRepository.AddSeriesAssetAsync(seriesAsset);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<SeriesAssetDto>(seriesAsset);
        }

        #endregion

    }
}