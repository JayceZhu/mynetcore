using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using MySql.Data.MySqlClient;
using PubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.DrawPrize
{
    public class LoadDrawCountParameter : MemberParameter
    {
        public string Kind { get; set; }

        public string OrderNo { get; set; }
    }
    public class LoadDrawCountCommand : Command<int>
    {
        protected override CommandResult<int> OnExecute(object commandParameter)
        {
            var result = new CommandResult<int>();
            var param = commandParameter as LoadDrawCountParameter;
            using (CoreContext context = new CoreContext())
            {
                var act = context.ActivityInfo.Where(a => a.StartTime <= DateTime.Now && a.EndTime >= DateTime.Now && a.Status == 1).FirstOrDefault();
                if (act == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }
                int count = 0;
                var zlopenid = "";
                zlopenid = context.MemberInfo.Where(m => m.AccountId == param.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                string memo = "";
                if (param.Kind == "login")
                {
                    if ((context.AddDrawCountLog.Where(l => l.CreateDate <= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") && l.CreateDate >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00") && l.MemberAccount == zlopenid && l.Kind == param.Kind && l.ActId == act.Recid).Count() == 0))
                    {
                        memo = "登录送1次抽奖机会";
                        count = 1;
                    }
                }
                else
                {
                    var constr = ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"];
                    using (MySqlConnection con = new MySqlConnection(constr))
                    {
                        con.Open();
                        string sql = @"select payment_fee,member_account from shop_order_info where PAY_STATUS=1 and order_no=?ono and PAYMENT_TIME>='2018-11-01'";
                        MySqlCommand com = new MySqlCommand(sql, con);
                        com.Parameters.Add(new MySqlParameter("?ono", param.OrderNo));

                        MySqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            zlopenid = reader["member_account"] as string;
                            count = Convert.ToInt32(reader["payment_fee"]) / 100;
                            memo = $"下单赠送次数,订单号:{param.OrderNo}";
                        }

                        con.Close();
                    }
                }
                if (count > 0)
                {
                    context.Database.ExecuteSqlCommand(@"insert into member_draw_count (MEMBER_ACCOUNT,ACT_ID,COUNTER,CURRENT_COUNT)  values(@p0,@p1,@p2,@p2) 
                                                            on  DUPLICATE key update COUNTER=COUNTER+@p2,CURRENT_COUNT=CURRENT_COUNT+@p2 ", zlopenid, act.Recid, count);

                    context.AddDrawCountLog.Add(new AddDrawCountLog()
                    {
                        MemberAccount = zlopenid,
                        Counter = 1,
                        CreateDate = DateTime.Now,
                        Memo = memo,
                        Kind = string.IsNullOrEmpty(param.Kind) ? "order" : param.Kind,
                        ActId = act.Recid
                    });
                }



                context.SaveChanges();

                result.Data = Convert.ToInt32(context.MemberDrawCount.Where(m => m.MemberAccount == zlopenid && m.ActId == act.Recid).Select(m => m.CurrentCount).FirstOrDefault());
            }

            return result;
        }
    }
}
