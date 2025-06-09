using System.Text;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AgriEnergyConnect
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add configuration
            _ = builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _ = builder.Configuration.AddEnvironmentVariables();

            // Database Configuration
            _ = builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity Configuration
            _ = builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // JWT Authentication
            _ = builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured."),
                    ValidAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not configured."),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured."))),
                    ClockSkew = TimeSpan.Zero // Remove tolerance for token expiration
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            _ = builder.Services.AddAuthorization();

            // Add application services
            _ = builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            _ = builder.Services.AddScoped<IFarmerRepository, FarmerRepository>();
            _ = builder.Services.AddScoped<IProductRepository, ProductRepository>();
            _ = builder.Services.AddScoped<IJwtService, JwtService>();
            _ = builder.Services.AddScoped<IAuthService, AuthService>();

            // MVC Configuration
            _ = builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            })
            .AddRazorRuntimeCompilation();

            // Build the application
            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error");
                _ = app.UseHsts();
            }

            _ = app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            _ = app.UseHttpsRedirection();
            _ = app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
                }
            });

            _ = app.UseRouting();
            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Database initialization
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                try
                {
                    AppDbContext context = services.GetRequiredService<AppDbContext>();
                    UserManager<IdentityUser> userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await context.Database.MigrateAsync();
                    SeedData.Initialize(context, userManager, roleManager);
                }
                catch (Exception ex)
                {
                    ILogger logger = services.GetRequiredService<ILogger>();
                    ProgramLogHelpers.LogDatabaseInitializationFailed(logger, ex);
                }
            }

            await app.RunAsync();
        }
    }

    public static class ProgramLogHelpers
    {
        private static readonly Action<ILogger, string, Exception?> _logDatabaseInitializationFailed =
            LoggerMessage.Define<string>(
                LogLevel.Critical,
                new EventId(1, nameof(LogDatabaseInitializationFailed)),
                "Database initialization failed: {Message}");

        public static void LogDatabaseInitializationFailed(ILogger logger, Exception exception)
        {
            _logDatabaseInitializationFailed(logger, exception.Message, exception);
        }
    }
}