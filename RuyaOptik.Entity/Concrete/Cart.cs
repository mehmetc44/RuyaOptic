using RuyaOptik.Entity.Common;

namespace RuyaOptik.Entity.Concrete
{
    public class Cart : BaseEntity
    {        
        public string UserId { get; set; } = null!;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }
    }
}
