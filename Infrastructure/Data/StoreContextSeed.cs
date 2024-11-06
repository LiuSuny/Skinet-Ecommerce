using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if(!context.Products.Any())
            {
                var productData = await System.IO.File.
                ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
                
                //var product = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };

                var products = JsonSerializer.Deserialize<List<Product>>(productData); //deserialize our userData into an object
                if(products == null) return;

                context.Products.AddRange(products);
                await context.SaveChangesAsync();

            }


        }
    }
}