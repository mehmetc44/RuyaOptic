namespace RuyaOptik.DTO.Inventory
{
    public class InventoryCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int MinimumThreshold { get; set; }
        public string? Location { get; set; }
    }
}
