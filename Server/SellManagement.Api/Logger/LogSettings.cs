using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace SellManagement.Api.Logger
{
    public abstract class LogSettings
    {
        public abstract string GetLoggerName();

        public abstract string GetConsoleLayout();

        public abstract string GetFileLayout();

        public string GetCommandText() => "Insert Into TblOperationLogs ("
            + "RequestId, LogType, LogLevel, RequestId, LogDateTime, LoginId, UserName, UserAgent, SessionId, IpAddressV4, IpAddressV6, ComputerName, "
            + "FunctionCode, FunctionName, OperationName, OperationTarget, ClassMethod, ServerName, ProcessResult, ProcessResultDetail, ProcessCount, ProcessTime, ExceptionMessage, StackTrace"
            + ") values ("
            + "@TenantId, @LogType, @LogLevel, @RequestId, @LogDateTime, @LoginId, @UserName, @UserAgent,"
            + "@SessionId, @IpAddressV4, @IpAddressV6, @ComputerName, @FunctionCode, @FunctionName, @OperationName,"
            + "@OperationTarget, @ClassMethod, @ServerName, @ProcessResult, @ProcessResultDetail, @ProcessCount, @ProcessTime, @ExceptionMessage, @StackTrace"
            + ")";
        protected readonly string DefaultLayout = "|${event-properties:item=RequestId}" +
                                            "|${event-properties:item=LogType}" +
                                            "|${date}" +
                                            "|${event-properties:item=UserName}" +
                                            "|${event-properties:item=SessionId}" +
                                            "|${event-properties:item=IpAddressV4}" +
                                            "|${event-properties:item=IpAddressV6}" +
                                            "|${machinename}" +
                                            "|${event-properties:item=FunctionCode}" +
                                            "|${event-properties:item=FunctionName}" +
                                            "|${event-properties:item=OperationName}" +
                                            "|${event-properties:item=OperationTarget}" +
                                            "|${callsite}" +
                                            "|${event-properties:item=ServerName}" +
                                            "|${event-properties:item=ProcessResult}" +
                                            "|${event-properties:item=ProcessResultDetail}" +
                                            "|${event-properties:item=ProcessCount}" +
                                            "|${event-properties:item=ProcessTime}" +
                                            "|${event-properties:item=ExceptionMessage}" +
                                            "|${event-properties:item=StackTrace}";

        public void AddConsoleTarget()
        {
            var target = new ConsoleTarget("console")
            {
                Layout = GetConsoleLayout()
            };

            var conf = LogManager.Configuration;
            conf.AddTarget(target);
            conf.LoggingRules.Add(new LoggingRule(GetLoggerName(), LogLevel.Trace, target));
            LogManager.Configuration = conf;
        }

        public void AddFileTarget(string filePath, LogLevel targetLogLevel)
        {
            var target = new FileTarget("file")
            {
                Encoding = Encoding.UTF8,
                FileName = filePath,
                Layout = GetFileLayout()
            };

            SetConfiguration(targetLogLevel, target);
        }

        public virtual void AddDbTarget(string connectionString, LogLevel targetLogLevel)
        {
            var target = new DatabaseTargetBuilder(connectionString)
                .CommandText(GetCommandText())
                .Parameter("@RequestId", "${event-properties:item=RequestId}")
                .Parameter("@LogType", "${event-properties:item=LogType}")
                .Parameter("@LogDateTime", "${date}")
                .Parameter("@UserName", "${event-properties:item=UserName}")
                .Parameter("@SessionId", "${event-properties:item=SessionId}")
                .Parameter("@IpAddressV4", "${event-properties:item=IpAddressV4}")
                .Parameter("@IpAddressV6", "${event-properties:item=IpAddressV6}")
                .Parameter("@ComputerName", "${event-properties:item=ComputerName}")
                .Parameter("@FunctionCode", "${event-properties:item=FunctionCode}")
                .Parameter("@FunctionName", "${event-properties:item=FunctionName}")
                .Parameter("@OperationName", "${event-properties:item=OperationName}")
                .Parameter("@OperationTarget", "${event-properties:item=OperationTarget}")
                .Parameter("@ClassMethod", "${callsite}")
                .Parameter("@ServerName", "${event-properties:item=ServerName}")
                .Parameter("@ProcessResult", "${event-properties:item=ProcessResult}")
                .Parameter("@ProcessResultDetail", "${event-properties:item=ProcessResultDetail}")
                .Parameter("@ProcessCount", "${event-properties:item=ProcessCount}")
                .Parameter("@ProcessTime", "${event-properties:item=ProcessTime}")
                .Parameter("@ExceptionMessage", "${event-properties:item=ExceptionMessage}")
                .Parameter("@StackTrace", "${event-properties:item=StackTrace}")
                .Build();

            SetConfiguration(targetLogLevel, target);
        }
        public class DatabaseTargetBuilder
        {
            private string _connectionString;
            private string _name = "dbtarget";
            private string _dbProvider = "Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient";
            private string _commandText;
            private List<DatabaseParameterInfo> _params = new List<DatabaseParameterInfo>();
            public DatabaseTargetBuilder(string connectionString)
            {
                _connectionString = connectionString;
            }

            public DatabaseTargetBuilder CommandText(string commandText)
            {
                _commandText = commandText;
                return this;
            }
            public DatabaseTargetBuilder Parameter(string name, string layout)
            {
                var parameterInfo = new DatabaseParameterInfo
                {
                    Name = name,
                    Layout = layout
                };
                _params.Add(parameterInfo);
                return this;
            }
            public DatabaseTarget Build()
            {
                var target = new DatabaseTarget()
                {
                    Name = _name,
                    DBProvider = _dbProvider,
                    CommandText = _commandText,
                    ConnectionString = _connectionString
                };

                foreach (var param in _params)
                {
                    target.Parameters.Add(param);
                }

                return target;
            }
        }
        protected void SetConfiguration(LogLevel targetLogLevel, Target target)
        {
            var conf = LogManager.Configuration;
            conf.AddTarget(target);
            conf.LoggingRules.Add(new LoggingRule(GetLoggerName(), targetLogLevel, target));
            LogManager.Configuration = conf;
        }
    }
}
