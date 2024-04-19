using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Shared.Settings.Abstract
{
    public interface IMailSetting
    {
        public string ToMail { get; set; }
        public string FromMail { get; set; }
    }
}
