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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicatioonDbContext _db;
        public ProductRepository(ApplicatioonDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Product obj)
        {
            var objFromDb =_db.Products.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.Price5 = obj.Price5;
                objFromDb.Price10 = obj.Price10;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.CategoryId= obj.CategoryId;
                if(obj.ImageUrl !=null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
                
            }
        
        }
    }
}
