using RuyaOptik.Entity.Entities.Common;

namespace RuyaOptik.Entity.Entities.Concrete
{
    public class Cart : BaseEntity
    {        
        public string UserId { get; set; } = null!;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }
    }
}
