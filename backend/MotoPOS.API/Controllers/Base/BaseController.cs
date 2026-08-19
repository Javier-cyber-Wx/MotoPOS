using Microsoft.AspNetCore.Mvc;
using MotoPOS.API.Exceptions;

namespace MotoPOS.API.Controllers.Base;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected ActionResult HandleError(InvalidOperationException ex)
    {
        return ex switch
        {
            DuplicateException => Conflict(new { message = ex.Message }),
            NotFoundException => NotFound(new { message = ex.Message }),
            _ => Conflict(new { message = ex.Message })
        };
    }
}