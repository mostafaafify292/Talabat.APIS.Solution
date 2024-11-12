using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIS.DTOs
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address Address { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>(); 
        public decimal SupTotal { get; set; } // orderitem * Quantity
        public decimal Total { get; set; }   // suptotal + delivery
        public string? PaymentIntendId { get; set; }
    }
}
