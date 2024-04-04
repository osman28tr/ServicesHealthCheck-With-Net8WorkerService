using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Queries;

namespace ServicesHealthCheck.Monitoring.Controllers
{
    public class HealthCheckController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //var result = await _mediatr.Send(new GetListServiceHealthCheckQuery());
            return View();
        }
    }
}
