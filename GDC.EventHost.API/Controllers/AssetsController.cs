using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Asset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GDC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/assets")]
    [ApiController]
    public class AssetsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public AssetsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetAssets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task <ActionResult<IEnumerable<AssetDetailDto>>> GetAssets(
            [FromQuery] AssetResourceParameters resourceParms,
            ApiVersion version)
        {
            var assetFromRepo = await _eventHostRepository.GetAssetsAsync(resourceParms);

            return Ok(_mapper.Map<IEnumerable<AssetDetailDto>>(assetFromRepo));
        }


        [HttpGet("{assetId}", Name = "GetAssetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AssetDetailDto>> GetAssetById(Guid assetId,
            ApiVersion version)
        {
            if (assetId == Guid.Empty)
            {
                return BadRequest();
            }

            var assetFromRepo = await _eventHostRepository.GetAssetByIdAsync(assetId);

            if (assetFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AssetDetailDto>(assetFromRepo));
        }


        // TODO: support dimensions and details of image assets

        [HttpGet("list", Name = "GetAssetsForList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssetsForList(ApiVersion version)
        {
            var eventTypeFromRepo = await _eventHostRepository.GetAssetsAsync();

            return Ok(_mapper.Map<IEnumerable<AssetDto>>(eventTypeFromRepo));
        }


        [HttpPost(Name = "CreateAsset")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AssetDetailDto>> CreateAsset(AssetForUpdateDto assetDto,
            ApiVersion version)
        {
            if (assetDto == null)
            {
                return BadRequest();
            }

            var assetToReturn = await InsertAssetAsync(assetDto);

            return CreatedAtRoute("GetAssetById",
                new
                {
                    assetId = assetToReturn.Id,
                    version = $"{version}"
                },
                assetToReturn);
        }


        [HttpPut("{assetId}", Name = "UpdateAsset")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateAsset(Guid assetId, AssetForUpdateDto assetDto,
            ApiVersion version)
        {
            if (assetId == Guid.Empty || assetDto == null)
            {
                return BadRequest();
            }

            var assetFromRepo = await _eventHostRepository.GetAssetByIdAsync(assetId);

            if (assetFromRepo == null)
            {
                var assetToReturn = await InsertAssetAsync(assetDto);

                return CreatedAtRoute("GetAssetById",
                    new
                    {
                        assetId = assetToReturn.Id,
                        version = $"{version}"
                    },
                    assetToReturn);
            }

            _mapper.Map(assetDto, assetFromRepo);
            //await _eventHostRepository.UpdateAssetAsync(assetFromRepo);
            await _eventHostRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{assetId}", Name = "DeleteAsset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteAsset(Guid assetId,
            ApiVersion version)
        {
            //todo: verify the cascade effect

            if (assetId == Guid.Empty)
            {
                return BadRequest();
            }

            var assetFromRepo = await _eventHostRepository.GetAssetByIdAsync(assetId);

            if (assetFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteAsset(assetFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************

        [HttpHead(Name = "GetAssetMetadata")]
        public ActionResult GetAssetMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetAssetOptions")]
        public ActionResult GetAssetOptions(ApiVersion version)
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,DELETE,OPTIONS");
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

        private async Task<AssetDetailDto> InsertAssetAsync(AssetForUpdateDto assetDto)
        {
            var asset = _mapper.Map<Asset>(assetDto);
            await _eventHostRepository.AddAssetAsync(asset);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<AssetDetailDto>(asset);
        }

        #endregion

    }
}