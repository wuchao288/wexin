namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorOrders : VMemberTemplatedWebControl
    {
        private Literal litallnum;
        private Literal litfinishnum;
        private VshopTemplatedRepeater vshoporders;

        protected override void AttachChildControls()
        {
            this.vshoporders = (VshopTemplatedRepeater) this.FindControl("vshoporders");
            this.litfinishnum = (Literal) this.FindControl("litfinishnum");
            this.litallnum = (Literal) this.FindControl("litallnum");
            int result = 0;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out result);
            OrderQuery query = new OrderQuery {
                UserId = new int?(Globals.GetCurrentMemberUserId())
            };
            if (result != 5)
            {
                query.Status = OrderStatus.Finished;
                this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                query.Status = OrderStatus.All;
                this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
            }
            else
            {
                this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                query.Status = OrderStatus.Finished;
                this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
            }
            this.vshoporders.DataSource = DistributorsBrower.GetDistributorOrder(query);
            this.vshoporders.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorOrders.html";
            }
            base.OnInit(e);
        }
    }
}

