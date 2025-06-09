using AgriEnergyConnect.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriEnergyConnect.Controllers
{
    [AllowAnonymous]
    public class AccountController(ILogger<AccountController> logger) : Controller
    {

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Replace with your actual authentication logic
                    AuthUser? user = await AuthenticateUser(model.Email, model.Password);

                    if (user != null)
                    {
                        List<Claim> claims = new()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Role, user.Role)
                        };

                        ClaimsIdentity claimsIdentity = new(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties authProperties = new()
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = model.RememberMe ?
                                DateTimeOffset.UtcNow.AddDays(30) : null
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        logger.LogInformation("User {Email} logged in at {Time}",
                            model.Email, DateTime.UtcNow);

                        return LocalRedirect(returnUrl ?? Url.Content("~/"));
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Login failed for {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "An error occurred during login.");
                }
            }

            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            AuthResponse response = new();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Please correct the validation errors";
                return View(model);
            }

            try
            {
                AuthUser? user = await RegisterUser(model.Email, model.Password);

                if (user != null)
                {
                    response.Success = true;
                    response.Message = "Registration successful!";
                    response.RedirectUrl = Url.Content("~/");

                    // Auto-login
                    _ = await Login(new LoginViewModel
                    {
                        Email = model.Email,
                        Password = model.Password
                    });

                    return LocalRedirect(response.RedirectUrl);
                }

                response.Success = false;
                response.Message = "Registration failed";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred";
                response.ErrorDetails = ex.Message;
                logger.LogError(ex, "Registration failed");
            }

            return View(model);
        }
        

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            logger.LogInformation("User logged out at {Time}", DateTime.UtcNow);
            return RedirectToAction("Index", "Home");
        }

        // TODO: Implement these methods with your actual user store
        private static async Task<AuthUser?> AuthenticateUser(string email, string password)
        {
            // Replace with your actual authentication logic
            // This is just a placeholder implementation
            return await Task.FromResult(new AuthUser
            {
                Id = "1",
                Email = email,
                Name = "Test User",
                Role = "Farmer" // or "Employee" 
            });
        }

        private static async Task<AuthUser?> RegisterUser(string email, string password)
        {
            // Replace with your actual registration logic
            // This is just a placeholder implementation
            return await Task.FromResult(new AuthUser
            {
                Id = "2",
                Email = email,
                Name = "New User",
                Role = "Farmer"
            });
        }

        // Helper model for authentication results
        private class AuthUser
        {
            [JsonIgnore]
            public string Id { get; set; } = null!;
            [JsonIgnore]
            public string Email { get; set; } = null!;
            [JsonIgnore]
            public string Name { get; set; } = null!;
            [JsonIgnore]
            public string Role { get; set; } = null!;
        }
    }
}