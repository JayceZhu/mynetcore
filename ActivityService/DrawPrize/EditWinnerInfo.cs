using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.DrawPrize
{
    public class EditWinnerInfoParameter : MemberParameter
    {
        public int Id { get; set; }

        public string WinnerInfo { get; set; }
    }
    public class EditWinnerInfoCommand : Command<int>
    {
        protected override CommandResult<int> OnExecute(object commandParameter)
        {
            var result = new CommandResult<int>();
            var param = commandParameter as EditWinnerInfoParameter;
            using (CoreContext context = new CoreContext())
            {
                string acc = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                result.Data = context.Database.ExecuteSqlCommand("update prize_info set status=9,winner_info=@p2 where member_account=@p0 and recid=@p1 ", acc, param.Id, param.WinnerInfo);
            }

            return result;
        }
    }
}
