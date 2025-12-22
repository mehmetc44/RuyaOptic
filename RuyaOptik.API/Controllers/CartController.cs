using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Cart;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using Microsoft.AspNetCore.Authorization;
using RuyaOptik.Entity.Enums;
namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/cart/user/{userId}
        [HttpGet("user/{userId}")]
        [AuthorizeDefinition(Action=ActionType.Reading,Definition = "Definition",Menu=AuthorizeDefinitionConstants.Cart)]
        public async Task<IActionResult> GetCart(string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        // POST: api/cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var cart = await _cartService.AddToCartAsync(dto);
            return Ok(cart);
        }

        // DELETE: api/cart/item/{cartItemId}
        [HttpDelete("item/{cartItemId:int}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var ok = await _cartService.RemoveItemAsync(cartItemId);
            if (!ok) return NotFound();

            return NoContent();
        }

        // DELETE: api/cart/clear/{userId}
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> Clear(string userId)
        {
            var ok = await _cartService.ClearCartAsync(userId);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
