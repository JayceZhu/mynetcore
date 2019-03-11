using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubService.Server
{
    public class HttpRequestServer: IHttpRequestServer
    {
        public HttpRequestServer(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
            _request = _accessor.HttpContext.Request;
        }

        private HttpRequest _request;
        private IHttpContextAccessor _accessor;
        public string AbsoluteUri
        {
            get
            {
                var schem = !StringValues.IsNullOrEmpty(_request.Headers["X-Client-Scheme"]) ? _request.Headers["X-Client-Scheme"].ToString() : _request.Scheme;
                return new StringBuilder()
                            .Append(schem)
                            .Append("://")
                            .Append(_request.Host)
                            .Append(_request.PathBase)
                            .Append(_request.Path)
                            .Append(_request.QueryString)
                            .ToString();
            }
        }

        public string RootURI
        {
            get
            {
                var schem = !StringValues.IsNullOrEmpty(_request.Headers["X-Client-Scheme"]) ? _request.Headers["X-Client-Scheme"].ToString() : _request.Scheme;
                return new StringBuilder()
                  .Append(schem)
                  .Append("://")
                  .Append(_request.Host)
                  .ToString();
            }
        }

        public string RawUrl
        {
            get
            {
                return new StringBuilder()
                    .Append(_request.PathBase)
                    .Append(_request.Path)
                    .Append(_request.QueryString)
                    .ToString();
            }
        }
    }
}
