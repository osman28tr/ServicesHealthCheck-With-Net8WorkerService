using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.HealthChecks.Abstract
{
    public interface IHealthCheck
    {
        Task<List<Dictionary<string,string>>> CheckServicesHealth(List<string> services);
        void CheckResourceUsage(string serviceName);
    }
}
