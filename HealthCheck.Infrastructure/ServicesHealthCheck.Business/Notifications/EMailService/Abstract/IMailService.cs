using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Dtos.MailDtos;

namespace ServicesHealthCheck.Business.Notifications.EMailService.Abstract
{
    public interface IMailService
    {
        Task SendEmailAsync(MailDto mail, CancellationToken cancellationToken);
    }
}
