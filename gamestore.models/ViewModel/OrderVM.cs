﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.models.ViewModel
{
    public class OrderVM
    {
        public OrderHeader orderHeader {  get; set; }
        public IEnumerable<OrderDetails> orderDetails { get; set; }
    }
}
