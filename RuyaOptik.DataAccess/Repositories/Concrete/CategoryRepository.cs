using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.Entity.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Concrete
{
    public class CategoryRepository : EfRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(RuyaOptikDbContext context) : base(context)
        {
        }
    }
}
