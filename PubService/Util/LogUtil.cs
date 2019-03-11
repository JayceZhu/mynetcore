using System;
using System.Collections.Generic;
using System.Text;

namespace PubService
{
    public class LogUtil
    {
        public static void Log(string modulename, string logger, string message)
        {
            Serilog.Log.ForContext("ModuleName", modulename).ForContext("Logger", logger)
                .Information(message);
        }

        public static void LogText(string command, string logger, string message)
        {
            Serilog.Log.ForContext("Command", command).ForContext("Logger", logger)
               .Information(message);
        }
    }
}
