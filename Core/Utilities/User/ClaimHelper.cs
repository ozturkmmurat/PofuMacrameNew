using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.User
{
    public static class ClaimHelper
    {
        public static string GetUserName(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Name).Value.ToString();
        }

        public static string GetUserLastName(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Surname).Value.ToString();
        }

        public static string GetUserEmail(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Email).Value.ToString();
        }

        public static int GetUserId(HttpContext httpContext)
        {
            return int.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public static int GetCustomerId(HttpContext httpContext)
        {
            return int.Parse(httpContext.User.FindFirst("CustomerId").Value);
        }
    }
}
