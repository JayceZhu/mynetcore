using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubService.Server
{
    public interface IHttpRequestServer
    {
        string AbsoluteUri{ get;}

        string RootURI { get; }
        

        string RawUrl { get; }
    }
}
