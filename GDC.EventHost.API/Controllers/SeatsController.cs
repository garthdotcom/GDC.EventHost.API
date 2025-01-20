using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Seat;
using GHC.EventHost.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GDC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/seats")]
    [ApiController]
    public class SeatsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public SeatsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetSeats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeatDetailDto>>> GetSeats(ApiVersion version)
        {
            var seatFromRepo = await _eventHostRepository.GetSeatsAsync();

            return Ok(_mapper.Map<IEnumerable<SeatDetailDto>>(seatFromRepo));
        }


        [HttpGet("{seatId}", Name = "GetSeatById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SeatDisplayDto>> GetSeatById(Guid seatId,
            ApiVersion version)
        {
            if (seatId == Guid.Empty)
            {
                return BadRequest();
            }

            var seatFromRepo = await _eventHostRepository.GetSeatByIdAsync(seatId);

            if (seatFromRepo == null)
            {
                return NotFound();
            }

            var seatDisplayDto = _mapper.Map<SeatDisplayDto>(seatFromRepo);

            // need the seatingPlan for this seat in order to retrieve the venue and the positions for the display dto

            var seatPositionsFromRepo = _eventHostRepository.GetSeatPositionsForSeat(seatFromRepo.ParentId);

            if (seatPositionsFromRepo.Any())
            {
                var seatingPlanId = seatPositionsFromRepo.First().SeatingPlanId;

                var seatingPlanFromRepo = await _eventHostRepository.GetSeatingPlanByIdAsync(seatingPlanId);

                if (seatingPlanFromRepo != null)
                {
                    var seatList = new SeatList(_eventHostRepository, seatingPlanFromRepo.Id).SeatDetails;

                    seatDisplayDto = seatList
                        .FirstOrDefault(s => s.Id == seatId);

                    seatDisplayDto.SeatingPlanId = seatingPlanId;
                    seatDisplayDto.VenueId = seatingPlanFromRepo.VenueId;
                    seatDisplayDto.VenueName = seatingPlanFromRepo.Venue.Name;
                }
            }

            return Ok(seatDisplayDto);
        }


        [HttpGet("list", Name = "GetSeatsForList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetSeatsForList(ApiVersion version)
        {
            var eventTypeFromRepo = await _eventHostRepository.GetSeatsAsync();

            return Ok(_mapper.Map<IEnumerable<SeatDto>>(eventTypeFromRepo));
        }


        [HttpPost(Name = "CreateSeat")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SeatDetailDto>> CreateSeat(SeatForUpdateDto seatDto,
            ApiVersion version)
        {
            if (seatDto == null)
            {
                return BadRequest();
            }

            var seatToReturn = await InsertSeatAsync(seatDto);

            return CreatedAtRoute("GetSeatById",
                new
                {
                    seatId = seatToReturn.Id,
                    version = $"{version}"
                },
                seatToReturn);
        }


        [HttpPut("{seatId}", Name = "UpdateSeat")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateSeat(Guid seatId, SeatForUpdateDto seatDto,
            ApiVersion version)
        {
            if (seatId == Guid.Empty || seatDto == null)
            {
                return BadRequest();
            }

            var seatFromRepo = await _eventHostRepository.GetSeatByIdAsync(seatId);

            if (seatFromRepo == null)
            {
                var seatToReturn = await InsertSeatAsync(seatDto);

                return CreatedAtRoute("GetSeatById",
                    new
                    {
                        seatId = seatToReturn.Id,
                        version = $"{version}"
                    },
                    seatToReturn);
            }

            _mapper.Map(seatDto, seatFromRepo);
            //await _eventHostRepository.UpdateSeatAsync(seatFromRepo);
            await _eventHostRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{seatId}", Name = "PartiallyUpdateSeat")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateSeat(Guid seatId,
            JsonPatchDocument<SeatForUpdateDto> patchDocument,
            ApiVersion version)
        {
            if (seatId == Guid.Empty)
            {
                return BadRequest();
            }

            var seatExists = await _eventHostRepository.SeatExistsAsync(seatId);

            if (!seatExists)
            {
                var seatDto = new SeatForUpdateDto();

                patchDocument.ApplyTo(seatDto, ModelState);

                if (!TryValidateModel(seatDto))
                {
                    return ValidationProblem(ModelState);
                }

                var seatToReturn = await InsertSeatAsync(seatDto);

                return CreatedAtRoute("GetSeatById",
                    new
                    {
                        seatId = seatToReturn.Id,
                        version = $"{version}"
                    },
                    seatToReturn);
            }

            var seatFromRepo = await _eventHostRepository.GetSeatByIdAsync(seatId);

            var seatToPatch = _mapper.Map<SeatForUpdateDto>(seatFromRepo);

            patchDocument.ApplyTo(seatToPatch, ModelState);

            if (!TryValidateModel(seatToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(seatToPatch, seatFromRepo);
            //await _eventHostRepository.UpdateSeatAsync(seatFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{seatId}", Name = "DeleteSeat")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteSeat(Guid seatId,
            ApiVersion version)
        {
            if (seatId == Guid.Empty)
            {
                return BadRequest();
            }

            var seatFromRepo = await _eventHostRepository.GetSeatByIdAsync(seatId);

            if (seatFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteSeat(seatFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }

        //*************************************************************************************

        [HttpHead(Name = "GetSeatMetadata")]
        public ActionResult GetSeatMetadata(ApiVersion version)
        {
            return Ok();
        }

        [HttpOptions(Name = "GetSeatOptions")]
        public ActionResult GetSeatOptions(ApiVersion version)
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

        private async Task<SeatDetailDto> InsertSeatAsync(SeatForUpdateDto seatDto)
        {
            var seat = _mapper.Map<Seat>(seatDto);
            await _eventHostRepository.AddSeatAsync(seat);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<SeatDetailDto>(seat);
        }

        #endregion
    }
}