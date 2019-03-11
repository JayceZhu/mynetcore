using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    public class OAuthListParameter : MemberParameter
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
