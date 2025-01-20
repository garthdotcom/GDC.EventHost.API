using AutoMapper;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.SeatPosition;
using GDC.EventHost.Shared.Ticket;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GDC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/tickets")]
    [ApiController]
    public class TicketsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public TicketsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }

        // notes: 
        // retrieval of all tickets in the system is not supported for performance reasons
        // instead, tickets can be retrieved for individual events
        // tickets are created in the EventsController/CreateEventTickets

        [HttpGet("{ticketId}", Name = "GetTicketById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TicketDetailDto>> GetTicketById(Guid ticketId,
            ApiVersion version)
        {
            if (ticketId == Guid.Empty)
            {
                return BadRequest();
            }

            var ticketFromRepo = await _eventHostRepository.GetTicketByIdAsync(ticketId);

            if (ticketFromRepo == null)
            {
                return NotFound();
            }

            var ticketDetail = _mapper.Map<TicketDetailDto>(ticketFromRepo);

            // get the seat positions for the seat by walking up the ParentId chain

            var seatPositionsFromRepo = _eventHostRepository
                .GetSeatPositionsForSeat(ticketDetail.Seat.ParentId);

            if (seatPositionsFromRepo.Count() > 0)
            {
                ticketDetail.Seat.SeatPositions = 
                    _mapper.Map<IEnumerable<SeatPositionDisplayDto>>(seatPositionsFromRepo).ToList();
            }

            return Ok(ticketDetail);
        }


        [HttpPut("{ticketId}", Name = "UpdateTicket")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateTicket(Guid ticketId, TicketForUpdateDto ticketForUpdateDto,
            ApiVersion version)
        {
            if (ticketId == Guid.Empty || ticketForUpdateDto == null)
            {
                return BadRequest();
            }

            var ticketFromRepo = await _eventHostRepository.GetTicketByIdAsync(ticketId);

            if (ticketFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(ticketForUpdateDto, ticketFromRepo);
            //_eventHostRepository.UpdateTicket(ticketFromRepo);
            await _eventHostRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{ticketId}", Name = "PartiallyUpdateTicket")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateTicket(Guid ticketId,
            JsonPatchDocument<TicketForUpdateDto> patchDocument,
            ApiVersion version)
        {
            if (ticketId == Guid.Empty || patchDocument == null)
            {
                return BadRequest();
            }

            var ticketFromRepo = await _eventHostRepository.GetTicketByIdAsync(ticketId);

            if (ticketFromRepo == null)
            {
                return NotFound();
            }

            var ticketToPatch = _mapper.Map<TicketForUpdateDto>(ticketFromRepo);

            patchDocument.ApplyTo(ticketToPatch, ModelState);

            if (!TryValidateModel(ticketToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(ticketToPatch, ticketFromRepo);

            //_eventHostRepository.UpdateTicket(ticketFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpHead(Name = "GetTicketMetadata")]
        public ActionResult GetTicketMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetTicketOptions")]
        public ActionResult GetTicketOptions(ApiVersion version)
        {
            Response.Headers.Add("Allow", "GET,PUT,PATCH,OPTIONS");
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
    }
}