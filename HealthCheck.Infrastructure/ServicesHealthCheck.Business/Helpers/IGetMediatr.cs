using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.Helpers
{
    public interface IGetMediatr
    {
        IMediator GetMediator();
    }
}
