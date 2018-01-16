using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.Web.Admin
{
    /// <summary>
    /// MenuHandler 的摘要说明
    /// </summary>
    public class MenuHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            HttpCookie cookie = context.Request.Cookies["roleid"];
            if (cookie == null || cookie.Value == "") {
                return;
            }

            string mudule = context.Request.Form["module"];
            if(string.IsNullOrEmpty(mudule)){
                return;
            }

            int roleid = int.Parse(cookie.Value);

            IList<MenuQxInfo> list = ManagerHelper.GetMenuList(roleid, mudule);
            Hashtable result = new Hashtable();

            foreach (MenuQxInfo item in list)
            {
                IList<MenuQxInfo> ls = null;
                if (result.ContainsKey(item.category))
                {
                    ls = result[item.category] as IList<MenuQxInfo>;
                    ls.Add(item);
                }
                else {
                    ls = new List<MenuQxInfo>();
                    ls.Add(item);
                    result.Add(item.category, ls);
                }

            }

            string json = JsonConvert.SerializeObject(result);

            context.Response.Write(json);
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