using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
    {
         
         [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts
        (string? brand, string? type, string? sort)
        {
          var spec = new ProductFilterSortPaginationSpecification(brand, type, sort);
          var product = await repo.ListSpecAsync(spec);
          // return Ok(await productRepository.GetProductsAsync(brand, type, sort));
           return Ok(product);

        }

         [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          var product = await repo.GetByIdAsync(id);

          if(product is null) return NotFound();
          return product;
        }


          [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {

                repo.Add(product);
              if(await repo.SaveAllAsync())
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
             
             repo.Update(product);
             if(await repo.SaveAllAsync())
             return NoContent();
              
           return BadRequest("Failed to update products");
           
        }

          [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
          var product = await repo.GetByIdAsync(id);

          if(product == null) return NotFound();

           repo.Remove(product);
           
            if(await  repo.SaveAllAsync())
             return NoContent();
              
           return BadRequest("Failed to delete products");
           
        }

         [HttpGet("brands")]
         public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrand(int id)
        {
          
          //return Ok( await productRepository.GetProductBrandsAsync());

          
          var brandSpec = new BrandListSpecification();

          return Ok(await repo.ListSpecAsync(brandSpec));

           
        }

          [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes(int id)
        {
         
         // return Ok( await productRepository.GetProductTypesAsync());

           var brandSpec = new TypeListSpecification();

           return Ok(await repo.ListSpecAsync(brandSpec));
           
        }
      
          private bool ProductExist(int id)
          {
            return repo.Exist(id);
          }
          
    }
}