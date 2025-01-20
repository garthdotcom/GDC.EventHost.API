using AutoMapper;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.SeatPosition;
using Microsoft.AspNetCore.Mvc;

namespace GHC.EventHost.API.Controllers
{
    // helps maintain the position list when in the process of creating a layout

    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/seatpositions")]
    [ApiController]
    public class SeatPositionsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public SeatPositionsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetSeatPositions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeatPositionDto>>> GetSeatPositions(
            ApiVersion version)
        {
            var seatPositionDtos = await _eventHostRepository
                .GetSeatPositionsAsync();

            return Ok(_mapper.Map<IEnumerable<SeatPositionDto>>(seatPositionDtos));
        }

        [HttpGet("{seatPositionId}", Name = "GetSeatPositionById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SeatPositionDetailDto>> GetSeatPositionById(Guid seatPositionId,
            ApiVersion version)
        {
            if (seatPositionId == Guid.Empty)
            {
                return BadRequest();
            }

            var seatPositionFromRepo = await _eventHostRepository
                .GetSeatPositionByIdAsync(seatPositionId);

            if (seatPositionFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SeatPositionDetailDto>(seatPositionFromRepo));

        }

    }
}