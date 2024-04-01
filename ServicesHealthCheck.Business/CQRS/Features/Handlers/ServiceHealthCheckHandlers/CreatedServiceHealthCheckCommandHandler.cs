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
using ServicesHealthCheck.Business.CQRS.Features.Commands.ServiceHealthCheckCommands;
using ServicesHealthCheck.Business.CQRS.Features.Models.ServiceHealthCheckModels;
using ServicesHealthCheck.Business.Notifications.EMailService.Abstract;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.MailDtos;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;
using ServicesHealthCheck.Shared.Settings;

namespace ServicesHealthCheck.Business.CQRS.Features.Handlers.ServiceHealthCheckHandlers
{
    public class CreatedServiceHealthCheckCommandHandler : IRequestHandler<CreatedServiceHealthCheckCommand, List<ServiceHealthCheckDto>>
    {
        private readonly IServiceHealthCheckRepository _serviceHealthCheckRepository;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly MailSetting _mailSetting;
        public CreatedServiceHealthCheckCommandHandler(IServiceHealthCheckRepository serviceHealthCheckRepository, IMapper mapper, IOptions<MailSetting> mailSetting, IMailService mailService)
        {
            _serviceHealthCheckRepository = serviceHealthCheckRepository;
            _mapper = mapper;
            _mailSetting = mailSetting.Value;
            _mailService = mailService;
        }
        public async Task<List<ServiceHealthCheckDto>> Handle(CreatedServiceHealthCheckCommand request, CancellationToken cancellationToken)
        {
            List<ServiceHealthCheckDto> serviceHealthCheckDtos = new List<ServiceHealthCheckDto>();
            foreach (var serviceName in request.Services)
            {
                ServiceController service = new ServiceController(serviceName);
                Console.WriteLine("Servis durumu: " + service.Status);
                if (service.Status != ServiceControllerStatus.Running)
                {
                    await _mailService.SendEmailAsync(new MailDto
                    {
                        FromMail = _mailSetting.FromMail,
                        ToEmail = _mailSetting.ToMail,
                        Subject = "Servis Durumu",
                        Body = $"{serviceName} servisi çalışmıyor."
                    }, CancellationToken.None);
                }
                var IsExistServiceHealthCheck =
                    await _serviceHealthCheckRepository.GetByServiceNameAsync(serviceName);
                if (IsExistServiceHealthCheck != null)
                {
                    var resourceModel = CheckResourceUsage(serviceName);
                    serviceHealthCheckDtos.Add(new ServiceHealthCheckDto()
                    {
                        ServiceName = serviceName,
                        Status = service.Status.ToString(),
                        CpuUsage = resourceModel.CpuUsage,
                        PhysicalMemoryUsage = resourceModel.PhysicalMemoryUsage,
                        PrivateMemoryUsage = resourceModel.PrivateMemoryUsage,
                        VirtualMemoryUsage = resourceModel.VirtualMemoryUsage
                    });
                }
                else
                {
                    var resourceModel = CheckResourceUsage(serviceName);
                    var serviceHealthCheck = new ServiceHealthCheck()
                    {
                        ServiceName = serviceName,
                        Status = service.Status.ToString(),
                        CpuUsage = resourceModel.CpuUsage,
                        PhysicalMemoryUsage = resourceModel.PhysicalMemoryUsage,
                        PrivateMemoryUsage = resourceModel.PrivateMemoryUsage,
                        VirtualMemoryUsage = resourceModel.VirtualMemoryUsage
                    };
                    await _serviceHealthCheckRepository.AddAsync(serviceHealthCheck);
                }
                //check resource usage
            }
            return serviceHealthCheckDtos;
            //var serviceHealthCheck = _mapper.Map<ServiceHealthCheck>(request);
        }
        private ResourceUsageModel CheckResourceUsage(string serviceName)
        {
            ManagementObject wmiService;
            wmiService = new ManagementObject("Win32_Service.Name='" + serviceName + "'");
            object o = wmiService.GetPropertyValue("ProcessId");

            int processId = (int)((UInt32)o);
            Process process = Process.GetProcessById(processId);

            PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
            PerformanceCounter workingSetCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName);
            PerformanceCounter privateBytesCounter = new PerformanceCounter("Process", "Private Bytes", process.ProcessName);

            // Sanal bellek boyutu için performans sayaçlarını oluştur
            PerformanceCounter virtualMemoryCounter = new PerformanceCounter("Process", "Virtual Bytes", process.ProcessName);

            // Saniyede bir toplam CPU kullanımını al ve yazdır

            double workingSet = workingSetCounter.NextValue() / (1024 * 1024); // Byte cinsinden alınan değeri MB'ye çevir
            float privateBytes = privateBytesCounter.NextValue() / (1024 * 1024); // Byte cinsinden alınan değeri MB'ye çevir
            float cpuUsage = cpuCounter.NextValue(); // CPU kullanımını al
                                                     // Sanal bellek boyutunu al
            float virtualMemorySize = virtualMemoryCounter.NextValue() / (1024 * 1024); // Byte cinsinden alınan değeri MB'ye çevir
            Console.WriteLine(workingSet);
            ResourceUsageModel resourceUsageModel = new ResourceUsageModel()
            {
                CpuUsage = cpuUsage + "%",
                PrivateMemoryUsage = privateBytes.ToString() + "MB",
                VirtualMemoryUsage = virtualMemorySize.ToString() + "MB",
                PhysicalMemoryUsage = workingSet.ToString() + "MB"
            };
            return resourceUsageModel;
        }
    }
}
