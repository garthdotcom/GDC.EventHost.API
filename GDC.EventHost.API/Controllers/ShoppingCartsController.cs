using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.SeatPosition;
using GDC.EventHost.Shared.ShoppingCart;
using GDC.EventHost.Shared.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GHC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/shoppingcarts")]
    [ApiController]
    public class ShoppingCartsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public ShoppingCartsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet(Name = "GetShoppingCarts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShoppingCartDto>>> GetShoppingCarts(ApiVersion version)
        {
            var cartFromRepo = await _eventHostRepository.GetShoppingCartsAsync();

            return Ok(_mapper.Map<IEnumerable<ShoppingCartDto>>(cartFromRepo));
        }


        [HttpGet("{shoppingCartId}", Name = "GetShoppingCartById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartById(Guid shoppingCartId,
            ApiVersion version)
        {
            if (shoppingCartId == Guid.Empty)
            {
                return BadRequest();
            }

            var cartFromRepo = await _eventHostRepository.
                GetShoppingCartByIdAsync(shoppingCartId);

            if (cartFromRepo == null)
            {
                return NotFound();
            }

            var cartDto = _mapper.Map<ShoppingCartDto>(cartFromRepo);

            foreach (var shoppingCartItem in cartDto.ShoppingCartItems)
            {
                var ticketFromRepo = await _eventHostRepository
                    .GetTicketByIdAsync(shoppingCartItem.Ticket.Id);

                shoppingCartItem.Ticket = _mapper.Map<TicketDetailDto>(ticketFromRepo);

                // get the seat positions for the seat by walking up the ParentId chain

                var seatPositionsFromRepo = _eventHostRepository
                    .GetSeatPositionsForSeat(shoppingCartItem.Ticket.Seat.ParentId);

                if (seatPositionsFromRepo.Count() > 0)
                {
                    shoppingCartItem.Ticket.Seat.SeatPositions = 
                        _mapper.Map<IEnumerable<SeatPositionDisplayDto>>(seatPositionsFromRepo).ToList();
                }
            }

            return Ok(cartDto);
        }


        [HttpPost(Name = "CreateShoppingCart")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ShoppingCartDto>> CreateShoppingCart(ShoppingCartForUpdateDto cartDto,
            ApiVersion version)
        {
            if (cartDto == null)
            {
                return BadRequest();
            }

            var cartToReturn = await InsertShoppingCartAsync(cartDto);

            return CreatedAtRoute("GetShoppingCartById",
                new
                {
                    shoppingCartId = cartToReturn.Id,
                    version = $"{version}"
                },
                cartToReturn);
        }


        [HttpPut("{shoppingCartId}", Name = "UpdateShoppingCart")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateShoppingCart(Guid shoppingCartId, ShoppingCartForUpdateDto cartForUpdateDto,
            ApiVersion version)
        {
            if (shoppingCartId == Guid.Empty || cartForUpdateDto == null)
            {
                return BadRequest();
            }

            var cartFromRepo = await _eventHostRepository
                .GetShoppingCartByIdAsync(shoppingCartId);

            if (cartFromRepo == null)
            {
                var cartToReturn = await InsertShoppingCartAsync(cartForUpdateDto);

                return CreatedAtRoute("GetShoppingCartById",
                    new
                    {
                        shoppingCartId = cartToReturn.Id,
                        version = $"{version}"
                    },
                    cartToReturn);
            }

            _mapper.Map(cartForUpdateDto, cartFromRepo);
            //await _eventHostRepository.UpdateShoppingCartAsync(cartFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{shoppingCartId}", Name = "DeleteShoppingCart")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteShoppingCart(Guid shoppingCartId,
            ApiVersion version)
        {
            //todo: verify the cascade does not remove tickets

            if (shoppingCartId == Guid.Empty)
            {
                return BadRequest();
            }

            var shoppingCartFromRepo = await _eventHostRepository
                .GetShoppingCartByIdAsync(shoppingCartId);

            if (shoppingCartFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteShoppingCart(shoppingCartFromRepo);

            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{shoppingCartId}/shoppingcartitems", Name = "DeleteShoppingCartItems")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteShoppingCartItems(Guid shoppingCartId,
            ApiVersion version)
        {
            //todo: verify the cascade does not remove tickets

            if (shoppingCartId == Guid.Empty)
            {
                return BadRequest();
            }

            var cartFromRepo = await _eventHostRepository
                .GetShoppingCartByIdAsync(shoppingCartId);

            if (cartFromRepo == null)
            {
                return NotFound();
            }

            foreach (var cartItem in cartFromRepo.ShoppingCartItems)
            {
                _eventHostRepository.DeleteShoppingCartItem(cartItem);
            }

            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }


        //*************************************************************************************

        [HttpHead(Name = "GetShoppingCartMetadata")]
        public ActionResult GetShoppingCartMetadata(ApiVersion version)
        {
            return Ok();
        }


        [HttpOptions(Name = "GetShoppingCartOptions")]
        public ActionResult GetShoppingCartOptions(ApiVersion version)
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,DELETE,OPTIONS");
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

        private async Task<ShoppingCartDto> InsertShoppingCartAsync(ShoppingCartForUpdateDto cartForUpdateDto)
        {
            var shoppingCart = _mapper.Map<ShoppingCart>(cartForUpdateDto);
            await _eventHostRepository.AddShoppingCartAsync(shoppingCart);
            await _eventHostRepository.SaveChangesAsync();
            return _mapper.Map<ShoppingCartDto>(shoppingCart);
        }

        #endregion

    }
}