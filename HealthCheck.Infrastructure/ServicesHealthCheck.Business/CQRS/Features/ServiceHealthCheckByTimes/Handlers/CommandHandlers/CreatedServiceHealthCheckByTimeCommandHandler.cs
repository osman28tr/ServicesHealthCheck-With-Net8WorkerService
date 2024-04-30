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
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Models;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Models;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Handlers.CommandHandlers
{
    public class CreatedServiceHealthCheckByTimeCommandHandler : IRequestHandler<CreatedServiceHealthCheckByTimeCommand>
    {
        private readonly IServiceHealthCheckByTimeRepository _serviceHealthCheckByTimeRepository;
        private readonly IMapper _mapper;

        public CreatedServiceHealthCheckByTimeCommandHandler(
            IServiceHealthCheckByTimeRepository serviceHealthCheckByTimeRepository, IMapper mapper)
        {
            _serviceHealthCheckByTimeRepository = serviceHealthCheckByTimeRepository;
            _mapper = mapper;
        }

        public async Task Handle(CreatedServiceHealthCheckByTimeCommand request, CancellationToken cancellationToken)
        {
            foreach (var serviceName in request.Services)
            {
                try
                {
                    bool isHealthy = true;
                    bool isResourceUsageLimitExceeded = false;

                    ServiceController service = new ServiceController(serviceName);

                    Console.WriteLine($"{serviceName} is status in servicehealthcheckbytime: " + service.Status);

                    var resourceModel = CheckResourceUsage(serviceName);

                    if (service.Status != ServiceControllerStatus.Running)
                    {
                        isHealthy = false;
                    }

                    if (request.ServiceResourceUsageLimit.CpuMaxUsage < resourceModel.CpuUsage)
                    {
                        isHealthy = false;
                        isResourceUsageLimitExceeded = true;
                    }
                    var serviceHealthCheckByTime = _mapper.Map<ServiceHealthCheckByTime>(resourceModel);
                    serviceHealthCheckByTime.ServiceName = serviceName;
                    serviceHealthCheckByTime.IsHealthy = isHealthy;
                    serviceHealthCheckByTime.IsResourceUsageLimitExceeded = isResourceUsageLimitExceeded;
                    serviceHealthCheckByTime.Status = service.Status.ToString();

                    serviceHealthCheckByTime.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    await _serviceHealthCheckByTimeRepository.AddAsync(serviceHealthCheckByTime);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("An error occured in servicehealthcheckbytime." + exception);
                }
            }
        }

        private ResourceUsageModelByTime CheckResourceUsage(string serviceName)
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
            PerformanceCounter workingSetCounter =
                new PerformanceCounter("Process", "Working Set", process.ProcessName);
            PerformanceCounter privateBytesCounter =
                new PerformanceCounter("Process", "Private Bytes", process.ProcessName);

            PerformanceCounter virtualMemoryCounter =
                new PerformanceCounter("Process", "Virtual Bytes", process.ProcessName);

            //disk counter
            PerformanceCounter diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

            PerformanceCounter avgDiskQueueLengthCounter =
                new PerformanceCounter("PhysicalDisk", "Avg. Disk Queue Length", "_Total");
            // Gets current memory and CPU information

            float workingSet =
                workingSetCounter.NextValue() / (1024 * 1024); // Converts the value received in bytes to MB
            float privateBytes =
                privateBytesCounter.NextValue() / (1024 * 1024); // Converts the value received in bytes to MB

            float virtualMemorySize =
                virtualMemoryCounter.NextValue() / (1024 * 1024); // Converts the value received in bytes to MB

            float cpuUsage = cpuCounter.NextValue(); // Get cpu usage

            Thread.Sleep(1000);

            cpuUsage = cpuCounter.NextValue();

            float diskUsage = diskCounter.NextValue(); // Get disk usage

            var avgDiskQueueLength = avgDiskQueueLengthCounter.NextValue(); // Get average disk queue length
            ResourceUsageModelByTime resourceUsageModel = new ResourceUsageModelByTime()
            {
                CpuUsage = cpuUsage,
                PrivateMemoryUsage = privateBytes,
                VirtualMemoryUsage = virtualMemorySize,
                PhysicalMemoryUsage = workingSet,
                DiskUsage = diskUsage,
                AverageDiskQueueUsage = Convert.ToInt16(avgDiskQueueLength)
            };
            return resourceUsageModel;
        }
    }
}
