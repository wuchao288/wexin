using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.Web.Admin.distributor
{
    /// <summary>
    /// DistributorStatus 的摘要说明
    /// </summary>
    public class DistributorStatus : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest Request = context.Request;

            string userid = Request.Form["userid"];
            int status = int.Parse(Request.Form["status"]);
            if (string.IsNullOrEmpty(userid))
            {
                context.Response.Write("error");
                return;
            }

            VShopHelper.SetDistributorStatus(int.Parse(userid), status);

            context.Response.Write("success");
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