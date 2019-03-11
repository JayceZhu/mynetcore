using Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService
{
    public class LoadProductDetailParameter
    {
        public string ProductNo { get; set; }
    }
    public class LoadProductDetailResult
    {

    }
    public class LoadProductDetailCommand : Command<LoadProductDetailResult>
    {
        protected override CommandResult<LoadProductDetailResult> OnExecute(object commandParameter)
        {
            throw new NotImplementedException();
        }
    }
}
