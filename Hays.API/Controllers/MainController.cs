using Hays.Domain.Entities;
using Hays.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hays.API.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        protected ActionResult<Bag<T>> Result<T>(Bag<T> content) where T : BaseEntity
        {
            if(content.Data == null)
                return StatusCode(404, content);
            else if(!content.IsSuccess)
                return StatusCode(400, content);

            return StatusCode(200, content);

        }
    }
}
