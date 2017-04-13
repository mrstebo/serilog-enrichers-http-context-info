﻿using System.Web;

namespace Serilog
{
    public interface IHttpContextProvider
    {
        IHttpContextWrapper GetCurrentContext();
    }

    internal class HttpContextProvider : IHttpContextProvider
    {
        public IHttpContextWrapper GetCurrentContext()
        {
            return HttpContext.Current == null
                ? null
                : new HttpContextWrapper(HttpContext.Current);
        }
    }
}