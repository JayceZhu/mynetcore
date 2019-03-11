using Microsoft.AspNetCore.Http;

namespace PubService.Server
{
    public class HttpContextAccessor : IHttpContextAccessor
    {
        public HttpContextAccessor() { }

        public HttpContext HttpContext { get; set; }
    }
}