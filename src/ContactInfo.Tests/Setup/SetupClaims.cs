using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class SetupClaims
{
    public static ClaimsPrincipal CreateClaims(string name, int id)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.NameIdentifier, id.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims);

        return new ClaimsPrincipal(claimsIdentity);
    }

    public static void AddUserInfo(ControllerBase controller, string name, int id)
    {
        var claimsPrincipal = CreateClaims(name, id);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = claimsPrincipal,
        };
    }
}