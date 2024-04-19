using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Shared.Settings.Abstract;

namespace ServicesHealthCheck.Shared.Settings
{
    public class MailSetting : IMailSetting
    {
        public List<string> ToMail { get; set; }
        public string FromMail { get; set; }
    }
}
