using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Hidistro.Web
{
    /// <summary>
    /// ProductHandler 的摘要说明
    /// </summary>
    public class ProductHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string categoryId = context.Request.QueryString["categoryId"];
            int? cid = null;
            int pagesize = 20;
            if (categoryId != "") {
                cid = int.Parse(categoryId);
                pagesize = 100;
            }

            int total = 0;
            DataTable homeProduct2 = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, cid, "", 1, pagesize, out total, "ShowSaleCounts", "desc");

            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(homeProduct2));
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