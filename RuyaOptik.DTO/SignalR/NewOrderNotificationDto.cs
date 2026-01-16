namespace RuyaOptik.DTO.SignalR
{
    public class NewOrderNotificationDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
