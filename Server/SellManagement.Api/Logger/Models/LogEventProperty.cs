using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Logger.Models
{
    public class LogEventProperty
    {
        public static readonly string RequestId = "RequestId";
        public static readonly string LogType = "LogType";
        public static readonly string UserName = "UserName";
        public static readonly string SessionId = "SessionId";
        public static readonly string IpAddressV4 = "IpAddressV4";
        public static readonly string IpAddressV6 = "IpAddressV6";
        public static readonly string FunctionCode = "FunctionCode";
        public static readonly string ComputerName = "ComputerName";
        public static readonly string FunctionName = "FunctionName";
        public static readonly string OperationName = "OperationName";
        public static readonly string OperationTarget = "OperationTarget";
        public static readonly string ClassMethod = "ClassMethod";
        public static readonly string ServerName = "ServerName";
        public static readonly string ProcessResult = "ProcessResult";
        public static readonly string ProcessResultDetail = "ProcessResultDetail";
        public static readonly string ProcessTime = "ProcessTime";
        public static readonly string ProcessCount = "ProcessCount";
        public static readonly string ExceptionMessage = "ExceptionMessage";
        public static readonly string StackTrace = "StackTrace";

    }
}
