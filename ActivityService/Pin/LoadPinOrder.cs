using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ActivityService.Pin
{
    public class LoadPinOrderParameter : OAuthListParameter
    {

    }

    public class LoadPinOrderCommand : Command<DataTable>
    {
        protected override CommandResult<DataTable> OnExecute(object commandParameter)
        {
            var result = new CommandResult<DataTable>();
            var param = commandParameter as LoadPinOrderParameter;
            using (CoreContext context = new CoreContext())
            {
                var acc = param.MemberAccount;
                var conn = context.Database.GetDbConnection();
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $@"select p.Recid, p.MIN_COUNT,p.CONFIG,p.MAX_DATE,p.`STATUS`,o.CREATE_DATE,o.ORDER_NO from pin_order o left join pin_info p on o.MAIN_ID=p.RECID 
                              where o.`STATUS`=1 and p.`STATUS`!=0 and o.MEMBER_ACCOUNT=@p0 ORDER BY p.recid desc limit {param.PageSize * (param.PageIndex - 1)},{param.PageSize}";
                cmd.Parameters.Add(new MySqlParameter("@p0", acc));
                var reader = cmd.ExecuteReader();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(reader);
                reader.Close();
                conn.Close();
                result.Data = dt;
                //result.Data = context.PinOrder.FromSql(@"select  p.END_DATE,p.MIN_COUNT,p.CONFIG,p.MAX_DATE,p.`STATUS`,o.CREATE_DATE from pin_order o left join pin_info p on o.MAIN_ID=p.RECID 
                //              where o.`STATUS`=1 and p.`STATUS`!=0 and o.MEMBER_ACCOUNT=@p0", param.MemberAccount).ToList();
            }
            return result;
        }
    }
}