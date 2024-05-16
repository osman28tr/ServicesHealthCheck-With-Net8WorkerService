using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Queries;
using ServicesHealthCheck.Monitoring.Models;

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
            try
            {
                var result = await _mediatr.Send(new GetListServiceRuleQuery());
                return View(result);
            }
            catch (Exception exception)
            {
                Log.Error("An error occurred while listing the rules for eventviewer logs. " + exception.Message);
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceRuleModel serviceRuleModel)
        {
            try
            {
                var serviceRuleCommand = new CreatedServiceRuleCommand()
                {
                    ServiceName = serviceRuleModel.ServiceName, EventMessage = serviceRuleModel.EventMessage,
                    EventType = serviceRuleModel.EventType
                };
                if (serviceRuleModel.Period != null)
                {
                    if (serviceRuleModel.Time == "Day")
                        serviceRuleCommand.RestartTime.Day = int.Parse(serviceRuleModel.Period);
                    else if (serviceRuleModel.Time == "Week")
                        serviceRuleCommand.RestartTime.Week = int.Parse(serviceRuleModel.Period);
                    else
                        serviceRuleCommand.RestartTime.Month = int.Parse(serviceRuleModel.Period);
                }
                else
                    serviceRuleCommand.RestartTime = null;
            
                await _mediatr.Send(serviceRuleCommand);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                Log.Error("An error occurred while adding rules for eventviewer logs. " + exception.Message);
                return View();
            }
        }
    }
}
