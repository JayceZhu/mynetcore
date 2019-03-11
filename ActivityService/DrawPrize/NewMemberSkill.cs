using Command;
using Model.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PubService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivityService.DrawPrize
{
    public class NewMemberSkillParameter
    {
        public string MemberAccount { get; set; }

        public int LogCount { get; set; }

        public Hashtable Prize { get; set; }
    }


    public class NewMemberSkillCommand : Command<Hashtable>
    {
        protected static string NewMember = "NewMember";
        protected override CommandResult<Hashtable> OnExecute(object commandParameter)
        {
            var result = new CommandResult<Hashtable>();
            result.Data = new Hashtable();
            var param = commandParameter as NewMemberSkillParameter;
            string newSql = @"select count(1) Counter from shop_order_info  where MEMBER_ACCOUNT =?acc and pay_status=1";

            string _30Sql = @"SELECT count(1) Counter from shop_order_info  where  date(ORDER_TIME)<'2018-11-01' and date(ORDER_TIME)>='2018-10-01' and pay_status=1 and MEMBER_ACCOUNT = ?acc  ";

            var connect = ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"];
            bool isNew = true, is30 = true;
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand com1 = new MySqlCommand(newSql, con);
                com1.Parameters.Add(new MySqlParameter("?acc", param.MemberAccount));
                var Reader1 = com1.ExecuteReader();
                while (Reader1.Read())
                {
                    if (Convert.ToInt32(Reader1["Counter"]) > 0)
                    {
                        isNew = false;
                    }
                }
                Reader1.Close();

                if (!isNew)
                {
                    MySqlCommand com2 = new MySqlCommand(_30Sql, con);
                    com1.Parameters.Add(new MySqlParameter("?acc", param.MemberAccount));
                    var Reader2 = com1.ExecuteReader();
                    while (Reader2.Read())
                    {
                        if (Convert.ToInt32(Reader2["Counter"]) > 0)
                        {
                            is30 = false;
                            isNew = false;
                        }
                    }
                    Reader2.Close();
                }

                con.Close();
            }

            result.Data["isNew"] = isNew;
            result.Data["is30"] = is30;
            Hashtable prize = null;
            param.LogCount = param.LogCount + 1;
            if (isNew)
            {
                var newmemberprize = (param.Prize["NewMemberPrize"] as JArray).ToObject<List<Hashtable>>();
                foreach (var item in newmemberprize)
                {
                    JObject condiction = item["Condition"] as JObject;
                    if (param.LogCount == condiction["Counter"].Value<int>())
                    {
                        prize = (item["Prize"] as JObject).ToObject<Hashtable>();
                        break;
                    }
                }
            }
            if (is30)
            {
                var normalprize = (param.Prize["NormalPrize"] as JArray).ToObject<List<Hashtable>>();
                foreach (var item in normalprize)
                {
                    JObject condiction = item["Condition"] as JObject;
                    if (param.LogCount == condiction["Counter"].Value<int>())
                    {
                        prize = (item["Prize"] as JObject).ToObject<Hashtable>();
                        break;
                    }
                }
            }

            if (prize != null)
            {
                result.Data["Prize"] = prize;
            }

            return result;
        }
    }
}