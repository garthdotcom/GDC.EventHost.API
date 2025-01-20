using AutoMapper;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Member;
using Microsoft.AspNetCore.Mvc;

namespace GDC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public UsersController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }

        [HttpGet("{userId}", Name = "GetMemberByMembershipNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<MemberDetailDto>> GetMemberByMembershipNumber(string membershipNumber,
            ApiVersion version)
        {
            if (string.IsNullOrEmpty(membershipNumber))
            {
                return BadRequest();
            }

            var memberFromRepo = await _eventHostRepository
                .GetMemberByMembershipNumberAsync(membershipNumber);

            if (memberFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MemberDetailDto>(memberFromRepo));
        }

    }
}
