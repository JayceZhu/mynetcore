using Command;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Model.Data;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace sk.core.Filters
{
    public class MemberParamterFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (StringValues.IsNullOrEmpty(context.HttpContext.Request.Headers["MemberToken"]))
            {
                context.Result = new ObjectResult(ErrorResult<int>.NoAuthorization);
                return;
            }
            var token = context.HttpContext.Request.Headers["MemberToken"].ToString();

            MemberParameter parameter = null;
            foreach (var arg in context.ActionArguments)
            {
                if (arg.Value is MemberParameter)
                {
                    parameter = arg.Value as MemberParameter;
                }
            }
            if (parameter == null)
            {
                context.Result = new ObjectResult(ErrorResult<int>.ParameterError);
                return;
            }

            using (CoreContext mcontext = new CoreContext())
            {
                ////寻找有效授权
                //PubService.LogUtil.Log("MemberParamterFilter", "MemberParamterFilter", token+"#"+ DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd hh:mm:ss"));
                parameter.MemberAccount = mcontext.MemberOuathCode.Where(m => m.OuathCode == token && m.CreateTime > DateTime.Now.AddMinutes(-30) && m.Status == 1)
                    .Select(m => m.MemberAccount).FirstOrDefault();
                if (string.IsNullOrEmpty(parameter.MemberAccount))
                {
                    ////更新失效状态
                    //PubService.LogUtil.Log("MemberParamterFilter", "update", $"update  member_ouath_code set status=-9 where ouath_code={token}");
                    mcontext.Database.ExecuteSqlCommand($"update  member_ouath_code set status=-9 where ouath_code={token}");
                    context.Result = new ObjectResult(ErrorResult<int>.NoLogin);
                    return;
                }
            }
            // parameter.MemberAccount = auth.Principal.FindFirstValue(ClaimTypes.Sid);

            await next();
        }
    }
}
