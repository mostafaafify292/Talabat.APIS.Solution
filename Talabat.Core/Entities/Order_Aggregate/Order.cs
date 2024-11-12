using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order(string buyerEmail , Address address ,DeliveryMethod deliveryMethod ,ICollection<OrderItem> items ,decimal supTotal ,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            Items = items;
  
            Address = address;
            DeliveryMethod = deliveryMethod;
            PaymentIntendId = paymentIntentId;
            SupTotal = supTotal;
            //Total = GetTotal();


        }
        public Order()
        {
            
        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; }
        public Address Address { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } // relation 1[order][Mandatory] => M[delivery][Optional]
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); //navigate proberty meny
        public decimal SupTotal { get; set; } // orderitem * Quantity
        //public decimal Total { get;  }  // suptotal + delivery

        public decimal GetTotal()
            => SupTotal + DeliveryMethod.Cost;
        public string? PaymentIntendId  { get; set; }
    }
}
