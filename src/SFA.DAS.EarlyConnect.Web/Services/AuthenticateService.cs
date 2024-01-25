using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SFA.DAS.EarlyConnect.Application.Services
{
    public interface IAuthenticateService
    {
        Task SignInUser(string email, string studentSurveyId);
    }

    public class AuthenticateService : IAuthenticateService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticateService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SignInUser(string email, string studentSurveyId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, "user"),
                new Claim("StudentSurveyId", studentSurveyId)
            };

            var identity = new ClaimsIdentity(claims, "cookie");
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}