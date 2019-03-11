using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using PubService.ActiveMq;
using PubService.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Pin
{
    public class SetPinInfoSuccessParameter
    {
        public List<string> OrderNo { get; set; }
        public int MainId { get; set; }

    }
    public class SetPinInfoSuccessCommand : Command<int>
    {
        protected override CommandResult<int> OnExecute(object commandParameter)
        {
            var result = new CommandResult<int>();
            var param = commandParameter as SetPinInfoSuccessParameter;
            using (CoreContext context = new CoreContext())
            {
                //设置拼团成功
                result.Data = context.Database.ExecuteSqlCommand("update pin_info set status=9 where recid=@p0 and status!=9 ", param.MainId);
                if (result.Data > 0)
                {
                    foreach (var item in param.OrderNo)
                    {
                        try
                        {
                            var order = context.PinOrder.Where(p => p.OrderNo == item).FirstOrDefault();

                            var zlopenid = context.MemberInfo.Where(m => m.AccountId == order.MemberAccount).Select(m => m.ZlOpenId).FirstOrDefault();
                            var wxopneid = "";
                            using (MySqlConnection conn = new MySqlConnection(ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"]))
                            {
                                conn.Open();
                                MySqlCommand com = new MySqlCommand(@"select wx_open_id  from member_info where account_id=?acc ", conn);
                                com.Parameters.Add(new MySqlParameter("acc", zlopenid));

                                MySqlDataReader reader = com.ExecuteReader();
                                while (reader.Read())
                                {
                                    wxopneid = reader["wx_open_id"] as string;
                                }
                                reader.Close();
                                conn.Close();
                            }
                            if (!string.IsNullOrEmpty(wxopneid))
                            {
                                ActiveMQMessagePusher.Push("Message", new Dictionary<string, string>
                                        {
                                             { "MessageKey","PinSuccess"}
                                        }, new
                                        {
                                            WxOpenId = wxopneid,
                                            OrderNo = item,
                                            Product = JsonConvert.DeserializeObject<Hashtable>(order.ProductConfig)["ProductName"] as string
                                        });
                            }

                        }
                        catch (Exception ex)
                        {
                            LogUtil.LogText("sendmessage:MessageKey:PinSuccess", item, ex.Message);
                        }
                    }


                    var confirmRes = ZlanAPICaller.ExecuteSys("Sys.ChangeOrderConfirm", new { param.OrderNo, Type = "SUCCESS" });
                    if (!confirmRes["ErrorCode"].Value<string>().Equals("0000"))
                    {
                        LogUtil.Log("CreatePinOrder", param.MainId.ToString(), confirmRes["ErrorMsg"].Value<string>());

                        result.ErrorCode = -1;
                        result.ErrorMessage = "解锁商城订单失败";
                        return result;
                    }

                }

            }
            return result;
        }
    }
}
