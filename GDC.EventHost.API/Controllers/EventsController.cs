using AutoMapper;
using GDC.EventHost.API.Services;
using GDC.EventHost.DTO.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GDC.EventHost.API.Controllers
{
    [Authorize]
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IMailService _mailService;
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public EventsController(ILogger<EventsController> logger,
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
        public async Task<ActionResult<IEnumerable<EventWithoutPerformancesDto>>> GetEvents(
            [FromQuery] string? title, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            var (eventEntities, paginationMetadata) = await _eventHostRepository
                .GetEventsAsync(title, searchQuery, pageNumber, pageSize);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<EventWithoutPerformancesDto>>(eventEntities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(Guid id, bool includePerformances)
        {
            var entity = await _eventHostRepository.GetEventAsync(id, includePerformances);

            if (entity == null)
            {
                return NotFound();
            }

            if (includePerformances)
            {
                return Ok(_mapper.Map<EventDto>(entity));
            }

            return Ok(_mapper.Map<EventWithoutPerformancesDto>(entity));
        }
    }
}
