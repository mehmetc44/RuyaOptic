using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetActiveProductsAsync();
    }
}
