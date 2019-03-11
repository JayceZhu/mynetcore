using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using System;

namespace MbcService
{
    public class LoginBaseParameter
    {

    }
    public class LoginBaseResult
    {
        public string Token { get; set; }
    }
    public abstract class LoginBaseCommand<T> : Command<T> where T : LoginBaseResult, new()
    {
        protected override CommandResult<T> OnExecute(object commandParameter)
        {
            using (CoreContext coreContext = new CoreContext())
            {
                CommandResult<T> result = Login(coreContext, commandParameter as LoginBaseParameter);
                if (result.ErrorCode == 0)
                {
                    //更新登录数据
                    coreContext.Database.ExecuteSqlCommandAsync($"update member_info set log_times=ifnull(log_times,0)+1,LAST_LOG={DateTime.Now} where account_id={result.Data.Token}");

                }
                return result;
            }

        }

        protected abstract CommandResult<T> Login(CoreContext dbContext, LoginBaseParameter parameter);

    }
}
