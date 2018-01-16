using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.Web.Admin.distributor
{
    /// <summary>
    /// DistributorAdd 的摘要说明
    /// </summary>
    public class DistributorAdd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest Request = context.Request;

            try
            {
                string userid = Request.Form["userid"];
                string username = Request.Form["username"];
                string mobile = Request.Form["mobile"];
                string weixin = Request.Form["weixin"];
                string parent_id = Request.Form["parent_id"];
                string email = Request.Form["email"];
                string qq = Request.Form["qq"];
                string address = Request.Form["address"];

                VShopHelper.AddDistributor(userid,
                    username,
                    mobile,
                    weixin,
                    parent_id,
                    email,
                    qq,
                    address);

                context.Response.Write("保存成功");
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