using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    public class ErrorResult<T>
    {
        /// <summary>
        /// 参数错误
        /// </summary>
        public static CommandResult<T> ParameterError
        {
            get
            {
                return new CommandResult<T>()
                {
                    ErrorCode = -1,
                    ErrorMessage = "参数错误"
                };
            }
        }

        /// <summary>
        /// 用户未登录
        /// </summary>
        public static CommandResult<T> NoLogin
        {
            get
            {
                return new CommandResult<T>()
                {
                    ErrorCode = -2,
                    ErrorMessage = "用户未登录"
                };
            }
        }

        /// <summary>
        /// 用户未授权
        /// </summary>
        public static CommandResult<T> NoAuthorization
        {
            get
            {
                return new CommandResult<T>()
                {
                    ErrorCode = -3,
                    ErrorMessage = "用户未授权"
                };
            }
        }

        /// <summary>
        /// 用户未登录
        /// </summary>
        public static CommandResult<T> NoSignup
        {
            get
            {
                return new CommandResult<T>()
                {
                    ErrorCode = -4,
                    ErrorMessage = "用户未注册"
                };
            }
        }

        /// <summary>
        /// 找不到OpenId
        /// </summary>
        public static CommandResult<T> NoFundOpenId
        {
            get
            {
                return new CommandResult<T>()
                {
                    ErrorCode = -4,
                    ErrorMessage = "找不到OpenId"
                };
            }
        }

        public static CommandResult<T> Error(int errorCode, string errorMessage)
        {
            return new CommandResult<T>()
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }
    }
}
