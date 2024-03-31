using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using ServicesHealthCheck.Business.CQRS.Features.Commands.ServiceHealthCheckCommands;
using ServicesHealthCheck.Business.Notifications.EMailService.Abstract;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.MailDtos;
using ServicesHealthCheck.Shared.Settings;

namespace ServicesHealthCheck.Business.CQRS.Features.Handlers.ServiceHealthCheckHandlers
{
    public class ServiceHealthCheckCommandHandler : IRequestHandler<CreatedServiceHealthCheckCommand>
    {
        private readonly IServiceHealthCheckRepository _serviceHealthCheckRepository;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly MailSetting _mailSetting;
        public ServiceHealthCheckCommandHandler(IServiceHealthCheckRepository serviceHealthCheckRepository, IMapper mapper, IOptions<MailSetting> mailSetting, IMailService mailService)
        {
            _serviceHealthCheckRepository = serviceHealthCheckRepository;
            _mapper = mapper;
            _mailSetting = mailSetting.Value;
            _mailService = mailService;
        }
        public async Task Handle(CreatedServiceHealthCheckCommand request, CancellationToken cancellationToken)
        {
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
                var serviceHealthCheck = new ServiceHealthCheck()
                    { ServiceName = serviceName, Status = service.Status.ToString() };
                await _serviceHealthCheckRepository.AddAsync(serviceHealthCheck);
                //check resource usage
            }
            //var serviceHealthCheck = _mapper.Map<ServiceHealthCheck>(request);
        }
    }
}
