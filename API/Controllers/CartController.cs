using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController(ICartService cartService ): BaseApiController
    {
        

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            var carts = await cartService.GetCartAsync(id);
           // if(carts == null) return NotFound("Cart Null");
             return Ok(carts ?? new ShoppingCart{Id = id});
        }

         [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updateCart = await cartService.SetCartAsync(cart);
            if(updateCart == null) return BadRequest("Problem with cart");
             return updateCart;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var deleteCart = await cartService.DeleteCartAsync(id);
            if(!deleteCart) return BadRequest("Failed to delete cart");
             return NoContent();
        }
    }
}