using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Esfa.Recruit.Provider.Web.Configuration
{
    public  class AuthenticationExtension
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.Role, "user"),
                new Claim("IsLoggedIn", "true")
                // Add any other claims as needed
            };

            var identity = new ClaimsIdentity(claims, "cookie");
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}