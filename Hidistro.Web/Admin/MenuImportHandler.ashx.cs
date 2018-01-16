using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.Web.Admin
{
    /// <summary>
    /// MenuImportHandler 的摘要说明
    /// </summary>
    public class MenuImportHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string file = context.Server.MapPath("/Admin");
            ManagerHelper.ImportMenu(file + "/Menu.xml");
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