using Microsoft.AspNetCore.Mvc;

namespace ServicesHealthCheck.Monitoring.Controllers
{
    public class HealthCheckVisualizationController : Controller
    {
        public IActionResult Index()
        {
            var data = new int[] { 10, 20, 30, 40, 50 }; // Örnek CPU kullanımı

            return View(data);
        }
    }
}
