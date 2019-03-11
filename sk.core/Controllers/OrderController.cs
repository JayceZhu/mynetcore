using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.CommandData;
using OrderService;
using sk.core.Filters;

namespace sk.core.Controllers
{
    [Route("api/Order/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {


        /// <summary>
        /// 计算优惠
        /// </summary>
        /// <param name="DiscountParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<IList<DisocuntResult>> ComputeDiscount(DiscountParameter DiscountParameter)
        {
            return new ComputeDiscountCommand().Execute(DiscountParameter);
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<IList<PaymentData>> LoadPaymentIfo()
        {
            return new GetPaymentListCommand().Execute("");
        }
    }
}