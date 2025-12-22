namespace RuyaOptik.DTO.Order
{
    public class OrderCreateDto
    {
        public string UserId { get; set; } = null!;

        public string CustomerName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string? PostalCode { get; set; }

        public List<OrderItemCreateDto> Items { get; set; } = new();
    }
}
