using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Models;
using ServicesHealthCheck.Business.Notifications.EMailService.Abstract;
using ServicesHealthCheck.Business.RealTimes.SignalR.Abstract;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.MailDtos;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;
using ServicesHealthCheck.Dtos.SignalRDtos;
using ServicesHealthCheck.Shared.Settings;
using ServicesHealthCheck.Shared.Settings.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Handlers.CommandHandlers
{
    public class CreatedServiceHealthCheckCommandHandler : IRequestHandler<CreatedServiceHealthCheckCommand, GeneralServiceHealthCheckDto>
    {
        private readonly IServiceHealthCheckRepository _serviceHealthCheckRepository;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IMailSetting _mailSetting;
        private readonly ISignalRService _signalRService;
        public CreatedServiceHealthCheckCommandHandler(IServiceHealthCheckRepository serviceHealthCheckRepository, IMapper mapper, IMailSetting mailSetting, IMailService mailService, ISignalRService signalRService)
        {
            _serviceHealthCheckRepository = serviceHealthCheckRepository;
            _mapper = mapper;
            _mailSetting = mailSetting;
            _mailService = mailService;
            _signalRService = signalRService;
        }
        public async Task<GeneralServiceHealthCheckDto> Handle(CreatedServiceHealthCheckCommand request, CancellationToken cancellationToken)
        {
            //List<ServiceHealthCheckDto> updateServiceHealthCheckDtos = new List<ServiceHealthCheckDto>();
            GeneralServiceHealthCheckDto generalServiceHealthCheckDto = new GeneralServiceHealthCheckDto();

            foreach (var serviceName in request.Services)
            {
                var serviceHealthCheckDto = new ServiceHealthCheckDto();
                try
                {
                    bool isHealthy = true;
                    bool isResourceUsageLimitExceeded = false;
                    ServiceController service = new ServiceController(serviceName);
                    
                    Console.WriteLine($"{serviceName} is status: " + service.Status);

                    var IsExistServiceHealthCheck =
                        await _serviceHealthCheckRepository.GetByServiceNameAsync(serviceName); //Checking whether the service is in the database

                    var resourceModel = CheckResourceUsage(serviceName);

                    if (service.Status != ServiceControllerStatus.Running) // If the service is not working or unhealthy
                    {
                        //service.Start();
                        isHealthy = false;
                        resourceModel.CpuUsage = 0;

                        generalServiceHealthCheckDto.Errors.Add(new CreatedServiceErrorLogDto()
                        {
                            ServiceName = serviceName,
                            ErrorMessage = $"{serviceName} is stopped.",
                            ErrorDate = DateTime.Now.AddHours(3),
                            IsCompleted = false
                        });

                        if (IsExistServiceHealthCheck != null && IsExistServiceHealthCheck.IsHealthy == true) // If the current service has become unhealthy while it was healthy (a new situation has occurred, it prevents sending unnecessary e-mails).
                        {
                            foreach (var mail in _mailSetting.ToMail)
                            {
                                await _mailService.SendEmailAsync(new MailDto
                                {
                                    FromMail = _mailSetting.FromMail,
                                    ToEmail = mail,
                                    Subject = "Servis Durumu",
                                    Body = $"{serviceName} servisi durduruldu, ardından tekrar çalıştırıldı."
                                }, CancellationToken.None);
                            }
                        }
                    }

                    if (resourceModel.CpuUsage >
                        request.ServiceResourceUsageLimit
                            .CpuMaxUsage) // If the service's CPU usage exceeds the limit, send an e-mail
                    {
                        isResourceUsageLimitExceeded = true;
                        isHealthy = false;

                        generalServiceHealthCheckDto.Errors.Add(new CreatedServiceErrorLogDto()
                        {
                            ServiceName = serviceName,
                            ErrorMessage = $"{serviceName} service's CPU usage limit has been exceeded.",
                            ErrorDate = DateTime.Now.AddHours(3),
                            IsCompleted = false
                        });

                        if (IsExistServiceHealthCheck.IsResourceUsageLimitExceeded == false)
                        {
                            foreach (var mail in _mailSetting.ToMail)
                            {
                                await _mailService.SendEmailAsync(new MailDto
                                {
                                    FromMail = _mailSetting.FromMail,
                                    ToEmail = mail,
                                    Subject = "Servis Durumu",
                                    Body = $"{serviceName} servisinin CPU kullanım limiti aşıldı."
                                }, CancellationToken.None);
                            }
                        }
                    }
                    if (IsExistServiceHealthCheck != null) //If there is an existing service, if it is not a new service, update it
                    {
                        // get resource usages
                        serviceHealthCheckDto.ServiceName = serviceName;
                        serviceHealthCheckDto.Status = service.Status.ToString();
                        serviceHealthCheckDto.IsHealthy = isHealthy;
                        serviceHealthCheckDto.CpuUsage = resourceModel.CpuUsage;
                        serviceHealthCheckDto.PhysicalMemoryUsage = resourceModel.PhysicalMemoryUsage;
                        serviceHealthCheckDto.PrivateMemoryUsage = resourceModel.PrivateMemoryUsage;
                        serviceHealthCheckDto.VirtualMemoryUsage = resourceModel.VirtualMemoryUsage;
                        serviceHealthCheckDto.IsResourceUsageLimitExceeded = isResourceUsageLimitExceeded;

                        var serviceHealthCheckSignalRDto = _mapper.Map<ServicesHealthCheckSignalRDto>(serviceHealthCheckDto);
                        serviceHealthCheckSignalRDto.DiskUsage = resourceModel.DiskUsage;
                        var serviceResourceUsageVisualizationSignalRDto = _mapper.Map<ServiceResourceUsageVisualizationSignalRDto>(serviceHealthCheckSignalRDto);

                        await _signalRService.SendMessageAsync(serviceHealthCheckSignalRDto); // Send service related information to signalR
                        await _signalRService.SendVisualizationMessageAsync(serviceResourceUsageVisualizationSignalRDto);
                        generalServiceHealthCheckDto.ServiceHealthCheckDtos.Add(serviceHealthCheckDto); // add to dto and rotate to update service
                    }
                    else //The service is not in the database, add a new service to the database
                    {
                        var serviceHealthCheck = new ServiceHealthCheck()
                        {
                            ServiceName = serviceName,
                            Status = service.Status.ToString(),
                            IsHealthy = isHealthy,
                            CpuUsage = resourceModel.CpuUsage,
                            PhysicalMemoryUsage = resourceModel.PhysicalMemoryUsage,
                            PrivateMemoryUsage = resourceModel.PrivateMemoryUsage,
                            VirtualMemoryUsage = resourceModel.VirtualMemoryUsage,
                            IsResourceUsageLimitExceeded = isResourceUsageLimitExceeded
                        };
                        var serviceHealthCheckSignalRDto = _mapper.Map<ServicesHealthCheckSignalRDto>(serviceHealthCheck);
                        serviceHealthCheckSignalRDto.DiskUsage = resourceModel.DiskUsage;
                        var serviceResourceUsageVisualizationSignalRDto = _mapper.Map<ServiceResourceUsageVisualizationSignalRDto>(serviceHealthCheckSignalRDto);

                        await _signalRService.SendMessageAsync(serviceHealthCheckSignalRDto); // Send service related information to signalR
                        await _signalRService.SendVisualizationMessageAsync(serviceResourceUsageVisualizationSignalRDto);
                        await _serviceHealthCheckRepository.AddAsync(serviceHealthCheck);
                    }
                }
                catch (Exception exception)
                {
                    // If you get a service not found error
                    if (exception.Message.Contains($"Service '{serviceName}' was not found on computer"))
                    {
                        Console.WriteLine($"An error occurred: {serviceName} is not found.");
                        var serviceHealthCheckSignalRDto = new ServicesHealthCheckSignalRDto()
                        {
                            ServiceName = "Undefined",
                            Status = "-",
                            IsHealthy = false,
                            CpuUsage = 0,
                            PhysicalMemoryUsage = 0,
                            PrivateMemoryUsage = 0,
                            VirtualMemoryUsage = 0
                        };
                        generalServiceHealthCheckDto.Errors.Add(new CreatedServiceErrorLogDto()
                        {
                            ServiceName = serviceName,
                            ErrorMessage = exception.Message,
                            ErrorDate = DateTime.Now.AddHours(3),
                            IsCompleted = false
                        });

                        await _signalRService.SendMessageAsync(serviceHealthCheckSignalRDto);

                        //generalServiceHealthCheckDto.Errors.Add(new CreatedServiceErrorLogDto()
                        //{
                        //    ServiceName = serviceName,
                        //    ErrorMessage = exception.Message,
                        //    ErrorDate = DateTime.Now,
                        //    IsCompleted = false
                        //});
                    }
                    else
                    {
                        //mongoya log atabilirsin, erroMessage prop'u olsun. Oraya mesajları atsın.
                        generalServiceHealthCheckDto.Errors.Add(new CreatedServiceErrorLogDto()
                        {
                            ServiceName = serviceName,
                            ErrorMessage = exception.Message,
                            ErrorDate = DateTime.Now.AddHours(3),
                            IsCompleted = false
                        });

                        Console.WriteLine("An error occurred: " + exception.Message);
                    }
                    continue;
                }
            }
            return generalServiceHealthCheckDto;
        }
        private ResourceUsageModel CheckResourceUsage(string serviceName)
        {
            ManagementObject wmiService;
            wmiService = new ManagementObject("Win32_Service.Name='" + serviceName + "'");
            object o = wmiService.GetPropertyValue("ProcessId");

            int processId = (int)(uint)o;
            Process process = Process.GetProcessById(processId);

            // Creating performance counters for CPU, Memory usage(gets in bytes)

            //Cpu counter
            PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);

            //Memory counters
            PerformanceCounter workingSetCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName);
            PerformanceCounter privateBytesCounter = new PerformanceCounter("Process", "Private Bytes", process.ProcessName);

            PerformanceCounter virtualMemoryCounter = new PerformanceCounter("Process", "Virtual Bytes", process.ProcessName);

            //disk counter
            PerformanceCounter diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

            // Gets current memory and CPU information
            
            float workingSet = workingSetCounter.NextValue() / (1024 * 1024); // Converts the value received in bytes to MB
            float privateBytes = privateBytesCounter.NextValue() / (1024 * 1024); // Converts the value received in bytes to MB

            float virtualMemorySize = virtualMemoryCounter.NextValue() / (1024 * 1024); // Converts the value received in bytes to MB

            float cpuUsage = cpuCounter.NextValue(); // Get cpu usage

            float diskUsage = diskCounter.NextValue(); // Get disk usage

            Thread.Sleep(1000);

            cpuUsage = cpuCounter.NextValue();

            diskUsage = diskCounter.NextValue();

            ResourceUsageModel resourceUsageModel = new ResourceUsageModel()
            {
                CpuUsage = cpuUsage,
                PrivateMemoryUsage = privateBytes,
                VirtualMemoryUsage = virtualMemorySize,
                PhysicalMemoryUsage = workingSet,
                DiskUsage = diskUsage,
            };
            return resourceUsageModel;
        }
    }
}
