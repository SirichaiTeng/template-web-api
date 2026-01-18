using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OriginalExample.Interfaces.IServices;
using OriginalExample.Models.Response;

namespace OriginalExample.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet("getuser")]
    public async Task<IActionResult> GetUser([FromServices]IUserService userService)
    {
        var result = await userService.GetUserInfoAsync();
        if(result != null)
            return Ok(new ApiResponse<object>("200","", result, true));

        return BadRequest(new ApiResponse<object>("400", "", result, false));
    }
}
