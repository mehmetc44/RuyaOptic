using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Interfaces
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task<Inventory?> GetByProductIdAsync(int productId);
    }
}
