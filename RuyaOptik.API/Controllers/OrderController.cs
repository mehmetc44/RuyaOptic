using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Order;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
using Microsoft.AspNetCore.Authorization;
namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            var result = await _orderService.CreateAsync(dto);
            return Ok(result);
        }

        // GET: api/order/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var orders = await _orderService.GetByUserIdAsync(userId);
            return Ok(orders);
        }

        // PUT: api/order/{id}/status
        [HttpPut("{id:int}/status")]
        [Authorize("Admin")]
        [AuthorizeDefinition(Action=ActionType.Updating,Definition = "Sipariş Durumu Güncelle",Menu=AuthorizeDefinitionConstants.Order)]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] OrderStatusUpdateDto dto)
        {
            var success = await _orderService.UpdateStatusAsync(id, dto.Status);

            if (!success)
                return NotFound(new { message = "Order not found." });

            return NoContent();
        }
    }
}
