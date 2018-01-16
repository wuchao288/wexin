using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.Web.Vshop
{
    public partial class CategoryProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxMainCategories(100);

                this.categoryRepeater.DataSource = maxSubCategories;
                this.categoryRepeater.DataBind();

                //热卖榜单
                int total = 0;
                DataTable homeProduct2 = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, null, "", 1, 20, out total, "ShowSaleCounts", "desc");

                this.productRepeater.DataSource = homeProduct2;
                this.productRepeater.DataBind();
            }
        }
    }
}