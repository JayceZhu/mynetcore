using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using PubService;
using PubService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Pin
{
    public class ChangePinOrderStausParameter
    {
        public string OrderNo { get; set; }
    }
    public class ChangePinOrderStausCommand : Command<int>
    {
        protected override CommandResult<int> OnExecute(object commandParameter)
        {
            var result = new CommandResult<int>();
            var param = commandParameter as ChangePinOrderStausParameter;
            using (CoreContext context = new CoreContext())
            {
                var pinOrder = context.PinOrder.Where(p => p.OrderNo == param.OrderNo && p.Status == 0).FirstOrDefault();
                if (pinOrder != null)
                {
                    using (MySqlConnection conn = new MySqlConnection(ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"]))
                    {
                        conn.Open();
                        MySqlCommand com = new MySqlCommand(@"select count(1) Counter from shop_order_info where order_no=?no and pay_status=1", conn);
                        com.Parameters.Add(new MySqlParameter("no", param.OrderNo));

                        MySqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            if (Convert.ToInt32(reader["Counter"]) == 0)
                            {
                                result.ErrorCode = -1;
                                result.ErrorMessage = $"订单{param.OrderNo}未支付";
                                return result;
                            }
                        }
                        reader.Close();
                        conn.Close();
                    }
                    var pinInfo = context.PinInfo.Where(p => p.Recid == pinOrder.MainId).FirstOrDefault();
                    result.Data = context.Database.ExecuteSqlCommand("update pin_order set status=1 where order_no=@p0", param.OrderNo);
                    List<string> OrderNo = new List<string>();
                    OrderNo = context.PinOrder.Where(p => p.MainId == pinInfo.Recid && p.Status == 1).Select(p => p.OrderNo).ToList();
                    //如果拼团人数满足则设置拼团成功
                    if (OrderNo.Count >= pinInfo.MinCount)
                    {
                        new SetPinInfoSuccessCommand().Execute(new SetPinInfoSuccessParameter { OrderNo = OrderNo, MainId = pinInfo.Recid });

                    }

                }

            }
            return result;
        }
    }
}
