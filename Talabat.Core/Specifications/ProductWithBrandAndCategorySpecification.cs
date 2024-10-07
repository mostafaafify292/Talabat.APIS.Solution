using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndCategorySpecification :BaseSpecification<Product>
    {
        public ProductWithBrandAndCategorySpecification(int id):base(P=>P.Id==id)
        {
            Includes.Add(B => B.Brand);
            Includes.Add(B => B.Category);
        }
        public ProductWithBrandAndCategorySpecification():base()
        {
            Includes.Add(B=>B.Brand);
            Includes.Add(B=>B.Category);
        }
    }
}
