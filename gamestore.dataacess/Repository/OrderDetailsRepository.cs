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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicatioonDbContext _db;
        public OrderDetailsRepository(ApplicatioonDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderDetails obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
