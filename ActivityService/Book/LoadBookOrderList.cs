using Command;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using MySql.Data.MySqlClient;
using PubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Book
{
    public class LoadBookOrderListParameter : MemberParameter
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
    public class LoadBookOrderListCommand : Command<List<BookInfo>>
    {
        protected override CommandResult<List<BookInfo>> OnExecute(object commandParameter)
        {
            var param = commandParameter as LoadBookOrderListParameter;
            var result = new CommandResult<List<BookInfo>>();
            result.Data = new List<BookInfo>();
            using (CoreContext context = new CoreContext())
            {
                var bookList = context.BookInfo.FromSql($@"select b.* from book_info b left join pay_order p on p.order_no=b.order_no  where p.`STATUS`=1 and b.Member_Account={param.MemberAccount} order by b.status,b.Recid desc limit {(param.PageIndex - 1) * param.PageSize},{param.PageSize}").ToList();
                foreach (var item in bookList)
                {
                    using (MySqlConnection conn = new MySqlConnection(ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"]))
                    {
                        conn.Open();
                        MySqlCommand com = new MySqlCommand(@"select count(1) Counter from shop_order_info where order_no=?no and pay_status=1", conn);
                        com.Parameters.Add(new MySqlParameter("no", item.ShopOrderNo));

                        MySqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            if (Convert.ToInt32(reader["Counter"]) > 0)
                            {
                                item.Status = 9;
                            }
                        }
                        reader.Close();
                        conn.Close();
                    }
                    item.MemberAccount = null;
                }
                result.Data = bookList;
            }



            return result;
        }
    }
}
