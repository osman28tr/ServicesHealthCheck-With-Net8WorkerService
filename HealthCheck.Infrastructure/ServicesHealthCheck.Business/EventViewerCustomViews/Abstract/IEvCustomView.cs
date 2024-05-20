using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.EventViewerCustomViews.Abstract
{
    public interface IEvCustomView
    {
        Task CreateCustomViewAsync(List<string> services);
    }
}
