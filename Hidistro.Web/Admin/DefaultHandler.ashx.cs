using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.Web.Admin
{
    /// <summary>
    /// DefaultHandler 的摘要说明
    /// </summary>
    public class DefaultHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpCookie cookie = context.Request.Cookies["roleid"];
            if (cookie == null || cookie.Value == "") {
                return;
            }

            int roleid = int.Parse(cookie.Value);

            context.Response.Write(ManagerHelper.GetModuleQx(roleid));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}