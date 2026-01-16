using RuyaOptik.Entity.Enums;

namespace RuyaOptik.DTO.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
