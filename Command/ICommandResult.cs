using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    interface ICommandResult
    {
        int ErrorCode { get; set; }

        string ErrorMessage { get; set; }


    }
}
