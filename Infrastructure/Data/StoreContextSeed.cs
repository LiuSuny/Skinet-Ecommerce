using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if(!context.Products.Any())
            {
                var productData = await System.IO.File.
                ReadAllTextAsync(path + @"/Data/SeedData/products.json");
                
                //var product = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };

                var products = JsonSerializer.Deserialize<List<Product>>(productData); //deserialize our userData into an object
                if(products == null) return;

                context.Products.AddRange(products);
                await context.SaveChangesAsync();

            }

             if(!context.DeliveryMethods.Any())
            {
                var deliveryData = await System.IO.File.
                ReadAllTextAsync(path + @"/Data/SeedData/delivery.json");
                
                //var product = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };

                var deivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData); //deserialize our userData into an object
                if(deivery == null) return;

                context.DeliveryMethods.AddRange(deivery);
                await context.SaveChangesAsync();

            }



        }
    }
}