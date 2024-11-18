using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
       
       public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager,
        ClaimsPrincipal user) 
        {
              var userToRetun = await userManager.Users.
              FirstOrDefaultAsync(x => x.Email == user.GetUserEmail()) 
              ?? throw new AuthenticationException("Cannot get user email from cookies");

              return userToRetun;
        }

       public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager,
        ClaimsPrincipal user) 
        {
              var userToRetun = await userManager.Users
                     .Include(x => x.Address)
                     .FirstOrDefaultAsync(x => x.Email == user.GetUserEmail()) 
              ?? throw new AuthenticationException("Cannot get user email from cookies");

              return userToRetun;
        }

         
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
             var email = user.FindFirstValue(ClaimTypes.Email) 
            ?? throw new AuthenticationException("Cannot get user email from cookies");
           return email;
           
        }

          public static string GetUserId(this ClaimsPrincipal user)
        {
            ////this give us user by id from the token base authentication
        //     var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
        //   ?? throw new Exception("Cannot get username from token")); // static int GetUserId
         
         var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
          ?? throw new Exception("Cannot get username from token");
           return userId;
             
        }
    }
}