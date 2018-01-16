using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.Web.Admin.store
{
    /// <summary>
    /// QxHandler 的摘要说明
    /// </summary>
    public class QxHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string qx = context.Request.Form["qx"];
            string roleid = context.Request.Form["roleid"];

            try
            {
                ManagerHelper.SaveRoleQx(int.Parse(roleid), qx);

                context.Response.Write("ok");
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
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