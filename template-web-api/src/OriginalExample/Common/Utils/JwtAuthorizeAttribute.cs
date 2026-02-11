using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OriginalExample.Interfaces.IServices;
using OriginalExample.Models.Response;

namespace OriginalExample.Common.Utils;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class JwtAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public JsonResult UnAuthorization(string message)
    {
        return new JsonResult(new ApiResponse<string>("401", message, null, false))
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };
    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;

        // ✅ ดึง token จาก header
        var token = httpContext.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            context.Result = UnAuthorization("Token not found");
            return;
        }

        // ✅ resolve service จาก DI
        var jwtService = httpContext.RequestServices
            .GetRequiredService<ITokenService>();

        var result = await jwtService.ValidateTokenAsync(token);

        if (!result)
        {
            context.Result = UnAuthorization("Invalid token");
            return;
        }

        // (Optional) attach user to HttpContext
        httpContext.Items["User"] = result;
    }
}
