using AutoMapper;
using GDC.EventHost.API.Services;
using GDC.EventHost.DTO.Event;
using Microsoft.AspNetCore.Mvc;

namespace GDC.EventHost.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IMailService _mailService;
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public EventsController(ILogger<EventsController> logger,
            IMailService mailService,
            IEventHostRepository eventHostRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _eventHostRepository = eventHostRepository ?? throw new ArgumentNullException(nameof(eventHostRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventWithoutPerformancesDto>>> GetEvents()
        {
            var eventEntities = await _eventHostRepository.GetEventsAsync();

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
