using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Asset;
using GDC.EventHost.Shared.Series;
using GDC.EventHost.Shared.SeriesAsset;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using GDC.EventHost.Shared.Performance;
using GDC.EventHost.Shared.SeatingPlan;
using GDC.EventHost.Shared.Venue;
using GDC.EventHost.Shared.VenueAsset;
using GDC.EventHost.Shared.Event;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/series")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public SeriesController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetSeries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeriesDetailDto>>> GetSeries(
            [FromQuery] SeriesResourceParameters seriesResourceParameters,
            ApiVersion version)
        {
            var seriesFromRepo = await _eventHostRepository.GetSeriesAsync(seriesResourceParameters);

            return Ok(_mapper.Map<IEnumerable<SeriesDetailDto>>(seriesFromRepo));
        }


        [HttpGet("{seriesId}", Name = "GetSeriesById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SeriesDetailDto>> GetSeriesById(Guid seriesId,
            ApiVersion version)
        {
            if (seriesId == Guid.Empty)
            {
                return BadRequest();
            }

            var seriesFromRepo = await _eventHostRepository.GetSeriesByIdAsync(seriesId);

            if (seriesFromRepo == null)
            {
                return NotFound();
            }

            var seriesDetailDto = _mapper.Map<SeriesDetailDto>(seriesFromRepo);

            foreach (var asset in seriesFromRepo.SeriesAssets)
            {
                seriesDetailDto.SeriesAssets
                    .Add(_mapper.Map<SeriesAssetDto>(asset));
            }
            foreach (var evt in seriesFromRepo.Events)
            {
                seriesDetailDto.Events
                    .Add(_mapper.Map<EventDetailDto>(evt));
            }

            return Ok(_mapper.Map<SeriesDetailDto>(seriesFromRepo));
        }


        [HttpGet("list", Name = "GetSeriesForList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeriesDto>>> GetSeriesForList(ApiVersion version)
        {
            var eventTypeFromRepo = await _eventHostRepository.GetSeriesAsync();

            return Ok(_mapper.Map<IEnumerable<SeriesDto>>(eventTypeFromRepo));
        }


        [HttpPost(Name = "CreateSeries")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SeriesDetailDto>> CreateSeries(SeriesForUpdateDto seriesDto,
            ApiVersion version)
        {
            if (seriesDto == null)
            {
                return BadRequest();
            }

            var seriesToReturn = await InsertSeriesAsync(seriesDto);

            return CreatedAtRoute("GetSeriesById",
                new
                {
                    seriesId = seriesToReturn.Id,
                    version = $"{version}"
                },
                seriesToReturn);
        }


        [HttpPut("{seriesId}", Name = "UpdateSeriesAsync")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateSeriesAsync(Guid seriesId, SeriesForUpdateDto seriesDto,
            ApiVersion version)
        {
            if (seriesId == Guid.Empty || seriesDto == null)
            {
                return BadRequest();
            }

            var seriesFromRepo = await _eventHostRepository.GetSeriesByIdAsync(seriesId, false);

            if (seriesFromRepo == null)
            {
                var seriesToReturn = await InsertSeriesAsync(seriesDto);

                return CreatedAtRoute("GetSeriesById",
                    new
                    {
                        seriesId = seriesToReturn.Id,
                        version = $"{version}"
                    },
                    seriesToReturn);
            }

            _mapper.Map(seriesDto, seriesFromRepo);
            //_eventHostRepository.UpdateSeriesAsync(seriesFromRepo);
            await _eventHostRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{seriesId}", Name = "PartiallyUpdateSeriesAsync")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateSeriesAsync(Guid seriesId,
            JsonPatchDocument<SeriesForUpdateDto> patchDocument,
            ApiVersion version)
        {
            if (seriesId == Guid.Empty)
            {
                return BadRequest();
            }
            var seriesExists = await _eventHostRepository.SeriesExistsAsync(seriesId);

            if (!seriesExists)
            {
                var seriesDto = new SeriesForUpdateDto();

                patchDocument.ApplyTo(seriesDto, ModelState);

                if (!TryValidateModel(seriesDto))
                {
                    return ValidationProblem(ModelState);
                }

                var seriesToReturn = await InsertSeriesAsync(seriesDto);

                return CreatedAtRoute("GetSeriesById",
                    new
                    {
                        seriesId = seriesToReturn.Id,
                        version = $"{version}"
                    },
                    seriesToReturn);
            }

            var seriesFromRepo = await _eventHostRepository.GetSeriesByIdAsync(seriesId, false);

            var seriesToPatch = _mapper.Map<SeriesForUpdateDto>(seriesFromRepo);

            patchDocument.ApplyTo(seriesToPatch, ModelState);

            if (!TryValidateModel(seriesToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(seriesToPatch, seriesFromRepo);
            //_eventHostRepository.UpdateSeriesAsync(seriesFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{seriesId}", Name = "DeleteSeries")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteSeries(Guid seriesId,
            ApiVersion version)
        {
            if (seriesId == Guid.Empty)
            {
                return BadRequest();
            }

            var seriesFromRepo = await _eventHostRepository.GetSeriesByIdAsync(seriesId, false);

            if (seriesFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.SoftDeleteSeries(seriesFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************
        // Series Assets
        //*************************************************************************************

        // the resource parms allow us to filter by asset type

        [HttpGet("{seriesId}/assets", Name = "GetAssetsForSeries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeriesAssetDto>>> GetAssetsForSeries(Guid seriesId,
            [FromQuery] AssetResourceParameters resourceParms,
            ApiVersion version)
        {
            if (seriesId == Guid.Empty)
            {
                return BadRequest();
            }

            var seriesAssetsFromRepo = await _eventHostRepository
                .GetSeriesAssetsAsync(seriesId, resourceParms);

            return Ok(_mapper.Map<IEnumerable<SeriesAssetDto>>(seriesAssetsFromRepo));
        }


        [HttpPost("{seriesId}/assets", Name = "AddAssetToSeries")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SeriesAssetDto>> AddAssetToSeries(Guid seriesId,
            SeriesAssetForUpdateDto assetForUpdateDto,
            ApiVersion version)
        {
            if (seriesId == Guid.Empty || assetForUpdateDto == null)
            {
                return BadRequest();
            }

            var seriesAssetOfTypeExists = await _eventHostRepository
                .SeriesAssetExistsAsync(seriesId, assetForUpdateDto.AssetTypeId);

            if (seriesAssetOfTypeExists)
            {
                ModelState.AddModelError(nameof(SeriesAssetForUpdateDto),
                    $"An asset of type '{assetForUpdateDto.AssetTypeId}' currently exists for this series.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var seriesAsset = _mapper.Map<SeriesAsset>(assetForUpdateDto);

            seriesAsset.SeriesId = seriesId;

            await _eventHostRepository.AddSeriesAssetAsync(seriesAsset);

            await _eventHostRepository.SaveChangesAsync();

            var newSeriesAssetDto = _mapper.Map<SeriesAssetDto>(seriesAsset);

            return CreatedAtRoute("GetSeriesAssetById",
                new
                {
                    seriesAssetId = newSeriesAssetDto.Id,
                    version = $"{version}"
                },
                newSeriesAssetDto);
        }


        //*************************************************************************************


        [HttpHead(Name = "GetSeriesMetadata")]
        public ActionResult GetSeriesMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetSeriesOptions")]
        public ActionResult GetSeriesOptions(ApiVersion version)
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

        private async Task<SeriesDetailDto> InsertSeriesAsync(SeriesForUpdateDto seriesDto)
        {
            var series = _mapper.Map<Series>(seriesDto);
            await _eventHostRepository.AddSeriesAsync(series);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<SeriesDetailDto>(series);
        }

        private async Task<SeriesAssetDto> InsertSeriesAssetAsync(Guid seriesId, AssetForUpdateDto assetDto)
        {
            var asset = _mapper.Map<SeriesAsset>(assetDto);
            asset.SeriesId = seriesId;
            await _eventHostRepository.AddSeriesAssetAsync(asset);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<SeriesAssetDto>(asset);
        }

        #endregion
    }
}