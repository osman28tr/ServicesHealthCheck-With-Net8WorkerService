using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.Helpers
{
    public class GetMediatr : IGetMediatr
    {
        private readonly IMediator _mediator;

        public GetMediatr(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IMediator GetMediator()
        {
            return _mediator;
        }
    }
}
