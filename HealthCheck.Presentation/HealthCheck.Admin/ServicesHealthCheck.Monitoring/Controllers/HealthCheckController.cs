﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Queries;
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
            try
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
            }
            catch (Exception exception)
            {
                Log.Error("an error occured in get ErrorMessages actionresult" + exception.Message);
            }
            return View();
        }

        [HttpGet("HealthCheck/GetHealthCheckByFilter")]
        public async Task<IActionResult> GetHealthCheckByFilter(HealthCheckByFilterModel healthCheckByFilter)
        {
            try
            {
                var result = await _mediatr.Send(new GetListHealthCheckByFilterQuery()
                {
                    ServiceName = healthCheckByFilter.ServiceName,
                    StartTime = healthCheckByFilter.StartTime,
                    EndTime = healthCheckByFilter.EndTime
                });
                return View(result);
            }
            catch (Exception exception)
            {
                Log.Error("An error occured in GetHealthCheckByFilter actionresult" + exception.Message);
                return View();
            }
        }

        [HttpGet("HealthCheck/GetEventViewerLogsByFilter")]
        public async Task<IActionResult> GetEventViewerLogsByFilter(
            GetEventViewerLogByFilterModel eventViewerLogByFilter)
        {
            try
            {
                var result = await _mediatr.Send(new GetEventViewerLogByFilterQuery()
                {
                    ServiceName = eventViewerLogByFilter.ServiceName,
                    EventType = eventViewerLogByFilter.EventType,
                    EventStartDate = eventViewerLogByFilter.EventStartDate,
                    EventEndDate = eventViewerLogByFilter.EventEndDate
                });
                return View(result);
            }
            catch (Exception exception)
            {
                Log.Error("An error occured in GetEventViewerLogsByFilter actionresult" + exception.Message);
                return View();
            }
        }

        [HttpPost("HealthCheck/ChangeErrorLogStatus")]
        public async Task ChangeErrorLogStatus(ChangeErrorLogModel changeErrorLogViewModel)
        {
            try
            {
                await _mediatr.Send(new UpdatedServiceErrorLogCommand()
                    { Id = changeErrorLogViewModel.Id, IsCompleted = changeErrorLogViewModel.IsCompleted });
            }
            catch (Exception exception)
            {
                Log.Error("An error occured in ChangeErrorLogStatus actionresult" + exception.Message);
            }
        }

        [HttpDelete("HealthCheck/DeleteCompletedErrors")]
        public async Task DeleteCompletedErrors()
        {
            try
            {
                await _mediatr.Send(new DeletedServiceErrorLogCommand());
            }
            catch (Exception exception)
            {
                Log.Error("An error occured in DeleteCompletedErrors actionresult" + exception.Message);
            }
        }
    }
}
