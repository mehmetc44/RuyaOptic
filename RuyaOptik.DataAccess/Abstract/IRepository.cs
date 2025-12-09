using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RuyaOptik.DataAccess.Abstract
{
    public interface IRepository<T> where T : class
    {
        List<T> GetList();
        T GetById(int id);
        T GetByFilter(Expression<Func<T, bool>> predicate);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
        int Count();
        int FilteredCount(Expression<Func<T, bool>> predicate);
        List<T> GetListByFilter(Expression<Func<T, bool>> predicate);
    }
}