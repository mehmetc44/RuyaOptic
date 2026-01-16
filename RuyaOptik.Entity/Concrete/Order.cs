using RuyaOptik.Entity.Enums;
using RuyaOptik.Entity.Common;

namespace RuyaOptik.Entity.Concrete
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; } = null!;  // Identity kullanıcısı

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Teslimat bilgileri
        public string CustomerName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string? PostalCode { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
