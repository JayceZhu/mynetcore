using Command;
using Model.CommandData;
using Model.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.DrawPrize
{
    public class GetPrizeInfoParameter
    {
        public string Kind { get; set; }
    }
    public class GetPrizeInfoCommand : Command<List<PrizeModel>>
    {
        protected override CommandResult<List<PrizeModel>> OnExecute(object commandParameter)
        {
            var result = new CommandResult<List<PrizeModel>>();
            var param = commandParameter as GetPrizeInfoParameter;
            result.Data = new List<PrizeModel>();
            using (CoreContext context = new CoreContext())
            {
                var act = context.ActivityInfo.Where(a => a.StartTime <= DateTime.Now && a.EndTime >= DateTime.Now && a.Status == 1 && a.Kind == param.Kind).FirstOrDefault();
                if (act == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }

                var pconfig = JsonConvert.DeserializeObject<Hashtable>(act.ProductConfig);
                var prizeHash = new List<PrizeModel>();
                foreach (var item in (pconfig["PayPrize"] as JArray).ToObject<List<Hashtable>>())
                {
                    //var Prize = item["Prize"] as JObject;
                    //if (Prize["Kind"].Value<string>() == "RealPrize")
                    {
                        foreach (var hash in (item["Prize"] as JArray).ToObject<List<Hashtable>>())
                        {
                            if (!hash.ContainsKey("end") || DateTime.Parse(hash["start"] as string) <= DateTime.Now && DateTime.Parse(hash["end"] as string) >= DateTime.Now)
                            {
                                prizeHash.Add(new PrizeModel()
                                {
                                    PrizeCode = hash["PrizeCode"] as string,
                                    PrizeName = hash["PrizeName"] as string,
                                    Sort = Convert.ToInt32(hash["Sort"]),
                                    Kind = hash["Kind"] as string
                                });
                            }
                        }

                    }
                    //else
                    //{
                    //    prizeHash.Add(Prize.ToObject<PrizeModel>());
                    //}
                }

                //foreach (var item in (pconfig["PayPrize"] as JArray).ToObject<List<Hashtable>>())
                //{
                //    var Prize = item["Prize"] as JObject;
                //    if (Prize["Kind"].Value<string>() == "RealPrize")
                //    {
                //        foreach (var hash in (Prize["RealPrize"] as JArray).ToObject<List<Hashtable>>())
                //        {
                //            if (DateTime.Parse(hash["start"] as string) <= DateTime.Now && DateTime.Parse(hash["end"] as string) >= DateTime.Now)
                //            {
                //                prizeHash.Add(new PrizeModel()
                //                {
                //                    PrizeCode = hash["PrizeCode"] as string,
                //                    PrizeName = hash["PrizeName"] as string,
                //                    Sort = Convert.ToInt32(hash["Sort"]),
                //                    Kind = "RealPrize"
                //                });
                //            }
                //        }

                //    }
                //    else
                //    {
                //        prizeHash.Add(Prize.ToObject<PrizeModel>());
                //    }
                //}

                //foreach (var item in (pconfig["NormalPrize"] as JArray).ToObject<List<Hashtable>>())
                //{

                //    prizeHash.Add((item["Prize"] as JObject).ToObject<PrizeModel>());
                //}

                //foreach (var item in (pconfig["NewMemberPrize"] as JArray).ToObject<List<Hashtable>>())
                //{

                //    prizeHash.Add((item["Prize"] as JObject).ToObject<PrizeModel>());
                //}

                result.Data = prizeHash.Where((x, i) => prizeHash.FindIndex(z => z.PrizeCode == x.PrizeCode) == i).OrderBy(z => z.Sort).ToList();

            }

            return result;
        }
    }
}
