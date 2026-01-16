namespace RuyaOptik.DTO.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public decimal TotalPrice { get; set; }

        public List<CartItemDto> Items { get; set; } = new();
    }
}
