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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicatioonDbContext _db;
        public OrderHeaderRepository(ApplicatioonDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }
        public void UpdateStatus(int id, string OrderStatus, string? PaymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb == null)
            {
                orderFromDb.OrderStatus = OrderStatus;
                if (PaymentStatus != null)
                {
                    orderFromDb.PaymrntStatus = PaymentStatus;
                }
            }
        }
        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentIntentId = paymentIntentId;
        }


    }
}
