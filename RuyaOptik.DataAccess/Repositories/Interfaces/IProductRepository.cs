using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetActiveProductsAsync();

        // PAGINATION
        Task<int> CountAsync();
        Task<List<Product>> GetPagedAsync(int skip, int take);
    }
}
