using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OriginalExample.Interfaces.IServices;
using OriginalExample.Models.Response;
using Serilog;

namespace OriginalExample.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserService userService,
        ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("getuser")]
    public async Task<IActionResult> GetUser()
    {
        _logger.LogInformation("TEST TEST");
        _logger.LogWarning("TEST TEST");
        _logger.LogError("GetUserInfo failed");
        var result = await _userService.GetUserInfoAsync();
        if(result != null)
            return Ok(new ApiResponse<object>("200","", result, true));

        return BadRequest(new ApiResponse<object>("400", "", result, false));
    }
}
