using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class SetupClaims
{
    public static void AddUserInfo(ControllerBase controller, string name, int id)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims);
        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(claimsIdentity),
        };
    }
}