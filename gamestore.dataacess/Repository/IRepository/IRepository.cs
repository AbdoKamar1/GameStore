using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.dataacess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefult(Expression<Func<T, bool>> Filter, string? includeProoperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter=null, string? includeProoperties = null);
        void Add(T entity);
        void Remove (T entity);
        void RemoveRage(IEnumerable<T> entity);

    }


}
