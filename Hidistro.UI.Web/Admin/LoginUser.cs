namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;

    public class LoginUser : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            string str2 = context.Request.QueryString["action"];
            if (!string.IsNullOrEmpty(str2) && (str2 == "login"))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                s = ("{\"sitename\":\"" + masterSettings.SiteName + "\",") + "\"username\":\"" + ManagerHelper.GetCurrentManager().UserName + "\"}";
            }
            context.Response.ContentType = "text/json";
            context.Response.Write(s);
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

