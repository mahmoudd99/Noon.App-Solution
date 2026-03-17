using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Entities.PruductModule;

namespace Noon.Repository.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var BrandData = File.ReadAllText("../Noon.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (Brands is not null && Brands.Count > 0)
                {
                    foreach (var brand in Brands)
                    {
                        await context.Set<ProductBrand>().AddAsync(brand);
                    }
                    await context.SaveChangesAsync();
                }

            }
            
            if (!context.ProductTypes.Any())
            {
                var BrandData = File.ReadAllText("../Noon.Repository/Data/DataSeed/types.json");
                var Brands = JsonSerializer.Deserialize<List<ProductType>>(BrandData);
                if (Brands is not null && Brands.Count > 0)
                {
                    foreach (var brand in Brands)
                    {
                        await context.Set<ProductType>().AddAsync(brand);
                    }
                    await context.SaveChangesAsync();
                }

            }
 
            if (!context.Products.Any())
            {
                var BrandData = File.ReadAllText("../Noon.Repository/Data/DataSeed/products.json");
                var Brands = JsonSerializer.Deserialize<List<Product>>(BrandData);
                if (Brands is not null && Brands.Count > 0)
                {
                    foreach (var brand in Brands)
                    {
                        await context.Set<Product>().AddAsync(brand);
                    }
                    await context.SaveChangesAsync();
                }


            }



            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("../Noon.Repository/Data/DataSeed/delivery.json");
                var Delivers = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                if (Delivers is not null && Delivers.Count > 0)
                {
                    foreach (var delivery in Delivers)
                    {
                        await context.Set<DeliveryMethod>().AddAsync(delivery);
                    }
                    await context.SaveChangesAsync();
                }


            }
        }
    }
}
