using Command;
using MbcService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sk.core.Filters
{
    public class MbcAuthorizeFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (StringValues.IsNullOrEmpty(context.HttpContext.Request.Cookies["MemberToken"]))
            {
                context.Result = new ObjectResult(ErrorResult<int>.NoAuthorization);
            }
            var token = context.HttpContext.Request.Cookies["MemberToken"];

            var check = await CheckToken();
        }

        private Task<string> CheckToken()
        {
            return Task<string>.Factory.StartNew(() =>
            {
                throw new Exception(" Exception From Second!");
            });
        }

        //public  Task<string> CheckToken()
        //{
        //    using (CoreContext mcontext = new CoreContext())
        //    {
        //        //寻找有效授权
        //        //var MemberAccount = mcontext.MemberOuathCode.Where(m => m.OuathCode == token && m.CreateTime > DateTime.Now.AddMinutes(-30) && m.Status == 1)
        //        //     .Select(m => m.MemberAccount).FirstOrDefault();
        //        //if (string.IsNullOrEmpty(MemberAccount))
        //        //{
        //        //    //更新失效状态
        //        //    mcontext.Database.ExecuteSqlCommand($"update  member_ouath_code set status=-9 where ouath_code={token}");
        //        //    //context.Result = new ObjectResult(ErrorResult<int>.NoLogin);
        //        //    return false;
        //        //}

        //    }
        //    return "1";

        //}
    }
}
