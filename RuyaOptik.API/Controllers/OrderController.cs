using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Order;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.SignalR;
using RuyaOptik.API.Hubs;
using RuyaOptik.DTO.SignalR;

namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHubContext<OrdersHub> _hub;

        public OrderController(IOrderService orderService, IHubContext<OrdersHub> hub)
        {
            _orderService = orderService;
            _hub = hub;
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var result = await _orderService.CreateAsync(userId, dto);
            // Yeni sipariş geldi → admin paneline anlık bildirim
            var notification = new NewOrderNotificationDto
            {
                OrderId = result.Id,
                UserId = result.UserId,
                CustomerName = dto.CustomerName,
                TotalPrice = result.TotalPrice,
                CreatedDate = DateTime.UtcNow
            };

            await _hub.Clients
                .Group(OrdersHub.AdminGroup)
                .SendAsync("NewOrderCreated", notification);

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
        [Authorize(Roles = "Admin")]
        [AuthorizeDefinition(Action = ActionType.Updating, Definition = "Sipariş Durumu Güncelle", Menu = AuthorizeDefinitionConstants.Order)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatusUpdateDto dto)
        {
            var success = await _orderService.UpdateStatusAsync(id, dto.Status);

            if (!success)
                return NotFound(new { message = "Sipariş bulunamadı." });

            return NoContent();
        }
    }
}
