using System.Diagnostics;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AgriEnergyConnect.Data;

namespace AgriEnergyConnect.Controllers
{
    public class HomeController(ILogger<HomeController> logger, AppDbContext context) : Controller
    {
        public IActionResult Index()
        {
            // Add any required initialization logic
            ViewData["TotalFarmers"] = context.Farmers.Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            IStatusCodeReExecuteFeature? errorInfo = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            IExceptionHandlerFeature? exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>();

            ErrorViewModel model = new()
            {
                StatusCode = statusCode,
                RequestPath = errorInfo?.OriginalPath,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            if (exceptionHandler?.Error != null)
            {
                logger.LogError(exceptionHandler.Error,
                    "Error occurred on {Path}", errorInfo?.OriginalPath);

                if (HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
                {
                    model.DebugMessage = exceptionHandler.Error.ToString();
                }
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return Error(StatusCodes.Status500InternalServerError);
        }
    }
}