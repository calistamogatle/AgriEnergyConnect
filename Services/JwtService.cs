using AgriEnergyConnect.Data;
using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers.Api
{
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly ILogger<AuthApiController> _logger;
        private readonly JwtService _jwtServiceInstance;
        private readonly AppDbContext _dbContext;

        public AuthApiController(ILogger<AuthApiController> logger, JwtService jwtService, AppDbContext dbContext)
        {
            _logger = logger;
            _jwtServiceInstance = jwtService;
            _dbContext = dbContext;
        }
    }
}