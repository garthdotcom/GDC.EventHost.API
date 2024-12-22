using AutoMapper;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.DTO.Series;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GDC.EventHost.API.Controllers
{
    [Authorize]
    [Route("api/series")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ILogger<SeriesController> _logger;
        private readonly IMailService _mailService;
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public SeriesController(ILogger<SeriesController> logger,
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
        public async Task<ActionResult<IEnumerable<SeriesWithoutEventsDto>>> GetSeries(
            [FromQuery] SeriesResourceParameters seriesResourceParameters)
        {
            var pageSize = seriesResourceParameters.PageSize > maxPageSize
                ? maxPageSize 
                : seriesResourceParameters.PageSize;

            var (seriesEntities, paginationMetadata) = await _eventHostRepository
                .GetSeriesAsync(seriesResourceParameters);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            if (seriesResourceParameters.IncludeDetail)
            {
                return Ok(_mapper.Map<IEnumerable<SeriesDetailDto>>(seriesEntities));
            }

            return Ok(_mapper.Map<IEnumerable<SeriesDto>>(seriesEntities));
        }


        [HttpGet("{id}", Name = "GetSeries")]
        public async Task<ActionResult> GetSeries(Guid id, bool includeDetail = false)
        {
            var entity = await _eventHostRepository.GetSeriesAsync(id, includeDetail);

            if (entity == null)
            {
                return NotFound();
            }

            if (includeDetail)
            {
                return Ok(_mapper.Map<SeriesDetailDto>(entity));
            }

            return Ok(_mapper.Map<SeriesDto>(entity));
        }


        [HttpPost]
        public async Task<ActionResult<SeriesDto>> CreateSeries(
            [FromBody] SeriesForUpdateDto seriesForUpdateDto)
        {
            var newSeriesEntity = _mapper.Map<Entities.Series>(seriesForUpdateDto);

            _eventHostRepository.AddSeries(newSeriesEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                var seriesToReturnDto = _mapper.Map<SeriesDto>(newSeriesEntity);

                return CreatedAtRoute("GetSeries",
                    new
                    {
                        id = seriesToReturnDto.Id,
                        includeEvents = false
                    },
                    seriesToReturnDto);
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to create a new series.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSeries(
            Guid id,
            SeriesForUpdateDto seriesForUpdateDto)
        {
            var seriesEntity = await _eventHostRepository
                .GetSeriesAsync(id, false);

            if (seriesEntity == null)
            {
                _logger.LogInformation("Series with id {SeriesId} was not found when trying to update it.",
                    id);
                return NotFound();
            }

            // overwrite the entity with the corresponding values in the dto
            _mapper.Map(seriesForUpdateDto, seriesEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to update the series with id {id}.");
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateSeries(
            Guid id,
            JsonPatchDocument<SeriesForUpdateDto> patchDocument)
        {
            var seriesEntity = await _eventHostRepository
                .GetSeriesAsync(id, false);

            if (seriesEntity == null)
            {
                _logger.LogInformation("Series with id {SeriesId} was not found when trying to partially update it.", id);
                return NotFound();
            }

            var seriesToPatchDto = _mapper.Map<SeriesForUpdateDto>(seriesEntity);

            patchDocument.ApplyTo(seriesToPatchDto, ModelState);

            // check for any errors in the patch document
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("An issue was found in the patch document when trying to patch id {SeriesId}.", id);
                return BadRequest(ModelState);
            }

            // check for broken validation rules on the model
            if (!TryValidateModel(seriesToPatchDto))
            {
                _logger.LogInformation("Validation issue(s) was/were found when trying to patch id {SeriesId}.", id);
                return BadRequest(ModelState);
            }

            // overwrite the entity with the corresponding dto values
            _mapper.Map(seriesToPatchDto, seriesEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to partially update the series with id {id}.");

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            var seriesEntity = await _eventHostRepository
                .GetSeriesAsync(id, false);

            if (seriesEntity == null)
            {
                _logger.LogInformation("Series with id {SeriesId} was not found when trying to partially update it.", id);
                return NotFound();
            }

            _eventHostRepository.DeleteSeries(seriesEntity);

            if (await _eventHostRepository.SaveChangesAsync())
            {
                _mailService.Send($"Series Deleted: {seriesEntity.Title}",
                $"A series called '{seriesEntity.Title}' with id '{id}' was deleted.");
                return NoContent();
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    $"A problem occurred when trying to delete the series '{seriesEntity.Title}' with id {id}.");
        }

    }
}
