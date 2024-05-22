using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Results;
using ServicesHealthCheck.DataAccess.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Handlers.QueryHandlers
{
    public class GetListHealthCheckByFilterQueryHandler : IRequestHandler<GetListHealthCheckByFilterQuery, List<GetListHealthCheckByFilterQueryResult>>
    {
        private readonly IServiceHealthCheckByTimeRepository _serviceHealthCheckByTimeRepository;
        private readonly IMapper _mapper;
        public GetListHealthCheckByFilterQueryHandler(IServiceHealthCheckByTimeRepository serviceHealthCheckByTimeRepository, IMapper mapper)
        {
            _serviceHealthCheckByTimeRepository = serviceHealthCheckByTimeRepository;
            _mapper = mapper;
        }

        public async Task<List<GetListHealthCheckByFilterQueryResult>> Handle(GetListHealthCheckByFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ServiceName == null)
                {
                    request.StartTime = request.StartTime.ToUniversalTime();
                    if (request.EndTime == DateTime.MinValue)
                        request.EndTime = DateTime.MaxValue.ToUniversalTime();
                    else
                        request.EndTime = request.EndTime.ToUniversalTime();

                    var serviceHealthCheckListByFilterTime =
                        await _serviceHealthCheckByTimeRepository.FindAsync(x =>
                            x.Date >= request.StartTime && x.Date <= request.EndTime);
                    if (serviceHealthCheckListByFilterTime.Count()==0)
                        return null;
                    var resultByTime = _mapper.Map<List<GetListHealthCheckByFilterQueryResult>>(serviceHealthCheckListByFilterTime);
                    return resultByTime;
                }
                else
                {
                    request.StartTime = request.StartTime.ToUniversalTime();
                    if (request.EndTime == DateTime.MinValue)
                        request.EndTime = DateTime.MaxValue.ToUniversalTime();
                    else
                        request.EndTime = request.EndTime.ToUniversalTime();

                    var serviceHealthCheckListByFilter = await _serviceHealthCheckByTimeRepository.FindAsync(x =>
                        x.ServiceName == request.ServiceName && (x.Date >= request.StartTime && x.Date <= request.EndTime));
                    if (serviceHealthCheckListByFilter.Count()==0)
                        return null;
                    var result = _mapper.Map<List<GetListHealthCheckByFilterQueryResult>>(serviceHealthCheckListByFilter);
                    return result;
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception.StackTrace + " " + exception.Message);
                return null;
            }
        }
    }
}
