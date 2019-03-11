using Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService
{
    public interface IValidator
    {
        void InitData(System.Collections.Hashtable hash);

        bool Validate(DiscountParameter parameter, CoreContext context);
    }
}
