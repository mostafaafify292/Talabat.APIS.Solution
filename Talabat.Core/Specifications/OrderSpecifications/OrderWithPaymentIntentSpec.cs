﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderWithPaymentIntentSpec :BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpec(string PaymentIntentId):base(o=>o.PaymentIntendId == PaymentIntentId)
        {
            
        }
    }
}
