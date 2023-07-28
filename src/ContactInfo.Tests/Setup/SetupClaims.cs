using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class SetupClaims
{
    public static void AddUserName(ControllerBase controller, string name)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, name),
        };
        var claimsIdentity = new ClaimsIdentity(claims);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(claimsIdentity),
        };
    }
}