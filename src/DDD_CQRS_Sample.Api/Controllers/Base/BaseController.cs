using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace DDD_CQRS_Sample.Api.Controllers.Base;

public class BaseController : ControllerBase
{
    protected IActionResult ResponseResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}