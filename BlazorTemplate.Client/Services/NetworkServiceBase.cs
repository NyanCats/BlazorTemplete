using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Client.Services
{
    public abstract class NetworkServiceBase
    {
        public abstract string EndPointUri { get; }
    }
}
