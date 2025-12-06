namespace RuyaOptik.Entity.Entities.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // Soft delete için
        public bool IsDeleted { get; set; } = false;
    }
}
