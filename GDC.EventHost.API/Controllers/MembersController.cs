using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Member;
using GDC.EventHost.Shared.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/members")]
    [ApiController]
    public class MembersController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public MembersController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetMembers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MemberDetailDto>>> GetMembers(ApiVersion version)
        {
            var memberFromRepo = await _eventHostRepository.GetMembersAsync();

            return Ok(_mapper.Map<IEnumerable<MemberDetailDto>>(memberFromRepo));
        }


        [HttpGet("{memberId}/orders", Name = "GetMemberOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMemberOrders(Guid memberId, 
            ApiVersion version)
        {
            var ordersFromRepo = await _eventHostRepository.GetOrdersForMemberAsync(memberId);

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(ordersFromRepo));
        }


        [HttpGet("{memberId}", Name = "GetMemberById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<MemberDetailDto>> GetMemberByIdAsync(Guid memberId,
            ApiVersion version)
        {
            if (memberId == Guid.Empty)
            {
                return BadRequest();
            }

            var memberFromRepo = await _eventHostRepository.GetMemberByIdAsync(memberId);

            if (memberFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MemberDetailDto>(memberFromRepo));
        }


        [HttpPost(Name = "CreateMember")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<MemberDetailDto>> CreateMember(MemberForUpdateDto memberDto,
            ApiVersion version)
        {
            if (memberDto == null)
            {
                return BadRequest();
            }

            var memberToReturn = await InsertMemberAsync(memberDto);

            return CreatedAtRoute("GetMemberById",
                new
                {
                    memberId = memberToReturn.Id,
                    version = $"{version}"
                },
                memberToReturn);
        }


        [HttpPut("{memberId}", Name = "UpdateMember")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateMember(Guid memberId, MemberForUpdateDto memberDto,
            ApiVersion version)
        {
            if (memberId == Guid.Empty || memberDto == null)
            {
                return BadRequest();
            }

            var memberFromRepo = await _eventHostRepository.GetMemberByIdAsync(memberId);

            if (memberFromRepo == null)
            {
                var memberToReturn = await InsertMemberAsync(memberDto);

                return CreatedAtRoute("GetMemberById",
                    new
                    {
                        memberId = memberToReturn.Id,
                        version = $"{version}"
                    },
                    memberToReturn);
            }

            _mapper.Map(memberDto, memberFromRepo);
            //await _eventHostRepository.UpdateMemberAsync(memberFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************


        [HttpHead(Name = "GetMemberMetadata")]
        public ActionResult GetMemberMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetMemberOptions")]
        public ActionResult GetMemberOptions(ApiVersion version)
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

        private async Task<MemberDetailDto> InsertMemberAsync(MemberForUpdateDto memberDto)
        {
            var member = _mapper.Map<Member>(memberDto);
            await _eventHostRepository.AddMemberAsync(member);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<MemberDetailDto>(member);
        }

        #endregion
    }
}
