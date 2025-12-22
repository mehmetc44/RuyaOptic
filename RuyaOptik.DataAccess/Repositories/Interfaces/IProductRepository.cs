using RuyaOptik.Entity.Concrete;
using System.Linq.Expressions;

namespace RuyaOptik.DataAccess.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetActiveProductsAsync();

        // Pagination + Filtering (Expression ile)
        Task<int> CountAsync(Expression<Func<Product, bool>> predicate);

        Task<List<Product>> GetPagedAsync(
            Expression<Func<Product, bool>> predicate,
            int skip,
            int take);
    }
}
