using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Queries;

namespace ServicesHealthCheck.Monitoring.Controllers
{
    public class RuleController : Controller
    {
        private readonly IMediator _mediatr;
        public RuleController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _mediatr.Send(new GetListServiceRuleQuery());
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatedServiceRuleCommand createdServiceRuleCommand)
        {
            await _mediatr.Send(createdServiceRuleCommand);
            return RedirectToAction("Index");
        }
    }
}
