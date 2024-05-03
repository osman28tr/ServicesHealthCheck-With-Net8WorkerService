using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Dtos.ServiceErrorLogDtos
{
    public class CreatedServiceErrorLogDto
    {
        public string ServiceName { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ErrorDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
