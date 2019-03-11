using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    public class CommandResult<T> : ICommandResult
    {
        public CommandResult()
        {
            ErrorCode = 0;
            ErrorMessage = "ok";
        }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
