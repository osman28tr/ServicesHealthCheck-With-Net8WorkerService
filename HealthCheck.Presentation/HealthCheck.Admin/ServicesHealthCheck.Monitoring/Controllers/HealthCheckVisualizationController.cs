using Microsoft.AspNetCore.Mvc;

namespace ServicesHealthCheck.Monitoring.Controllers
{
    public class HealthCheckVisualizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
