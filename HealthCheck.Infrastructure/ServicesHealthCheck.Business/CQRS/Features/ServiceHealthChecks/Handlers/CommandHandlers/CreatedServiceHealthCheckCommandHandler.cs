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
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;
using ServicesHealthCheck.Dtos.SignalRDtos;
using ServicesHealthCheck.Shared.Settings;
using ServicesHealthCheck.Shared.Settings.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Handlers.CommandHandlers
{
    public class CreatedServiceHealthCheckCommandHandler : IRequestHandler<CreatedServiceHealthCheckCommand, List<ServiceHealthCheckDto>>
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
        public async Task<List<ServiceHealthCheckDto>> Handle(CreatedServiceHealthCheckCommand request, CancellationToken cancellationToken)
        {
            List<ServiceHealthCheckDto> updateServiceHealthCheckDtos = new List<ServiceHealthCheckDto>();
            foreach (var serviceName in request.Services)
            {
                bool isHealthy = true;
                ServiceController service = new ServiceController(serviceName);
                Console.WriteLine("Servis durumu: " + service.Status);

                var IsExistServiceHealthCheck =
                    await _serviceHealthCheckRepository.GetByServiceNameAsync(serviceName); //servis'in veritabanında olup olmadığınının kontrolü

                if (service.Status != ServiceControllerStatus.Running) // servis çalışmıyorsa veya sağlıksız ise
                {
                    if (IsExistServiceHealthCheck != null && IsExistServiceHealthCheck.IsHealthy == true) // mevcut servis sağlıklı iken sağlıksız hale geldiyse(yeni bir durum oluşmuş, boşa mail göndermeyi engeller.)
                    {
                        foreach (var mail in _mailSetting.ToMail)
                        {
                            await _mailService.SendEmailAsync(new MailDto
                            {
                                FromMail = _mailSetting.FromMail,
                                ToEmail = mail,
                                Subject = "Servis Durumu",
                                Body = $"{serviceName} servisi çalışmıyor."
                            }, CancellationToken.None);
                            isHealthy = false; // durumunu sağlıksız hale getir
                        }
                    }
                    else if (IsExistServiceHealthCheck.IsHealthy == false) //mevcut servis'in durumu sağlıksız ise başta verilen sağlık durumunu false yap, mail gönderme(zaten önceden gönderilmiş)
                    {
                        isHealthy = false;
                    }
                }
                if (IsExistServiceHealthCheck != null) //mevcut servis var ise, yeni bir servis değilse, güncelleme yap
                {
                    var resourceModel = CheckResourceUsage(serviceName); // kaynak kullanımlarını al
                    var serviceHealthCheckDto = new ServiceHealthCheckDto()
                    {
                        ServiceName = serviceName,
                        Status = service.Status.ToString(),
                        IsHealthy = isHealthy,
                        CpuUsage = resourceModel.CpuUsage,
                        PhysicalMemoryUsage = resourceModel.PhysicalMemoryUsage,
                        PrivateMemoryUsage = resourceModel.PrivateMemoryUsage,
                        VirtualMemoryUsage = resourceModel.VirtualMemoryUsage
                    };
                    var serviceHealthCheckSignalRDto = _mapper.Map<ServicesHealthCheckSignalRDto>(serviceHealthCheckDto);
                    await _signalRService.SendMessageAsync(serviceHealthCheckSignalRDto); // servis ile ilgili bilgileri signalR'a gönder

                    updateServiceHealthCheckDtos.Add(serviceHealthCheckDto); // servis'i güncellemek için dto'ya ekle ve döndür
                }
                else //servis veritabanında yok, yeni bir servis,veritabanına ekle
                {
                    var resourceModel = CheckResourceUsage(serviceName);

                    var serviceHealthCheck = new ServiceHealthCheck()
                    {
                        ServiceName = serviceName,
                        Status = service.Status.ToString(),
                        IsHealthy = isHealthy,
                        CpuUsage = resourceModel.CpuUsage,
                        PhysicalMemoryUsage = resourceModel.PhysicalMemoryUsage,
                        PrivateMemoryUsage = resourceModel.PrivateMemoryUsage,
                        VirtualMemoryUsage = resourceModel.VirtualMemoryUsage
                    };
                    var serviceHealthCheckSignalRDto = _mapper.Map<ServicesHealthCheckSignalRDto>(serviceHealthCheck);
                    await _signalRService.SendMessageAsync(serviceHealthCheckSignalRDto); // servis ile ilgili bilgileri signalR'a gönder
                    await _serviceHealthCheckRepository.AddAsync(serviceHealthCheck);
                }
            }
            return updateServiceHealthCheckDtos;
        }
        private ResourceUsageModel CheckResourceUsage(string serviceName)
        {
            ManagementObject wmiService;
            wmiService = new ManagementObject("Win32_Service.Name='" + serviceName + "'");
            object o = wmiService.GetPropertyValue("ProcessId");

            int processId = (int)(uint)o;
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
