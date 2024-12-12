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
        private readonly EventHostDataStore _eventHostDataStore;

        public EventsController(ILogger<EventsController> logger,
            IMailService mailService,
            EventHostDataStore eventHostDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _eventHostDataStore = eventHostDataStore ?? throw new ArgumentNullException(nameof(eventHostDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<EventDto>> GetEvents()
        {
            return Ok(_eventHostDataStore.Events);
        }

        [HttpGet("{id}")]
        public ActionResult<EventDto> GetEvent(Guid id)
        {
            var eventToReturn = _eventHostDataStore.Events.FirstOrDefault(e => e.Id == id);

            if (eventToReturn == null)
            {
                return NotFound();
            }

            return Ok(eventToReturn);
        }
    }
}
