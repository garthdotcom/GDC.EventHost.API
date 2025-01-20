using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Order;
using GDC.EventHost.Shared.SeatPosition;
using GDC.EventHost.Shared.Ticket;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using static GDC.EventHost.Shared.Enums;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/orders")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public OrdersController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(
            [FromQuery] OrderResourceParameters resourceParms, 
            ApiVersion version)
        {
            var cartFromRepo = await _eventHostRepository.GetOrdersAsync(resourceParms);

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(cartFromRepo));
        }


        [HttpGet("{orderId}", Name = "GetOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId,
            ApiVersion version)
        {
            if (orderId == Guid.Empty)
            {
                return BadRequest();
            }

            var cartFromRepo = await _eventHostRepository.GetOrderByIdAsync(orderId);

            if (cartFromRepo == null)
            {
                return NotFound();
            }

            var cartDto = _mapper.Map<OrderDto>(cartFromRepo);

            foreach (var orderItem in cartDto.OrderItems)
            {
                var ticketFromRepo = await _eventHostRepository.GetTicketByIdAsync(orderItem.Ticket.Id);

                orderItem.Ticket = _mapper.Map<TicketDetailDto>(ticketFromRepo);

                // get the seat positions for the seat by walking up the ParentId chain

                var seatPositionsFromRepo = _eventHostRepository
                    .GetSeatPositionsForSeat(orderItem.Ticket.Seat.ParentId);

                if (seatPositionsFromRepo.Count() > 0)
                {
                    orderItem.Ticket.Seat.SeatPositions =
                        _mapper.Map<IEnumerable<SeatPositionDisplayDto>>(seatPositionsFromRepo).ToList();
                }
            }

            return Ok(cartDto);
        }


        [HttpPost(Name = "CreateOrder")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderForUpdateDto orderForUpdateDto,
            ApiVersion version)
        {
            if (orderForUpdateDto == null)
            {
                return BadRequest();
            }

            orderForUpdateDto.OrderStatusId = OrderStatusEnum.Incomplete;

            var orderToReturn = await InsertOrderAsync(orderForUpdateDto);

            return CreatedAtRoute("GetOrderById",
                new
                {
                    orderId = orderToReturn.Id,
                    version = $"{version}"
                },
                orderToReturn);
        }


        [HttpPut("{orderId}", Name = "UpdateOrder")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder(Guid orderId, OrderForUpdateDto orderForUpdateDto,
            ApiVersion version)
        {
            if (orderId == Guid.Empty || orderForUpdateDto == null)
            {
                return BadRequest();
            }

            var orderFromRepo = await _eventHostRepository.GetOrderByIdAsync(orderId);

            if (orderFromRepo == null)
            {
                var orderToReturn = await InsertOrderAsync(orderForUpdateDto);

                return CreatedAtRoute("GetOrderById",
                    new
                    {
                        orderId = orderToReturn.Id,
                        version = $"{version}"
                    },
                    orderToReturn);
            }

            _mapper.Map(orderForUpdateDto, orderFromRepo);
            //await _eventHostRepository.UpdateOrderAsync(orderFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            if (orderForUpdateDto.OrderStatusId == OrderStatusEnum.Complete ||
                orderForUpdateDto.OrderStatusId == OrderStatusEnum.Cancelled)
            {
                // the order is complete. remove the items from the shopping cart

                var cartFromRepo = await _eventHostRepository
                    .GetShoppingCartByMemberIdAsync(orderFromRepo.MemberId);

                if (cartFromRepo == null)
                {
                    return NotFound();
                }

                foreach (var cartItem in cartFromRepo.ShoppingCartItems)
                {
                    _eventHostRepository.DeleteShoppingCartItem(cartItem);
                }

                await _eventHostRepository.SaveChangesAsync();

            }

            return NoContent();
        }


        [HttpPatch("{orderId}", Name = "PartiallyUpdateOrder")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PartiallyUpdateOrder(Guid orderId,
            JsonPatchDocument<OrderForPatchDto> patchDocument,
            ApiVersion version)
        {
            if (orderId == Guid.Empty || patchDocument == null)
            {
                return BadRequest();
            }

            var orderFromRepo = await _eventHostRepository.GetOrderByIdAsync(orderId);

            if (orderFromRepo == null)
            {
                return NotFound();
            }

            var orderToPatch = _mapper.Map<OrderForPatchDto>(orderFromRepo);

            patchDocument.ApplyTo(orderToPatch, ModelState);

            if (!TryValidateModel(orderToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(orderToPatch, orderFromRepo);
            //await _eventHostRepository.UpdateOrderAsync(orderFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{orderId}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(Guid orderId,
            ApiVersion version)
        {
            if (orderId == Guid.Empty)
            {
                return BadRequest();
            }

            var orderFromRepo = await _eventHostRepository.GetOrderByIdAsync(orderId);

            if (orderFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteOrder(orderFromRepo);

            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{orderId}/shoppingcartitems", Name = "DeleteOrderItems")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrderItems(Guid orderId,
            ApiVersion version)
        {
            //todo: verify the cascade does not remove tickets

            if (orderId == Guid.Empty)
            {
                return BadRequest();
            }

            var cartFromRepo = await _eventHostRepository.GetOrderByIdAsync(orderId);

            if (cartFromRepo == null)
            {
                return NotFound();
            }

            foreach (var cartItem in cartFromRepo.OrderItems)
            {
                _eventHostRepository.DeleteOrderItem(cartItem);
            }

            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************

        [HttpHead(Name = "GetOrderMetadata")]
        public ActionResult GetOrderMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetOrderOptions")]
        public ActionResult GetOrderOptions(ApiVersion version)
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

        private async Task<OrderDto> InsertOrderAsync(OrderForUpdateDto cartForUpdateDto)
        {
            var order = _mapper.Map<Order>(cartForUpdateDto);
            await _eventHostRepository.AddOrderAsync(order);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<OrderDto>(order);
        }

        #endregion
    }
}