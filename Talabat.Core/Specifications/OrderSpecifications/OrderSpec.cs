using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderSpec : BaseSpecification<Order>
    {
        public OrderSpec(string Email) : base(o=>o.BuyerEmail == Email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o=>o.Items);   

            AddOrderBy(o=>o.OrderDate);
        }
        public OrderSpec(int orderid , string buyeremail) 
            : base(o=>o.Id == orderid && o.BuyerEmail == buyeremail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
