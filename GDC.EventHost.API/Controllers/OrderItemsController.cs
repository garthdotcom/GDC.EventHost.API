using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Order;
using Microsoft.AspNetCore.Mvc;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/orderitems")]
    [ApiController]
    public class OrderItemsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public OrderItemsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet("{orderItemId}", Name = "GetOrderItemById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OrderItemDto>> GetOrderItemById(Guid orderItemId,
            ApiVersion version)
        {
            if (orderItemId == Guid.Empty)
            {
                return BadRequest();
            }

            var cartItemFromRepo = await _eventHostRepository.GetOrderItemByIdAsync(orderItemId);

            if (cartItemFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OrderItemDto>(cartItemFromRepo));
        }


        [HttpPost(Name = "CreateOrderItem")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem(OrderItemForUpdateDto cartItemDto,
            ApiVersion version)
        {
            if (cartItemDto == null)
            {
                return BadRequest();
            }

            var orderItem = _mapper.Map<OrderItem>(cartItemDto);

            await _eventHostRepository.AddOrderItemAsync(orderItem);

            await _eventHostRepository.SaveChangesAsync();

            var cartItemToReturn = _mapper.Map<OrderItemDto>(orderItem);

            return CreatedAtRoute("GetOrderItemById",
                new
                {
                    orderItemId = cartItemToReturn.Id,
                    version = $"{version}"
                },
                cartItemToReturn);
        }


        [HttpDelete("{orderItemId}", Name = "DeleteOrderItem")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrderItem(Guid orderItemId,
            ApiVersion version)
        {
            if (orderItemId == Guid.Empty)
            {
                return BadRequest();
            }

            var orderItemFromRepo = await _eventHostRepository.GetOrderItemByIdAsync(orderItemId);

            if (orderItemFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteOrderItem(orderItemFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}