using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Results
{
    public class GeneralGetListServiceErrorLogQueryResult
    {
        public GeneralGetListServiceErrorLogQueryResult()
        {
            ErrorList = new List<GetListServiceErrorLogQueryResult>();
            Errors = new List<CreatedServiceErrorLogDto>();
        }
        public List<GetListServiceErrorLogQueryResult> ErrorList { get; set; }
        public List<CreatedServiceErrorLogDto> Errors { get; set; }
    }
}
