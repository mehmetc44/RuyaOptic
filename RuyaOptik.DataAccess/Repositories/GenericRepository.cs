using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuyaOptik.DataAccess.Abstract;
using System.Linq.Expressions;
using RuyaOptik.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace RuyaOptik.DataAccess.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();
        public int Count()
        {
            return Table.Count();
        }

        public void Create(T entity)
        {
            Table.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = Table.Find(id);
            Table.Remove(entity);
            _context.SaveChanges();
        }

        public int FilteredCount(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).Count();
        }

        public T GetByFilter(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).FirstOrDefault();
        }

        public T GetById(int id)
        {
            return Table.Find(id);
        }

        public List<T> GetList()
        {
            return Table.ToList();
        }

        public List<T> GetListByFilter(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).ToList();
        }

        public void Update(T entity)
        {
            Table.Update(entity);
            _context.SaveChanges();
        }
    }
}