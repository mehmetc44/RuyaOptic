using RuyaOptik.Entity.Entities.Common;

namespace RuyaOptik.Entity.Entities.Concrete
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
