using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Data;
using ProductService;

namespace sk.core.Controllers
{
    [Route("api/Product/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// 加载产品列表
        /// </summary>
        /// <param name="LoadProductListParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<IList<ProductInfo>> LoadProductList(LoadProductListParameter LoadProductListParameter)
        {
            return new LoadProductListComomand().Execute(LoadProductListParameter);
        }
    }
}
