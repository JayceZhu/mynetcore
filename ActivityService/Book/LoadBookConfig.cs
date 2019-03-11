using Command;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityService.Book
{
    public class LoadBookConfigParameter
    {
        public int ActId { get; set; }
    }

    public class LoadBookConfigCommand : Command<BookConfig>
    {
        protected override CommandResult<BookConfig> OnExecute(object commandParameter)
        {
            var result = new CommandResult<BookConfig>();
            var param = commandParameter as LoadBookConfigParameter;

            using (CoreContext context = new CoreContext())
            {
                var bookConfig = context.BookConfig.Where(b => b.Recid == param.ActId).FirstOrDefault();
                if (bookConfig == null)
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = "活动已结束";
                    return result;
                }
            }

            return result;

        }
    }
}
