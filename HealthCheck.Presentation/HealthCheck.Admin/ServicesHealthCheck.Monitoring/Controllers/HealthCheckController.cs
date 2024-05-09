using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Queries;
using ServicesHealthCheck.Monitoring.Models;

namespace ServicesHealthCheck.Monitoring.Controllers
{
    public class HealthCheckController : Controller
    {
        private readonly IMediator _mediatr;
        public HealthCheckController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        public async Task<IActionResult> Index()
        {
            //var result = await _mediatr.Send(new GetListServiceHealthCheckQuery());
            return View();
        }
        [HttpGet("HealthCheck/ErrorMessages")]
        public async Task<IActionResult> ErrorMessages()
        {
            var result = await _mediatr.Send(new GetListServiceErrorLogQuery());
            if (result.Errors.Any() || result.ErrorList.Any())
            {
                if (result.ErrorList.Any())
                {
                    return View(result.ErrorList);
                }

                if (result.Errors.Any())
                {
                    var updatedErrors = await _mediatr.Send(new CreatedServiceErrorLogCommand()
                    { Errors = result.Errors });
                    if (updatedErrors.Any())
                    {
                        updatedErrors.ForEach(async x =>
                        {
                            await _mediatr.Send(new UpdatedServiceErrorLogCommand()
                            { Id = x.Id, IsCompleted = x.IsCompleted });
                        });
                    }
                }
            }
            return View();
        }

        [HttpGet("HealthCheck/GetHealthCheckByFilter")]
        public async Task<IActionResult> GetHealthCheckByFilter(HealthCheckByFilterViewModel healthCheckByFilter)
        {
            var result = await _mediatr.Send(new GetListHealthCheckByFilterQuery()
            {
                ServiceName = healthCheckByFilter.ServiceName,
                StartTime = healthCheckByFilter.StartTime,
                EndTime = healthCheckByFilter.EndTime
            });
            return View(result);
        }

        [HttpPost("HealthCheck/ChangeErrorLogStatus")]
        public async Task ChangeErrorLogStatus(ChangeErrorLogViewModel changeErrorLogViewModel)
        {
            await _mediatr.Send(new UpdatedServiceErrorLogCommand()
                { Id = changeErrorLogViewModel.Id, IsCompleted = changeErrorLogViewModel.IsCompleted });
        }

        [HttpDelete("HealthCheck/DeleteCompletedErrors")]
        public async Task DeleteCompletedErrors()
        {
            await _mediatr.Send(new DeletedServiceErrorLogCommand());
        }
    }
}
