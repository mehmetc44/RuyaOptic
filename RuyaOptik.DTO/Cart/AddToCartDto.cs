namespace RuyaOptik.DTO.Cart
{
    public class AddToCartDto
    {
        public string UserId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
