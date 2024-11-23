using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ProductsController(IUnitOfWork unitOfWork) : BaseApiController
    {
         
         [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery] ProductSpecParams specParams)
        {
          var spec = new ProductFilterSortPaginationSpecification(specParams);

            return await CreatePagedResult(unitOfWork.Repository<Product>(), spec, specParams.PageIndex, specParams.PageSize);
          

        }

         [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);

          if(product is null) return NotFound();
          return product;
        }


          [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {

                unitOfWork.Repository<Product>().Add(product);
              if(await unitOfWork.Complete())
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
             
             unitOfWork.Repository<Product>().Update(product);
             if(await unitOfWork.Complete())
             return NoContent();
              
           return BadRequest("Failed to update products");
           
        }

          [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
          var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);

          if(product == null) return NotFound();

           unitOfWork.Repository<Product>().Remove(product);
           
            if(await  unitOfWork.Complete())
             return NoContent();
              
           return BadRequest("Failed to delete products");
           
        }

         [HttpGet("brands")]
         public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrand(int id)
        {
          
          //return Ok( await productRepository.GetProductBrandsAsync());

          
          var brandSpec = new BrandListSpecification();

          return Ok(await unitOfWork.Repository<Product>().ListSpecAsync(brandSpec));

           
        }

          [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes(int id)
        {
         
         // return Ok( await productRepository.GetProductTypesAsync());

           var brandSpec = new TypeListSpecification();

           return Ok(await unitOfWork.Repository<Product>().ListSpecAsync(brandSpec));
           
        }
      
          private bool ProductExist(int id)
          {
            return unitOfWork.Repository<Product>().Exist(id);
          }
          
    }
}