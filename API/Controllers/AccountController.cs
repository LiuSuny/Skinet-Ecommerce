using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
    {
        
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
             var user = new AppUser
             {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
             };
            
            //Created 
             var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);
             //next we check if it doesnt succeded
              if(!result.Succeeded)
              {
                  foreach(var error in result.Errors)
                  {
                     ModelState.AddModelError(error.Code, error.Description);
                  }
                  return ValidationProblem();
              }

              return Ok();             
              
        }
       
       [Authorize]
       [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
             await signInManager.SignOutAsync();
             return NoContent();
        }
      
      
       [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
          //check if user is authenticated
          if(User.Identity?.IsAuthenticated == false) return NoContent();
            
            var user  = await signInManager.UserManager.GetUserByEmailWithAddress(User);
                 
                //return a new annonumus with new type
                 return Ok( new {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    Address = user.Address?.ToDto()
                 });
        }

    
    //this endpoint just tell us if users is authenticated
       [HttpGet]
        public ActionResult GetAuthState()
        {
                 return Ok( new {
                    IsAuthenticated = User.Identity?.IsAuthenticated ?? false
                 });
        }

         [Authorize]
         [HttpPost("address")]
        public async Task<ActionResult<Address>> CreateOrUpdateUserAddress(AddressDto addressDto)
        {
             var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

              //check if user address is null
              if(user.Address == null)
              {
                 user.Address = addressDto.ToEntity();
              }
              else{
               user.Address.UpdateFromDto(addressDto);
              }

              var result = await signInManager.UserManager.UpdateAsync(user);

               if(!result.Succeeded)
              {
                 return BadRequest("Problem updating user address");
              }

              return Ok(user.Address.ToDto());             
              
        }
      
    }
}