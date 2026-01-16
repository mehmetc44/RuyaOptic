using RuyaOptik.Entity.Common;
namespace RuyaOptik.Entity.Concrete
{
    public class Inventory : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }     // Toplam stok
        public int Reserved { get; set; }     // Siparişte ayrılan stok
        public int MinimumThreshold { get; set; } = 0; // Kritik seviye

        public string? Location { get; set; } // Depo / raf bilgisi
    }
}
