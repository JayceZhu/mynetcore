// Copyright 2017 Zethian Inc.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using MySql.Data.MySqlClient;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Batch;
using Serilog.Sinks.Extensions;

namespace Serilog.Sinks.MySQL
{
    internal class MySqlSink : BatchProvider, ILogEventSink
    {
        public MySqlSink()
        {

        }

        public void Emit(LogEvent logEvent)
        {
            PushEvent(logEvent);
        }



        protected override async Task<bool> WriteLogEventAsync(ICollection<LogEvent> logEventsBatch)
        {
            try
            {
                using (CoreContext context = new CoreContext())
                {
                    var transaction = context.Database.BeginTransaction();
                    foreach (var logEvent in logEventsBatch)
                    {
                        object logger = "";
                        if (logEvent.Properties.ContainsKey("Logger"))
                        {
                            logger = (logEvent.Properties["Logger"] as ScalarValue).Value;
                        }
                        object moduleName = "";
                        if (logEvent.Properties.ContainsKey("ModuleName"))
                        {
                            moduleName = (logEvent.Properties["ModuleName"] as ScalarValue).Value;
                        }
                        await context.Database.ExecuteSqlCommandAsync($"INSERT INTO logs (Timestamp,ModuleName, Level, Message, Exception, Properties,Logger)  VALUES ({logEvent.Timestamp.DateTime}, {moduleName},{logEvent.Level.ToString()}, {logEvent.MessageTemplate.Text}, {logEvent.Exception}, {logEvent.Properties.Json()},{logger})");
                    }
                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine(ex.Message);
                return false;
            }
        }
    }
}