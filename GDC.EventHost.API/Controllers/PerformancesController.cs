using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Performance;
using GDC.EventHost.Shared.PerformanceAsset;
using GDC.EventHost.Shared.Ticket;
using GHC.EventHost.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using static GDC.EventHost.Shared.Enums;
using GDC.EventHost.Shared.SeatingPlan;
using GDC.EventHost.Shared.Venue;
using GDC.EventHost.Shared.VenueAsset;

namespace GDC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/performances")]
    [ApiController]
    public class PerformancesController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public PerformancesController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetPerformance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PerformanceDetailDto>>> GetPerformances(
            [FromQuery] PerformanceResourceParameters resourceParms,
            ApiVersion version)
        {
            var performanceFromRepo = await _eventHostRepository.GetPerformancesAsync(resourceParms);

            var performanceDetailDtos = _mapper.Map<IEnumerable<PerformanceDetailDto>>(performanceFromRepo);

            foreach (var performanceDetailDto in performanceDetailDtos)
            {
                performanceDetailDto.TicketCount = _eventHostRepository.
                    GetPerformanceTicketCount(performanceDetailDto.Id);
            }

            return Ok(performanceDetailDtos);
        }


        [HttpGet("{performanceId}", Name = "GetPerformanceById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PerformanceDetailDto>> GetPerformanceById(Guid performanceId,
            ApiVersion version)
        {
            if (performanceId == Guid.Empty)
            {
                return BadRequest();
            }

            var performanceFromRepo = await _eventHostRepository.GetPerformanceByIdAsync(performanceId);

            if (performanceFromRepo == null)
            {
                return NotFound();
            }

            var performanceDetailDto = _mapper.Map<PerformanceDetailDto>(performanceFromRepo);

            foreach (var asset in performanceFromRepo.PerformanceAssets)
            {
                performanceDetailDto.PerformanceAssets
                    .Add(_mapper.Map<PerformanceAssetDto>(asset));
            }

            performanceDetailDto.TicketCount = _eventHostRepository.
                GetPerformanceTicketCount(performanceId);

            return Ok(performanceDetailDto);
        }


        [HttpPost(Name = "CreatePerformance")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PerformanceDto>> CreatePerformance(PerformanceForUpdateDto performanceDto,
            ApiVersion version)
        {
            if (performanceDto == null)
            {
                return BadRequest();
            }

            var performanceToReturn = await InsertPerformanceAsync(performanceDto);

            return CreatedAtRoute("GetPerformanceById",
                new
                {
                    performanceId = performanceToReturn.Id,
                    version = $"{version}"
                },
                performanceToReturn);
        }


        [HttpPut("{performanceId}", Name = "UpdatePerformance")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdatePerformance(Guid performanceId, PerformanceForUpdateDto performanceDto,
            ApiVersion version)
        {
            if (performanceId == Guid.Empty || performanceDto == null)
            {
                return BadRequest();
            }

            var performanceFromRepo = await _eventHostRepository
                .GetPerformanceByIdAsync(performanceId);

            if (performanceFromRepo == null)
            {
                var performanceToReturn = await InsertPerformanceAsync(performanceDto);

                return CreatedAtRoute("GetPerformanceById",
                    new
                    {
                        performanceId = performanceToReturn.Id,
                        version = $"{version}"
                    },
                    performanceToReturn);
            }

            _mapper.Map(performanceDto, performanceFromRepo);
            //await _eventHostRepository.UpdatePerformanceAsync(performanceFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpPatch("{performanceId}", Name = "PartiallyUpdatePerformance")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdatePerformance(Guid performanceId,
            JsonPatchDocument<PerformanceForUpdateDto> patchDocument,
            ApiVersion version)
        {
            if (performanceId == Guid.Empty)
            {
                return BadRequest();
            }

            var performanceExists = await _eventHostRepository.PerformanceExistsAsync(performanceId);

            if (!performanceExists)
            {
                var performanceDto = new PerformanceForUpdateDto();

                patchDocument.ApplyTo(performanceDto, ModelState);

                if (!TryValidateModel(performanceDto))
                {
                    return ValidationProblem(ModelState);
                }

                var performanceToReturn = await InsertPerformanceAsync(performanceDto);

                return CreatedAtRoute("GetPerformanceById",
                    new
                    {
                        performanceId = performanceToReturn.Id,
                        version = $"{version}"
                    },
                    performanceToReturn);
            }

            var performanceFromRepo = await _eventHostRepository
                .GetPerformanceByIdAsync(performanceId);

            var performanceToPatch = _mapper.Map<PerformanceForUpdateDto>(performanceFromRepo);

            patchDocument.ApplyTo(performanceToPatch, ModelState);

            if (!TryValidateModel(performanceToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(performanceToPatch, performanceFromRepo);
            //await _eventHostRepository.UpdatePerformanceAsync(performanceFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{performanceId}", Name = "DeletePerformance")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeletePerformance(Guid performanceId,
            ApiVersion version)
        {
            if (performanceId == Guid.Empty)
            {
                return BadRequest();
            }

            var performanceFromRepo = await _eventHostRepository
                .GetPerformanceByIdAsync(performanceId);

            if (performanceFromRepo == null)
            {
                return NotFound();
            }

            // recalculate the Event's start and end date range because a performance
            // has been removed

            var existingEvent = await _eventHostRepository.
                GetEventByIdAsync(performanceFromRepo.EventId);

            existingEvent.StartDate = DateTime.MaxValue;
            existingEvent.EndDate = DateTime.MinValue;

            foreach (var existingPerformance in existingEvent.Performances)
            {
                if (existingPerformance.Id == performanceFromRepo.Id)
                {
                    continue;   // we're deleting this one so ignore it
                }
                if (existingPerformance.Date < existingEvent.StartDate)
                {
                    existingEvent.StartDate = existingPerformance.Date;
                }
                if (existingPerformance.Date > existingEvent.EndDate)
                {
                    existingEvent.EndDate = existingPerformance.Date;
                }
            }

            //await _eventHostRepository.UpdateEventAsync(existingEvent);

            // soft delete the performance

            _eventHostRepository.SoftDeletePerformance(performanceFromRepo);

            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************
        // Performance Assets

        // the resource parms allow us to filter by asset type

        [HttpGet("{performanceId}/assets", Name = "GetAssetsForPerformance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PerformanceAssetDto>>> GetAssetsForPerformance(Guid performanceId,
            [FromQuery] AssetResourceParameters resourceParms,
            ApiVersion version)
        {
            if (performanceId == Guid.Empty)
            {
                return BadRequest();
            }

            var performanceAssetsFromRepo = await _eventHostRepository
                .GetPerformanceAssetsAsync(performanceId, resourceParms);

            return Ok(_mapper.Map<IEnumerable<PerformanceAssetDto>>(performanceAssetsFromRepo));
        }


        [HttpPost("{performanceId}/assets", Name = "AddAssetToPerformance")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PerformanceAssetDto>> AddAssetToPerformance(Guid performanceId,
            PerformanceAssetForUpdateDto assetForUpdateDto,
            ApiVersion version)
        {
            if (performanceId == Guid.Empty || assetForUpdateDto == null)
            {
                return BadRequest();
            }

            var performanceAssetExists = await _eventHostRepository
                .PerformanceAssetExistsAsync(performanceId, assetForUpdateDto.AssetTypeId);

            if (performanceAssetExists)
            {
                ModelState.AddModelError(nameof(PerformanceAssetForUpdateDto),
                    $"An asset of type '{assetForUpdateDto.AssetTypeId}' currently exists for this performance.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var performanceAsset = _mapper.Map<PerformanceAsset>(assetForUpdateDto);

            performanceAsset.PerformanceId = performanceId;

            await _eventHostRepository.AddPerformanceAssetAsync(performanceAsset);

            await _eventHostRepository.SaveChangesAsync();

            var newPerformanceAssetDto = _mapper.Map<PerformanceAssetDto>(performanceAsset);

            return CreatedAtRoute("GetPerformanceAssetById",
                new
                {
                    performanceAssetId = newPerformanceAssetDto.Id,
                    version = $"{version}"
                },
                newPerformanceAssetDto);
        }


        //*************************************************************************************
        // Tickets

        [HttpGet("{performanceId}/tickets", Name = "GetPerformanceTickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetPerformanceTickets(Guid performanceId)
        {
            if (performanceId == Guid.Empty)
            {
                return BadRequest();
            }

            var ticketsFromRepo = await _eventHostRepository.GetPerformanceTicketsAsync(performanceId);

            return Ok(_mapper.Map<IEnumerable<TicketDto>>(ticketsFromRepo));
        }


        [HttpGet("{performanceId}/tickets/detail", Name = "GetDetailedPerformanceTickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TicketDetailDto>>> GetDetailedPerformanceTickets(Guid performanceId)
        {
            if (performanceId == Guid.Empty)
            {
                return BadRequest();
            }

            var performanceFromRepo = await _eventHostRepository.GetPerformanceByIdAsync(performanceId);

            if (performanceFromRepo == null)
            {
                return NotFound();
            }

            if (performanceFromRepo.SeatingPlan == null)
            {
                ModelState.AddModelError(nameof(Performance),
                    "A seating plan must be added to the Performance before tickets can be created.");
                return new UnprocessableEntityObjectResult(ModelState);
            }
            var seatList = new SeatList(_eventHostRepository, performanceFromRepo.SeatingPlanId.Value);

            var detailedTicketList = seatList.GetPerformanceTicketsAsync(performanceId);

            return Ok(detailedTicketList);
        }


        [HttpPost("{performanceId}/tickets", Name = "CreatePerformanceTickets")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<TicketDetailDto>>> CreatePerformanceTickets(Guid performanceId,
            PerformanceTicketForCreateDto ticketForCreateDto,
            ApiVersion version)
        {
            if (performanceId == Guid.Empty || ticketForCreateDto is null)
            {
                return BadRequest();
            }

            var performanceFromRepo = await _eventHostRepository.GetPerformanceByIdAsync(performanceId);

            if (performanceFromRepo is null)
            {
                return NotFound();
            }

            if (performanceFromRepo.SeatingPlanId is null)
            {
                ModelState.AddModelError(nameof(Performance),
                    "A seating plan must be added to the performance before tickets can be created.");
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var detailedSeatList = 
                new SeatList(_eventHostRepository, ticketForCreateDto.SeatingPlanId).SeatDetails;

            if (!detailedSeatList.Any())
            {
                return NotFound();
            }

            var ticketDtoList = new List<TicketDto>();

            foreach (var detailedSeat in detailedSeatList)
            {
                var ticketDto = new TicketDto()
                {
                    Number = GenerateTicketNumber(),
                    Price = ticketForCreateDto.BasicTicketPrice,
                    PerformanceId = performanceId,
                    TicketStatusId = TicketStatusEnum.UnSold,
                    SeatId = detailedSeat.Id
                };

                var ticket = _mapper.Map<Ticket>(ticketDto);
                ticket.PerformanceId = performanceId;
                await _eventHostRepository.AddTicketAsync(ticket);
                ticketDtoList.Add(_mapper.Map<TicketDto>(ticket));
            }

            await _eventHostRepository.SaveChangesAsync();

            return CreatedAtRoute("GetDetailedPerformanceTickets",
                new
                {
                    performanceId,
                    version = $"{version}"
                },
                ticketDtoList);
        }

        //*************************************************************************************

        [HttpHead(Name = "GetPerformanceMetadata")]
        public ActionResult GetPerformanceMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetPerformanceOptions")]
        public ActionResult GetPerformanceOptions(ApiVersion version)
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

        private async Task<PerformanceDto> InsertPerformanceAsync(PerformanceForUpdateDto performanceDto)
        {
            // update event with new start and end date if needed
            await HandleEventDatesAsync(performanceDto.EventId, performanceDto.Date);

            var performanceItem = _mapper.Map<Performance>(performanceDto);
            await _eventHostRepository.AddPerformanceAsync(performanceItem);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<PerformanceDto>(performanceItem);
        }


        private async Task HandleEventDatesAsync(Guid existingEventId, DateTime performanceDate)
        {
            var existingEvent = await _eventHostRepository.GetEventByIdAsync(existingEventId, false);

            if (!existingEvent.StartDate.HasValue || performanceDate < existingEvent.StartDate)
            {
                existingEvent.StartDate = performanceDate;
            }

            if (!existingEvent.EndDate.HasValue || performanceDate > existingEvent.EndDate)
            {
                existingEvent.EndDate = performanceDate;
            }

            //await _eventHostRepository.UpdateEventAsync(existingEvent);
        }

        private string GenerateTicketNumber()
        {
            return Guid.NewGuid().ToString().ToUpper().Replace("-", null);
        }

        #endregion
    }
}