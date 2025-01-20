using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.ShoppingCart;
using Microsoft.AspNetCore.Mvc;

namespace GDC.EventHost.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/shoppingcartitems")]
    [ApiController]
    public class ShoppingCartItemsController : Controller
    {
        private readonly IEventHostRepository _eventHostRepository;
        private readonly IMapper _mapper;

        public ShoppingCartItemsController(IEventHostRepository eventHostRepository, IMapper mapper)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _eventHostRepository = eventHostRepository;
        }


        [HttpGet("{shoppingCartItemId}", Name = "GetShoppingCartItemById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ShoppingCartItemDto>> GetShoppingCartItemById(
            Guid shoppingCartItemId,
            ApiVersion version)
        {
            if (shoppingCartItemId == Guid.Empty)
            {
                return BadRequest();
            }

            var cartItemFromRepo = await _eventHostRepository
                .GetShoppingCartItemByIdAsync(shoppingCartItemId);

            if (cartItemFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ShoppingCartItemDto>(cartItemFromRepo));
        }


        [HttpPost(Name = "CreateShoppingCartItem")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ShoppingCartItemDto>> CreateShoppingCartItem(
            ShoppingCartItemForUpdateDto cartItemDto,
            ApiVersion version)
        {
            if (cartItemDto == null)
            {
                return BadRequest();
            }

            var shoppingCartItem = _mapper.Map<ShoppingCartItem>(cartItemDto);

            await _eventHostRepository.AddShoppingCartItemAsync(shoppingCartItem);

            await _eventHostRepository.SaveChangesAsync();

            var cartItemToReturn = _mapper.Map<ShoppingCartItemDto>(shoppingCartItem);

            return CreatedAtRoute("GetShoppingCartItemById",
                new
                {
                    shoppingCartItemId = cartItemToReturn.Id,
                    version = $"{version}"
                },
                cartItemToReturn);
        }


        [HttpDelete("{shoppingCartItemId}", Name = "DeleteShoppingCartItem")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteShoppingCartItem(Guid shoppingCartItemId,
            ApiVersion version)
        {

            if (shoppingCartItemId == Guid.Empty)
            {
                return BadRequest();
            }

            var shoppingCartItemFromRepo = await _eventHostRepository
                .GetShoppingCartItemByIdAsync(shoppingCartItemId);

            if (shoppingCartItemFromRepo == null)
            {
                return NotFound();
            }

            _eventHostRepository.DeleteShoppingCartItem(shoppingCartItemFromRepo);
            await _eventHostRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}