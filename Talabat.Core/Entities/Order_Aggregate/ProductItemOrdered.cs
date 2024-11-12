using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered(int productId , string productName , string productURL)
        {
            ProductId = productId ;
            ProductName = productName ;
            ProductURL = productURL ;
        }
        public ProductItemOrdered()
        {
            
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductURL { get; set; }
    }
}
