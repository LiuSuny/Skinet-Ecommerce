using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ProductsController(IProductRepository productRepository) : BaseApiController
    {
         
         [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts
        (string? brand, string? type, string? sort)
        {
           return Ok(await productRepository.GetProductsAsync(brand, type, sort));
        }

         [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          var product = await productRepository.GetProductByIdAsync(id);

          if(product is null) return NotFound();
          return product;
        }


          [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {

                productRepository.AddProduct(product);
              if(await productRepository.SaveChangesAsync())
              {
                return CreatedAtAction("GetProduct", new {id = product.Id}, product);
              }
           
               return BadRequest("Failed to create products");
        }

         [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if(product.Id != id || !ProductExist(id))
             return BadRequest("Cannot update the products");
             
             productRepository.UpdateProduct(product);
             if(await productRepository.SaveChangesAsync())
             return NoContent();
              
           return BadRequest("Failed to update products");
           
        }

          [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
          var product = await productRepository.GetProductByIdAsync(id);

          if(product == null) return NotFound();

           productRepository.DeleteProduct(product);
           
            if(await productRepository.SaveChangesAsync())
             return NoContent();
              
           return BadRequest("Failed to delete products");
           
        }

          [HttpGet("brands")]
         public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrand(int id)
        {
          
          return Ok( await productRepository.GetProductBrandsAsync());
           
        }

          [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes(int id)
        {
         
          return Ok( await productRepository.GetProductTypesAsync());
           
        }
      
          private bool ProductExist(int id)
          {
            return productRepository.ProductExist(id);
          }
          
    }
}