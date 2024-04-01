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
                    serviceHealthCheckDtos.Add(new ServiceHealthCheckDto()
                    { ServiceName = serviceName, Status = service.Status.ToString() });
                }
                else
                {
                    var serviceHealthCheck = new ServiceHealthCheck()
                    { ServiceName = serviceName, Status = service.Status.ToString() };
                    await _serviceHealthCheckRepository.AddAsync(serviceHealthCheck);
                }
                //check resource usage
            }
            return serviceHealthCheckDtos;
            //var serviceHealthCheck = _mapper.Map<ServiceHealthCheck>(request);
        }
    }
}
