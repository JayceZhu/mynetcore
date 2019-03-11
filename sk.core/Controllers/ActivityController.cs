using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityService;
using ActivityService.Book;
using ActivityService.DrawPrize;
using ActivityService.Pin;
using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.CommandData;
using Model.Data;
using Newtonsoft.Json.Linq;
using sk.core.Filters;

namespace sk.core.Controllers
{
    [Route("api/Activity/[action]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        /// <summary>
        /// 加载预定产品详情
        /// </summary>
        /// <param name="LoadBookProductDetailParameter"></param>
        /// <returns></returns>
        [HttpPost, CacheFilter]
        public CommandResult<ProductData> LoadBookProductDetail([FromBody] LoadBookProductDetailParameter LoadBookProductDetailParameter)
        {
            return new LoadBookProductDetailCommand().Execute(LoadBookProductDetailParameter);
        }

        /// <summary>
        /// 创建预付订单
        /// </summary>
        /// <param name="CreateBookOrderParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<CreateBookOrderResult> CreateBookOrder(CreateBookOrderParameter CreateBookOrderParameter)
        {
            return new CreateBookOrderCommand().Execute(CreateBookOrderParameter);
        }

        [HttpPost]
        public CommandResult<int> CreateShopOrder()
        {
            return new ActivityService.Book.CreateShopOrderCommand().Execute("");
        }

        /// <summary>
        /// 加载预付订单列表
        /// </summary>
        /// <param name="LoadBookOrderListParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter, CacheFilter]
        public CommandResult<List<BookInfo>> LoadBookOrderList(LoadBookOrderListParameter LoadBookOrderListParameter)
        {
            return new LoadBookOrderListCommand().Execute(LoadBookOrderListParameter);
        }
        /*--------------------------------拼团----------------------------*/
        /// <summary>
        /// 创建拼团订单
        /// </summary>
        /// <param name="CreatePinParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<string> CreatePinOrder(CreatePinParameter CreatePinParameter)
        {
            return new CreatePinOrderCommad().Execute(CreatePinParameter);
        }

        /// <summary>
        /// 加载拼团详情
        /// </summary>
        /// <param name="LoadPinDetailParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter, CacheFilter]
        public CommandResult<LoadPinDetailResult> LoadPinDetail(LoadPinDetailParameter LoadPinDetailParameter)
        {
            return new LoadPinDetailCommand().Execute(LoadPinDetailParameter);
        }

        /// <summary>
        /// 加载拼团列表
        /// </summary>
        /// <param name="LoadPinOrderParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter, CacheFilter]
        public CommandResult<System.Data.DataTable> LoadPinOrderList(LoadPinOrderParameter LoadPinOrderParameter)
        {
            return new LoadPinOrderCommand().Execute(LoadPinOrderParameter);
        }

        /// <summary>
        /// 修改拼团订单状态
        /// </summary>
        /// <param name="ChangePinOrderStausParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<int> ChangePinOrderStaus(ChangePinOrderStausParameter ChangePinOrderStausParameter)
        {
            return new ChangePinOrderStausCommand().Execute(ChangePinOrderStausParameter);
        }


        /// <summary>
        /// 计算优惠
        /// </summary>
        /// <param name="ComputePromotionParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<List<Hashtable>> ComputePromotion(ComputePromotionParameter ComputePromotionParameter)
        {
            return new ComputePromotionCommand().Execute(ComputePromotionParameter);
        }

        /// <summary>
        /// 加载拼团配置
        /// </summary>
        /// <param name="LoadPinConfigParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter, CacheFilter]
        public CommandResult<Dictionary<string, object>> LoadPinConfig(LoadPinConfigParameter LoadPinConfigParameter)
        {
            return new LoadPinConfigComand().Execute(LoadPinConfigParameter);
        }

        /// <summary>
        /// 加载可参加的团
        /// </summary>
        /// <param name="LoadPinGroupParameter"></param>
        /// <returns></returns>
        [HttpPost, CacheFilter]
        public CommandResult<List<Dictionary<string, object>>> LoadPinGroup(LoadPinGroupParameter LoadPinGroupParameter)
        {
            return new LoadPinGroupCommand().Execute(LoadPinGroupParameter);
        }

        /// <summary>
        /// 机器人拼团
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpGet]
        public CommandResult<int> RobotPin()
        {
            return new RobotPinCommand().Execute("");
        }


        /*---------抽奖-------*/

        /// <summary>
        /// 抽奖接口
        /// </summary>
        /// <param name="NewYearDrawPrizeParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<PrizeModel> DrawPrize(NewYearDrawPrizeParameter NewYearDrawPrizeParameter)
        {
            return new NewYearDrawPrizeCommand().Execute(NewYearDrawPrizeParameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AddDrawCountParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public CommandResult<int> AddDrawCountCommand(LoadDrawCountParameter AddDrawCountParameter)
        {
            return new LoadDrawCountCommand().Execute(AddDrawCountParameter);
        }

        /// <summary>
        /// 加载我的奖品
        /// </summary>
        /// <param name="LoadPrizeListParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<List<PrizeInfo>> LoadPrizeList(LoadPrizeListParameter LoadPrizeListParameter)
        {
            return new LoadPrizeListCommand().Execute(LoadPrizeListParameter);
        }

        /// <summary>
        /// 填写中奖信息
        /// </summary>
        /// <param name="EditWinnerInfoParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<int> EditWinnerInfo(EditWinnerInfoParameter EditWinnerInfoParameter)
        {
            return new EditWinnerInfoCommand().Execute(EditWinnerInfoParameter);
        }

        /// <summary>
        /// 获取活动奖品信息
        /// </summary>
        /// <param name="GetPrizeInfoParameter"></param>
        /// <returns></returns>
        [HttpPost, CacheFilter]
        public CommandResult<List<PrizeModel>> GetPrizeInfo(GetPrizeInfoParameter GetPrizeInfoParameter)
        {
            return new GetPrizeInfoCommand().Execute(GetPrizeInfoParameter);
        }

        /// <summary>
        /// 加载抽奖次数
        /// </summary>
        /// <param name="LoadDrawCountParameter"></param>
        /// <returns></returns>
        [HttpPost, MemberParamterFilter]
        public CommandResult<int> LoadDrawCount(LoadDrawCountParameter LoadDrawCountParameter)
        {
            return new LoadDrawCountCommand().Execute(LoadDrawCountParameter);
        }

        /// <summary>
        /// 添加抽奖次数
        /// </summary>
        /// <param name="LoadDrawCountParameter"></param>
        /// <returns></returns>
        [HttpPost, HttpGet]
        public CommandResult<int> GetDrawCount(LoadDrawCountParameter LoadDrawCountParameter)
        {
            return new LoadDrawCountCommand().Execute(LoadDrawCountParameter);
        }

        [HttpPost]
        public void SetPrize(List<int> Id)
        {
            var redisdb = PubService.RedisClient.GetDatabase(2);
            using (CoreContext context = new CoreContext())
            {
                var act = context.ActivityInfo.Where(a => Id.Contains(a.Recid) && a.Status == 1 && a.Kind == "SKILL").FirstOrDefault();
                if (act == null)
                {
                    return;
                }

                var pconfig = Newtonsoft.Json.JsonConvert.DeserializeObject<Hashtable>(act.ProductConfig);
                var prizeHash = new List<PrizeModel>();
                foreach (var item in (pconfig["PayPrize"] as Newtonsoft.Json.Linq.JArray).ToObject<List<Hashtable>>())
                {
                    //var Prize = item["Prize"] as Newtonsoft.Json.Linq.JObject;
                    //if (Prize["Kind"].Value<string>() == "RealPrize")
                    {
                        foreach (var hash in (item["Prize"] as Newtonsoft.Json.Linq.JArray).ToObject<List<Hashtable>>())
                        {
                            //if (DateTime.Parse(hash["start"] as string) <= DateTime.Now && DateTime.Parse(hash["end"] as string) >= DateTime.Now)
                            if (hash["Kind"] as string == "RealPrize")
                            {
                                for (int i = 0; i < Convert.ToInt32(hash["Counter"]); i++)
                                {
                                    hash["Kind"] = "RealPrize";
                                    redisdb.ListRightPush("Prize:" + hash["PrizeCode"] as string, Newtonsoft.Json.JsonConvert.SerializeObject(hash));
                                }
                            }

                        }

                    }

                }
            }
        }

        [HttpPost]
        public int AddDrawCount()
        {
            int data = 0;
            var constr = PubService.ConfigurationUtil.GetSection("ConnectionStrings")["ShopConnectString"];
            using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(constr))
            {
                con.Open();
                string sql = @"SELECT order_no,payment_fee,member_account from shop_order_info where PAY_STATUS=1 and PAYMENT_FEE>68 and order_no like '%00' and order_time >='2018-11-01'";
                MySql.Data.MySqlClient.MySqlCommand com = new MySql.Data.MySqlClient.MySqlCommand(sql, con);
                //com.Parameters.Add(new MySqlParameter("?ono", param.OrderNo));

                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    if (Convert.ToInt32(reader["payment_fee"]) >= 68)
                    {
                        string acc = reader["member_account"] as string;
                        int count = Convert.ToInt32(reader["payment_fee"]) / 68;
                        string memo = $"下单赠送次数,订单号:{reader["order_no"] as string}";
                        using (CoreContext context = new CoreContext())
                        {
                            if (context.AddDrawCountLog.Where(a => a.MemberAccount == acc && a.Memo.Contains(reader["order_no"] as string)).Count() < 1)
                            {
                                context.Database.ExecuteSqlCommand(@"insert into member_draw_count (MEMBER_ACCOUNT,ACT_ID,COUNTER,CURRENT_COUNT)  values(@p0,@p1,@p2,@p2) 
                                                            on  DUPLICATE key update COUNTER=COUNTER+@p2,CURRENT_COUNT=CURRENT_COUNT+@p2 ", acc, 2, count);

                                context.AddDrawCountLog.Add(new AddDrawCountLog()
                                {
                                    MemberAccount = acc,
                                    Counter = 1,
                                    CreateDate = DateTime.Now,
                                    Memo = memo,
                                    Kind = "order"
                                });
                                context.SaveChanges();
                                data++;
                            }

                        }
                    }

                }
                reader.Close();
                con.Close();
            }
            return data;
        }

        //        [HttpPost]
        //        public void AddMember()
        //        {
        //            using (Model.Data.CoreContext context = new Model.Data.CoreContext())
        //            {
        //                for (int i = 0; i < 30; i++)
        //                {
        //                    var filelist = new List<Model.CommandData.SimpleFileInfo>()
        //{
        //new Model.CommandData.SimpleFileInfo() {
        //FileExt="jpg",
        //FileId=-1,
        //FileName="test" + i,
        //FilePath=$"/pinimage/test{i}.jpg"
        //}};

        //                    context.MemberInfo.Add(new Model.Data.MemberInfo
        //                    {
        //                        AccountId = "test" + i,
        //                        NickName = "test" + i,
        //                        MemberName = "test" + i,
        //                        AddDate = DateTime.Now,
        //                        Score = 0,
        //                        Coins = 0,
        //                        Status = "1",
        //                        LastLog = DateTime.Now,
        //                        LogTimes = 1,
        //                        PhotoUrl = Newtonsoft.Json.JsonConvert.SerializeObject(filelist)
        //                    });
        //                    context.SaveChanges();
        //                }

        //            }
        //        }
    }
}