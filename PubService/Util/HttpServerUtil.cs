using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubService
{
    public class HttpServerUtil
    {
        /// <summary>
        /// 获取当前url
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns>AbsoluteUri</returns>
        public static string GetAbsoluteUri(HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        public static string GetRootURI(HttpRequest request)
        {
            return new StringBuilder().Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .ToString();
        }

    }
}
