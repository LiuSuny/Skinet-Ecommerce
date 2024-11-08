using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        
        protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repo, 
            ISpecification<T> spec, int pageSize, int pageIndex)  where T : BaseEntity
        {
            var items = await repo.ListSpecAsync(spec);
            var count = await repo.CountAsync(spec);

            var pagination = new Pagination<T>(pageIndex, pageSize, count, items);
            return Ok(pagination);
        } 
    }
}