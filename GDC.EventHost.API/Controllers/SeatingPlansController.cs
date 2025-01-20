using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Seat;
using GDC.EventHost.Shared.SeatingPlan;
using GHC.EventHost.API.Services;
using Microsoft.AspNetCore.Mvc;
using static GDC.EventHost.Shared.Enums;

namespace Garth.EventHost.Api.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/seatingPlans")]
    [ApiController]
    public class SeatingPlansController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public SeatingPlansController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetSeatingPlan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeatingPlanDetailDto>>> GetSeatingPlans(
            [FromQuery] SeatingPlanResourceParameters resourceParms,
            ApiVersion version)
        {
            var seatingPlanFromRepo = await _eventHostRepository.GetSeatingPlansAsync(resourceParms);

            return Ok(_mapper.Map<IEnumerable<SeatingPlanDetailDto>>(seatingPlanFromRepo)); // returns seat tree
        }


        [HttpGet("{seatingPlanId}", Name = "GetSeatingPlanById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SeatingPlanDetailDto>> GetSeatingPlanById(Guid seatingPlanId,
            ApiVersion version)
        {
            if (seatingPlanId == Guid.Empty)
            {
                return BadRequest();
            }

            var seatingPlanFromRepo = await _eventHostRepository.GetSeatingPlanByIdAsync(seatingPlanId);

            if (seatingPlanFromRepo == null)
            {
                return NotFound();
            }

            var seatList = new SeatList(_eventHostRepository, seatingPlanId).SeatDetails;

            var seatingPlanDetail = new SeatingPlanDetailDto()
            {
                Id = seatingPlanFromRepo.Id,
                Name = seatingPlanFromRepo.Name,
                StatusId = seatingPlanFromRepo.StatusId,
                StatusValue = seatingPlanFromRepo.StatusId.ToString(),
                VenueId = seatingPlanFromRepo.VenueId,
                VenueName = seatingPlanFromRepo.Venue.Name,
                SeatsForDisplay = seatList.ToList()
            };

            return Ok(seatingPlanDetail);
        }


        [HttpGet("{seatingPlanId}/seats/{seatId}", Name = "GetSeatingPlanSeatById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<SeatDisplayDto> GetSeatingPlanSeatById(Guid seatingPlanId, Guid seatId,
            ApiVersion version)
        {
            // we need the seatingPlan to build the seat list. from the seat list, we can get the
            // details for the seat (i.e. the SeatPosition tree). if we are not interested in
            // obtaining the tree, the seat can be retrieved directly (like for deletion).

            if (seatingPlanId == Guid.Empty || seatId == Guid.Empty)
            {
                return BadRequest();
            }

            var seatList = new SeatList(_eventHostRepository, seatingPlanId).SeatDetails;

            var seatDisplayDto = seatList
                .FirstOrDefault(s => s.Id == seatId);

            return Ok(seatDisplayDto);
        }


        [HttpGet("list", Name = "GetSeatingPlansForList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeatingPlanDto>>> GetSeatingPlansForList(ApiVersion version)
        {
            var eventTypeFromRepo = await _eventHostRepository.GetSeatPositionsAsync();

            return Ok(_mapper.Map<IEnumerable<SeatingPlanDto>>(eventTypeFromRepo));  // returns seatingPlan only
        }


        [HttpPost(Name = "CreateSeatingPlan")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SeatingPlanDetailDto>> CreateSeatingPlan(SeatingPlanForCreateDto seatingPlanDto,
            ApiVersion version)
        {
            if (seatingPlanDto == null)
            {
                return BadRequest();
            }

            var seatingPlanToReturn = await InsertSeatingPlanAsync(seatingPlanDto);

            return CreatedAtRoute("GetSeatingPlanById",
                new
                {
                    seatingPlanId = seatingPlanToReturn.Id,
                    version = $"{version}"
                },
                seatingPlanToReturn);
        }


        private async Task<SeatingPlanDetailDto> InsertSeatingPlanAsync(SeatingPlanForCreateDto seatingPlanCreateDto)
        {
            var newSeatingPlan = new SeatingPlan()
            {
                Name = seatingPlanCreateDto.Name,
                StatusId = seatingPlanCreateDto.StatusId,
                VenueId = seatingPlanCreateDto.VenueId
            };

            await _eventHostRepository.AddSeatingPlanAsync(newSeatingPlan);

            var previousLevelItems = new List<SeatPosition>();
            
            int currentLevel = 0;

            foreach (var levelSpec in seatingPlanCreateDto.SeatPositions)
            {
                var currentLevelItems = new List<SeatPosition>();

                for (int levelItemCount = 1; levelItemCount <= levelSpec.Number; levelItemCount++)
                {
                    if (previousLevelItems.Count() == 0)
                    {
                        var newCurrentItem = new SeatPosition()
                        {
                            Level = currentLevel,
                            SeatingPlanId = newSeatingPlan.Id,
                            OrdinalValue = levelItemCount,
                            DisplayValue = levelItemCount.ToString(),
                            SeatPositionTypeId = levelSpec.SeatPositionType
                        };

                        currentLevelItems.Add(newCurrentItem);
                    }
                    else
                    {
                        foreach (var previousLevelItem in previousLevelItems)
                        {
                            var newCurrentItem = new SeatPosition()
                            {
                                Level = currentLevel,
                                SeatingPlanId = newSeatingPlan.Id,
                                OrdinalValue = levelItemCount,
                                DisplayValue = levelItemCount.ToString(),
                                SeatPositionTypeId = levelSpec.SeatPositionType
                            };

                            previousLevelItem.Children.Add(newCurrentItem);

                            currentLevelItems.Add(newCurrentItem);
                        }
                    }
                }

                if (currentLevel == 0)
                {
                    newSeatingPlan.SeatPositions = currentLevelItems;
                }

                previousLevelItems = currentLevelItems;
                currentLevel++;
            }

            foreach (var bottomLevelItem in previousLevelItems)
            {
                for (int seatCount = 1; seatCount <= seatingPlanCreateDto.NumberOfSeatsPerRow; seatCount++)
                {
                    var newSeat = new Seat()
                    {
                        OrdinalValue = seatCount,
                        DisplayValue = seatCount.ToString(),
                        SeatTypeId = SeatTypeEnum.Standard
                    };

                    bottomLevelItem.Seats.Add(newSeat);
                } 
            }

            await _eventHostRepository.SaveChangesAsync();

            var seatList = new SeatList(_eventHostRepository, newSeatingPlan.Id).SeatDetails;

            return new SeatingPlanDetailDto()
            {
                Id = newSeatingPlan.Id,
                Name = newSeatingPlan.Name,
                StatusId = newSeatingPlan.StatusId,
                VenueId = newSeatingPlan.VenueId,
                SeatsForDisplay = seatList.ToList()
            };
        }

    }
}