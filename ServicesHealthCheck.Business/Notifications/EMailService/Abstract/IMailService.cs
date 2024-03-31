using ServicesHealthCheck.Dtos.MailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.Notifications.EMailService.Abstract
{
    public interface IMailService
    {
        Task SendEmailAsync(MailDto mail, CancellationToken cancellationToken);
    }
}
