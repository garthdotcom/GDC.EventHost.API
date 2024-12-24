using AutoMapper;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.DTO.Venue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GDC.EventHost.API.Controllers
{
    [Authorize]
    [Route("api/venues")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly ILogger<VenuesController> _logger;
        private readonly IMailService _mailService;
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public VenuesController(ILogger<VenuesController> logger,
            IMailService mailService,
            IEventHostRepository eventHostRepository,
            IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ??
                throw new ArgumentNullException(nameof(mailService));
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueDto>>> GetVenues(
            [FromQuery] VenueResourceParameters venueResourceParameters)
        {
            var pageSize = venueResourceParameters.PageSize > maxPageSize
                ? maxPageSize
                : venueResourceParameters.PageSize;

            var (venueEntities, paginationMetadata) = await _eventHostRepository
                .GetVenuesAsync(venueResourceParameters);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            if (venueResourceParameters.IncludeDetail)
            {
                return Ok(_mapper.Map<IEnumerable<VenueDetailDto>>(venueEntities));
            }

            return Ok(_mapper.Map<IEnumerable<VenueDto>>(venueEntities));
        }


        [HttpGet("{id}", Name = "GetVenue")]
        public async Task<ActionResult> GetVenue(Guid id, bool includeDetail = false)
        {
            var entity = await _eventHostRepository.GetVenueAsync(id, includeDetail);

            if (entity == null)
            {
                return NotFound();
            }

            if (includeDetail)
            {
                return Ok(_mapper.Map<VenueDetailDto>(entity));
            }

            return Ok(_mapper.Map<VenueDto>(entity));
        }


        [HttpPost]
        public async Task<ActionResult<VenueDto>> CreateVenue(
            [FromBody] VenueForUpdateDto venueForUpdateDto)
        {
            var newVenueEntity = _mapper.Map<Entities.Venue>(venueForUpdateDto);

            _eventHostRepository.AddVenue(newVenueEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                var venueToReturnDto = _mapper.Map<VenueDto>(newVenueEntity);

                return CreatedAtRoute("GetVenue",
                    new
                    {
                        id = venueToReturnDto.Id,
                        includeEvents = false
                    },
                    venueToReturnDto);
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to create a new venue.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVenue(
            Guid id,
            VenueForUpdateDto venueForUpdateDto)
        {
            var venueEntity = await _eventHostRepository
                .GetVenueAsync(id, false);

            if (venueEntity == null)
            {
                _logger.LogInformation("Venue with id {VenueId} was not found when trying to update it.",
                    id);
                return NotFound();
            }

            // overwrite the entity with the corresponding values in the dto
            _mapper.Map(venueForUpdateDto, venueEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to update the venue with id {id}.");
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateVenue(
            Guid id,
            JsonPatchDocument<VenueForUpdateDto> patchDocument)
        {
            var venueEntity = await _eventHostRepository
                .GetVenueAsync(id, false);

            if (venueEntity == null)
            {
                _logger.LogInformation("Venue with id {VenueId} was not found when trying to partially update it.", id);
                return NotFound();
            }

            var venueToPatchDto = _mapper.Map<VenueForUpdateDto>(venueEntity);

            patchDocument.ApplyTo(venueToPatchDto, ModelState);

            // check for any errors in the patch document
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("An issue was found in the patch document when trying to patch id {VenueId}.", id);
                return BadRequest(ModelState);
            }

            // check for broken validation rules on the model
            if (!TryValidateModel(venueToPatchDto))
            {
                _logger.LogInformation("Validation issue(s) was/were found when trying to patch id {VenueId}.", id);
                return BadRequest(ModelState);
            }

            // overwrite the entity with the corresponding dto values
            _mapper.Map(venueToPatchDto, venueEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to partially update the venue with id {id}.");

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVenue(Guid id)
        {
            var venueEntity = await _eventHostRepository
                .GetVenueAsync(id, false);

            if (venueEntity == null)
            {
                _logger.LogInformation("Venue with id {VenueId} was not found when trying to partially update it.", id);
                return NotFound();
            }

            _eventHostRepository.DeleteVenue(venueEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                _mailService.Send($"Venue Deleted: {venueEntity.Name}",
                $"A venue called '{venueEntity.Name}' with id '{id}' was deleted.");
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to delete the venue '{venueEntity.Name}' with id {id}.");
        }

    }
}
