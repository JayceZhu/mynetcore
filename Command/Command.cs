using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    public abstract class Command<T> : ICommand<T>
    {
        public virtual void AfterExecute(object commandParameter, CommandResult<T> commandResult)
        {

        }


        public virtual bool BeforeExecute(object commandParameter)
        {
            return true;
        }

        protected abstract CommandResult<T> OnExecute(object commandParameter);

        public CommandResult<T> Execute(object commandParameter)
        {
            CommandResult<T> result = null;
            if (BeforeExecute(commandParameter))
            {
                try
                {

                    result = OnExecute(commandParameter);
                    Log.ForContext("Command", this).ForContext("Parameter", JsonConvert.SerializeObject(commandParameter)).Information(JsonConvert.SerializeObject(result));

                    if (result.ErrorCode == 0)
                    {

                        try
                        {
                            AfterExecute(commandParameter, result);
                        }
                        catch (Exception ex)
                        {
                            result = new CommandResult<T>
                            {
                                ErrorCode = -4,
                                ErrorMessage = ex.Message
                            };
                            Log.ForContext("Command", this).ForContext("Parameter", JsonConvert.SerializeObject(commandParameter)).Error(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = new CommandResult<T>
                    {
                        ErrorCode = -4,
                        ErrorMessage = ex.Message
                    };
                    Log.ForContext("Command", this).ForContext("Parameter", JsonConvert.SerializeObject(commandParameter)).Error(ex.Message);
                }

            }
            return result;
        }
    }
}
