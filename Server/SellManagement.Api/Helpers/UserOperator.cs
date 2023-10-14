using Microsoft.AspNetCore.Http;
using SellManagement.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Helpers
{
    public class UserOperator
    {
        IHttpContextAccessor _httpContext;
        public UserOperator(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetRequestUserId()
        {
            return GetRequestUser().LoginId;
        }

        public Functions.User GetRequestUser()
        {
            if (_httpContext == null)
                return null;

            return ((Functions.User)_httpContext.HttpContext.Items["User"]);

        }
    }
}
