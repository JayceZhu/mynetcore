using Command;
using Model.CommandData;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrderService
{
    public class GetPaymentListCommand : Command<IList<PaymentData>>
    {
        protected override CommandResult<IList<PaymentData>> OnExecute(object commandParameter)
        {
            var result = new CommandResult<IList<PaymentData>>();
            using (CoreContext context = new CoreContext())
            {
                result.Data = context.PaymentConfig.Where(p => p.Status == "1").Select(p => new PaymentData()
                {
                    Id = p.Id,
                    PayUrl = p.PayUrl,
                    Title = p.PaymentName
                }).ToList();
            }

            return result;
        }
    }
}
