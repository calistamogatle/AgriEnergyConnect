using AgriEnergyConnect.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;



namespace AgriEnergyConnect.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly ILogger<AuthApiController> _logger;
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;

        [JsonIgnore]
        public object BCrypt { get; private set; }

        public AuthApiController(ILogger<AuthApiController> logger, JwtService jwtService, AppDbContext context)
        {
            _logger = logger;
            _jwtService = jwtService;
            _context = context;
        }
    }
}