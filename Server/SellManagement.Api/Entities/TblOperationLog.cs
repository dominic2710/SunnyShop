using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Entities
{
    public class TblOperationLog
    {
        public string RequestId { get; set; }
        public string LogType { get; set; }
        public DateTime LogDate { get; set; }
        public string UserName { get; set; }
        public string SessionId { get; set; }
        public string IpAddressV4 { get; set; }
        public string IpAddressV6 { get; set; }
        public string FunctionCode { get; set; }
        public string ComputerName { get; set; }
        public string FunctionName { get; set; }
        public string OperationName { get; set; }
        public string OperationTarget { get; set; }
        public string ClassMethod { get; set; }
        public string ServerName { get; set; }
        public string ProcessResult { get; set; }
        public string ProcessResultDetail { get; set; }
        public string ProcessTime { get; set; }
        public string ProcessCount { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
