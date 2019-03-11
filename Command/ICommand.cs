using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    interface ICommand<T>
    {
        bool BeforeExecute(object commandParameter);
        CommandResult<T> Execute(object commandParameter);

        void AfterExecute(object commandParameter, CommandResult<T> commandResult);
    }
}
