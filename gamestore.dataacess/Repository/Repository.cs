using gamestore.dataacess.Data;
using gamestore.dataacess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.dataacess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicatioonDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicatioonDbContext db)
        {
            
            _db = db;
            this.dbSet = _db.Set<T>();
      }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter=null, string? includeProoperties = null)
        {
            IQueryable<T> query = dbSet;
            if (Filter != null)
            {
                query = query.Where(Filter);
            }
            if (includeProoperties != null)
            {
                foreach (var item in includeProoperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProoperties);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefult(Expression<Func<T, bool>> Filter, string? includeProoperties = null)
        {
            IQueryable<T> query = dbSet;
            query= query.Where(Filter);
            if (includeProoperties != null)
            {
                foreach (var item in includeProoperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProoperties);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRage(IEnumerable<T> entity)
        {
           dbSet.RemoveRange(entity);
        }
    }
}
