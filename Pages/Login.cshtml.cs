
using System.Security.Claims;
using JustShortIt.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuthenticationService = JustShortIt.Service.AuthenticationService;

namespace JustShortIt.Pages; 

public class LoginModel : PageModel {
    [BindProperty]
    public User? UserModel { get; set; }

    private AuthenticationService Authentication { get; }

    public LoginModel(AuthenticationService authentication) {
        Authentication = authentication;
    }

    public async Task<IActionResult> OnPostAsync() {
        if (!ModelState.IsValid) return Page();

        if (Authentication.IsUser(UserModel!.Username, UserModel!.Password)) {

            var claims = new List<Claim> {
                new(ClaimTypes.Name, UserModel.Username),
                new(ClaimTypes.Role, "Administrator")
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                IssuedUtc = DateTimeOffset.UtcNow,
                RedirectUri = "/"
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity), 
                properties);

            return LocalRedirect("~/urls");
        }

        ModelState.AddModelError(string.Empty, "Invalid Username or Password");
        return Page();
    }

    public IActionResult OnGet() => Page();
}