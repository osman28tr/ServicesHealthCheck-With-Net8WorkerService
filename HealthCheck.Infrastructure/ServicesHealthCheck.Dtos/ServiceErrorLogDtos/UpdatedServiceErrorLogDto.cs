using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Dtos.ServiceErrorLogDtos
{
    public class UpdatedServiceErrorLogDto
    {
        public string Id { get; set; }
        public bool IsCompleted { get; set; }
    }
}
