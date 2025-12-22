namespace RuyaOptik.DTO.Inventory
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public int Reserved { get; set; }
        public int MinimumThreshold { get; set; }
        public string? Location { get; set; }
    }
}
