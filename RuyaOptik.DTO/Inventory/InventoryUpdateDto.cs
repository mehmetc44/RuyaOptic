namespace RuyaOptik.DTO.Inventory
{
    public class InventoryUpdateDto
    {
        public int Quantity { get; set; }
        public int Reserved { get; set; }
        public int MinimumThreshold { get; set; }
        public string? Location { get; set; }
    }
}
