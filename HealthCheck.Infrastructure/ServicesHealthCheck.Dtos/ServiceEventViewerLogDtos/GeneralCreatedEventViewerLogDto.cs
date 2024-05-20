using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Dtos.ServiceRuleDtos;

namespace ServicesHealthCheck.Dtos.ServiceEventViewerLogDtos
{
    public class GeneralCreatedEventViewerLogDto
    {
        public List<UpdatedServiceRuleDto> UpdatedServiceRules { get; set; }
        public List<UpdatedServiceEventViewerLogDto> UpdatedServiceEventViewerLogDtos { get; set; }

        public GeneralCreatedEventViewerLogDto()
        {
            UpdatedServiceRules = new List<UpdatedServiceRuleDto>();
            UpdatedServiceEventViewerLogDtos = new List<UpdatedServiceEventViewerLogDto>();
        }
    }
}
