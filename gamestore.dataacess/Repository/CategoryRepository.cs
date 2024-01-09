using gamestore.dataacess.Data;
using gamestore.dataacess.Repository.IRepository;
using gamestore.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.dataacess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicatioonDbContext _db;
        public CategoryRepository(ApplicatioonDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
