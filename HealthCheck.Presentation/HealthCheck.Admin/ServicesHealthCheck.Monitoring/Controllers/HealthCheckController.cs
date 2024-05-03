﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Queries;
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
            return View(result);
        }

        [HttpPost("HealthCheck/ChangeErrorLogStatus")]
        public async Task ChangeErrorLogStatus(ChangeErrorLogViewModel changeErrorLogViewModel)
        {
            await _mediatr.Send(new UpdatedServiceErrorLogCommand()
                { Id = changeErrorLogViewModel.Id, IsCompleted = changeErrorLogViewModel.IsCompleted });
        }
    }
}
