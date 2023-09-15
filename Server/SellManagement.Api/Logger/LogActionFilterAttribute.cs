using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using NLog;

namespace SellManagement.Api.Logger
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger logger;
        /// <summary>  
        /// Initializes a new instance of the <see cref="LogActionFilterAttribute" /> class.  
        /// </summary>  
        /// <param name="logger">The logger.</param>  
        public LogActionFilterAttribute(ILogger logger)
        {
            this.logger = logger;
        }
        /// <summary>  
        /// Called when [action executing].  
        /// </summary>  
        /// <param name="context">The filter context.</param>  
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.Log("OnActionExecuting", context.RouteData);
            base.OnActionExecuting(context);
        }
        /// <summary>  
        /// Called when [action executed].  
        /// </summary>  
        /// <param name="context"></param>  
        /// <inheritdoc />  
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            this.Log("OnActionExecuted", context.RouteData);
            base.OnActionExecuted(context);
        }
        /// <summary>  
        /// Called when [result executing].  
        /// </summary>  
        /// <param name="context">The filter context.</param>  
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            this.Log("OnResultExecuting", context.RouteData);
            base.OnResultExecuting(context);
        }
        /// <summary>  
        /// Called when [result executed].  
        /// </summary>  
        /// <param name="context">The filter context.</param>  
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            this.Log("OnResultExecuted", context.RouteData);
            base.OnResultExecuted(context);
        }
        /// <summary>  
        /// Logs the specified method name.  
        /// </summary>  
        /// <param name="methodName">Name of the method.</param>  
        /// <param name="routeData">The route data.</param>  
        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            string message = $"MethodName :{methodName} , controller:{controllerName} , action:{actionName}";
            //this.logger.Log(message);

            this.logger.Error("aaa");
        }
    }
}
