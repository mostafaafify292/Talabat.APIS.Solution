using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeeding
    {
        public static async Task SeedAsync(StoreContext _Context)
        {
            #region ProductBrand Seeding
            var brandData = File.ReadAllText("../Talabat.Repository/Data/Data Seeding/brands.json");
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
            if (brands.Count() > 0)
            {           
                if (_Context.productBrands.Count() == 0)
                {
                    brands = brands.Select(P => new ProductBrand
                    {
                        Name = P.Name
                    }).ToList();
                    foreach (var brand in brands)
                    {
                        _Context.Set<ProductBrand>().Add(brand);
                    }
                    await _Context.SaveChangesAsync();
                }
            }
            #endregion
            #region Product Seeding
            var ProductData = File.ReadAllText("../Talabat.Repository/Data/Data Seeding/products.json");
            var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
            if (Products.Count() > 0)
            {
                if (_Context.products.Count() == 0)
                {
                    foreach (var product in Products)
                    {
                        _Context.Set<Product>().Add(product);
                    }
                    await _Context.SaveChangesAsync();
                }

            }
            #endregion
            #region Category Seeding
            var CategoryData = File.ReadAllText("../Talabat.Repository/Data/Data Seeding/categories.json");
            var categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);
            if (categories.Count() > 0)
            {
                if (_Context.productCategories.Count() == 0)
                {
                    //categories = categories.Select(C=>new ProductCategory {Name = C.Name}).ToList();
                    foreach (var category in categories)
                    {
                        _Context.Set<ProductCategory>().Add(category);
                    }
                    await _Context.SaveChangesAsync();
                }

            }
            #endregion
        }
    }
}
