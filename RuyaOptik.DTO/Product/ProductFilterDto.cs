namespace RuyaOptik.DTO.Product
{
    public class ProductFilterDto
    {
        public int? CategoryId { get; set; }
        public string? Brand { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public bool? IsActive { get; set; }
    }
}
