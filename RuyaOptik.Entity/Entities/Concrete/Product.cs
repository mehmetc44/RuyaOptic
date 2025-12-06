using RuyaOptik.Entity.Entities.Common;

namespace RuyaOptik.Entity.Entities.Concrete
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public string? Brand { get; set; }        // Örn: Ray-Ban, Polaroid
        public string? ModelCode { get; set; }    // Örn: RB1234
        public string? Color { get; set; }        // Çerçeve / cam rengi
        public string? FrameType { get; set; }    // Metal, plastik, tam çerçeve, yarım çerçeve
        public string? LensType { get; set; }     // Numara, güneş, blue light vb.

        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }

        public bool IsActive { get; set; } = true;

        // Category relation
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Navigations
        public Inventory Inventory { get; set; } = null!;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
