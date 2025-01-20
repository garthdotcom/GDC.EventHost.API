using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.PerformanceType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Garth.EventHost.Api.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/performancetypes")]
    [ApiController]
    public class PerformanceTypesController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public PerformanceTypesController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetPerformanceTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PerformanceTypeDetailDto>>> GetPerformanceTypes(ApiVersion version)
        {
            var performanceTypeFromRepo = await _eventHostRepository.GetPerformanceTypesAsync();

            return Ok(_mapper.Map<IEnumerable<PerformanceTypeDetailDto>>(performanceTypeFromRepo));
        }


        [HttpGet("{performanceTypeId}", Name = "GetPerformanceTypeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PerformanceTypeDetailDto>> GetPerformanceTypeById(Guid performanceTypeId,
            ApiVersion version)
        {
            if (performanceTypeId == Guid.Empty)
            {
                return BadRequest();
            }

            var performanceTypeFromRepo = await _eventHostRepository.
                GetPerformanceTypeByIdAsync(performanceTypeId);

            if (performanceTypeFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PerformanceTypeDetailDto>(performanceTypeFromRepo));
        }


        [HttpGet("list", Name = "GetPerformanceTypesForList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PerformanceTypeDto>>> GetPerformanceTypesForList(ApiVersion version)
        {
            var performanceTypeFromRepo = await _eventHostRepository
                .GetPerformanceTypesAsync();

            return Ok(_mapper.Map<IEnumerable<PerformanceTypeDto>>(performanceTypeFromRepo));
        }
         

        [HttpPost(Name = "CreatePerformanceType")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PerformanceTypeDetailDto>> CreatePerformanceType(PerformanceTypeForUpdateDto performanceTypeDto,
            ApiVersion version)
        {
            if (performanceTypeDto == null)
            {
                return BadRequest();
            }

            var performanceTypeToReturn = await InsertPerformanceTypeAsync(performanceTypeDto);

            return CreatedAtRoute("GetPerformanceTypeById",
                new
                {
                    performanceTypeId = performanceTypeToReturn.Id,
                    version = $"{version}"
                },
                performanceTypeToReturn);
        }


        [HttpPut("{performanceTypeId}", Name = "UpdatePerformanceType")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdatePerformanceType(Guid performanceTypeId, PerformanceTypeForUpdateDto performanceTypeDto,
            ApiVersion version)
        {
            if (performanceTypeId == Guid.Empty || performanceTypeDto == null)
            {
                return BadRequest();
            }

            var performanceTypeFromRepo = await _eventHostRepository
                .GetPerformanceTypeByIdAsync(performanceTypeId);

            if (performanceTypeFromRepo == null)
            {
                var performanceTypeToReturn = await InsertPerformanceTypeAsync(performanceTypeDto);

                return CreatedAtRoute("GetPerformanceTypeById",
                    new
                    {
                        performanceTypeId = performanceTypeToReturn.Id,
                        version = $"{version}"
                    },
                    performanceTypeToReturn);
            }

            _mapper.Map(performanceTypeDto, performanceTypeFromRepo);
            //await _eventHostRepository.UpdatePerformanceTypeAsync(performanceTypeFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************


        [HttpHead(Name = "GetPerformanceTypeMetadata")]
        public ActionResult GetPerformanceTypeMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetPerformanceTypeOptions")]
        public ActionResult GetPerformanceTypeOptions(ApiVersion version)
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

        private async Task<PerformanceTypeDetailDto> InsertPerformanceTypeAsync(PerformanceTypeForUpdateDto performanceTypeDto)
        {
            var performanceType = _mapper.Map<PerformanceType>(performanceTypeDto);
            await _eventHostRepository.AddPerformanceTypeAsync(performanceType);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<PerformanceTypeDetailDto>(performanceType);
        }

        #endregion
    }
}
