using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ServicesHealthCheck.Business.HealthChecks.Abstract;
using ServicesHealthCheck.Business.Notifications.EMailService.Abstract;
using ServicesHealthCheck.Dtos.MailDtos;
using ServicesHealthCheck.Shared.Settings;

namespace ServicesHealthCheck.Business.HealthChecks
{
    public class HealthCheck : IHealthCheck
    {
        private readonly IMailService _mailService;
        private readonly MailSetting _mailSetting;
        public HealthCheck(IMailService mailService,IOptions<MailSetting> mailOptions)
        {
            _mailService = mailService;
            _mailSetting = mailOptions.Value;
        }
        public async Task<List<Dictionary<string,string>>> CheckServicesHealth(List<string> services)
        {
            List<Dictionary<string,string>> stateServices = new List<Dictionary<string, string>>();
            foreach (var serviceName in services)
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
                    },CancellationToken.None);
                }
                Dictionary<string, string> serviceState = new Dictionary<string, string>();
                serviceState.Add(serviceName, service.Status.ToString());
                stateServices.Add(serviceState);
                //check resource usage
                CheckResourceUsage(serviceName);
            }
            return stateServices;
        }

        public void CheckResourceUsage(string serviceName)
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

            float workingSet = workingSetCounter.NextValue() / (1024 * 1024); // Byte cinsinden alınan değeri MB'ye çevir
            float privateBytes = privateBytesCounter.NextValue() / (1024 * 1024); // Byte cinsinden alınan değeri MB'ye çevir
            float cpuUsage = cpuCounter.NextValue(); // CPU kullanımını al
                                                     // Sanal bellek boyutunu al
            float virtualMemorySize = virtualMemoryCounter.NextValue() / (1024 * 1024); // Byte cinsinden alınan değeri MB'ye çevir

            Console.WriteLine("CPU Kullanımı: " + cpuUsage + "%");
            Console.WriteLine("Fiziksel olarak kullanılan ram miktarı: " + workingSet + " MB");
            Console.WriteLine("Sanal olarak kullanılan ram miktarı: " + virtualMemorySize + " MB");
            Console.WriteLine("Özel olarak kullanılan ram miktarı: " + privateBytes + " MB");
            //Console.WriteLine($"En yüksek fiziksel bellek kullanımı: {peakWorkingSet}" + " MB");
            //Console.WriteLine($"En yüksek sayfalanmış bellek kullanımı: {peakPagedMem}" + " MB");
            //Console.WriteLine($"En yüksek sanal bellek kullanımı: {peakVirtualMem}" + " MB");
            System.Threading.Thread.Sleep(1000);
        }
    }
}
