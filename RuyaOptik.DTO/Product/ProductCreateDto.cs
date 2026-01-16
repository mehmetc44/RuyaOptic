namespace RuyaOptik.DTO.Product
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public string? Brand { get; set; }
        public string? ModelCode { get; set; }
        public string? Color { get; set; }
        public string? FrameType { get; set; }
        public string? LensType { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }

        public int CategoryId { get; set; }
    }
}
