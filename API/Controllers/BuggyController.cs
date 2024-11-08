using API.DTOs;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class BuggyController : BaseApiController
    {

        //use to test our 401 authorize responses
       // [Authorize]
        [HttpGet("unauthorized")]
        public IActionResult GetUnAuthorized()
        {
            return Unauthorized();
        }

        //bad request 400
        [HttpGet("badrequest")]      
        public IActionResult GetBadRequest()
        {
            return BadRequest("Not a good request");
        }

        //500 server error
        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }


       
        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a test exception");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationErrol(CreateProductDto product)
        {
            return Ok();
        }

    }
}