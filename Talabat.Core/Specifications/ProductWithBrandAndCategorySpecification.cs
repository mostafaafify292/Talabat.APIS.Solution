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
        public ProductWithBrandAndCategorySpecification(ProductSpecParams spec) : base(
            P =>
            //(!brandId.HasValue  || P.BrandId ==brandId)&&
            //(!categoryId.HasValue || P.CategoryId == categoryId)
            (!string.IsNullOrEmpty(spec.SearchByName) ? P.Name.ToLower().Contains(spec.SearchByName.ToLower()):true)&&
            (spec.brandId.HasValue ? P.BrandId == spec.brandId : true) &&
            (spec.categoryId.HasValue ? P.CategoryId == spec.categoryId : true)
            
            )
        {
            Includes.Add(B=>B.Brand);
            Includes.Add(B=>B.Category);
            if (!string.IsNullOrEmpty(spec.sort))
            {
                switch(spec.sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P=>P.Name);
                        break;        
                }            
            }
            else
            { AddOrderBy(P=>P.Name); }

            if (spec.pageIndex != 0 && spec.pagesize != 0)
            {
                ApplyPagination((spec.pageIndex - 1) * spec.pagesize, spec.pagesize);
            }
            
            
           
            
        }
        public ProductWithBrandAndCategorySpecification(int? brandId ,int? categoryId ,string? searchByName)
              :base(P=>
                    (!string.IsNullOrEmpty(searchByName) ? P.Name.ToLower().Contains(searchByName.ToLower()) : true) &&
                   (brandId.HasValue ? P.BrandId ==brandId : true) &&
                   (categoryId.HasValue ? P.CategoryId == categoryId : true)
                   )
        {
            
        }
    }
}
