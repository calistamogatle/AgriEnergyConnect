using AgriEnergyConnect.Data;
using AgriEnergyConnect.DTO;
using AgriEnergyConnect.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Services
{
    public class AuthService(
        AppDbContext context,
        IJwtService jwtService,
        IPasswordHasher<AuthUser> passwordHasher,
        ILogger<AuthService> logger) : IAuthService
    {
        private readonly AppDbContext _context = context;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IPasswordHasher<AuthUser> _passwordHasher = passwordHasher;
        private readonly ILogger<AuthService> _logger = logger;

        public async Task<AuthResult> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                // Validate email uniqueness
                if (await _context.AuthUsers.AnyAsync(u => u.Email == model.Email))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Errors = ["Email already in use"]
                    };
                }

                // Create user
                var newUser = new AuthUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    CreatedAt = DateTime.UtcNow,
                    Role = "Farmer" // Default role
                };

                newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);

                await _context.AuthUsers.AddAsync(newUser);
                _ = await _context.SaveChangesAsync();

                // Generate token
                var token = _jwtService.GenerateToken(newUser);

                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    User = MapToDto(newUser)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for {Email}", model.Email);
                return new AuthResult
                {
                    Success = false,
                    Errors = ["An error occurred during registration"]
                };
            }
        }

        public async Task<AuthResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                var user = await _context.AuthUsers
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Errors = ["Invalid credentials"]
                    };
                }

                var verificationResult = _passwordHasher.VerifyHashedPassword(
                    user, user.PasswordHash, model.Password);

                if (verificationResult != PasswordVerificationResult.Success)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Errors = ["Invalid credentials"]
                    };
                }

                user.LastLogin = DateTime.UtcNow;
                _ = await _context.SaveChangesAsync();

                return new AuthResult
                {
                    Success = true,
                    Token = _jwtService.GenerateToken(user),
                    User = MapToDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for {Email}", model.Email);
                return new AuthResult
                {
                    Success = false,
                    Errors = ["An error occurred during login"]
                };
            }
        }

        public async Task<AuthUser?> GetUserByIdAsync(string userId)
        {
            return await _context.AuthUsers.FindAsync(userId);
        }

        private static AuthUserDto MapToDto(AuthUser user)
        {
            return new()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            };
        }
    }

    public interface IJwtService
    {
    }
}