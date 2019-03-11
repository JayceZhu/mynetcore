using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.CommandData;
using PubService.Command;
using sk.core.Filters;

namespace sk.core.Controllers
{
    [Route("api/Pub/[action]")]
    [ApiController]
    public class PubController : Controller
    {
        /// <summary>
        /// 获取省市区
        /// </summary>
        /// <param name="LoadAddressParameter"></param>
        /// <returns></returns>
        [HttpPost, CacheFilter]
        public CommandResult<IList<ParamKeyValuePair>> LoadAddress([FromBody] LoadAddressParameter LoadAddressParameter)
        {
            return new LoadAddressCommand().Execute(LoadAddressParameter);
        }
    }
}