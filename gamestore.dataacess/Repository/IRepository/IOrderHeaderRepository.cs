﻿using gamestore.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.dataacess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatus(int id, string OrderStatus, string? PaymentStatus = null);
        void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);

    }
}
