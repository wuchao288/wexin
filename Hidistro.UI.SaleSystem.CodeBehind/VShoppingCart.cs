namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VShoppingCart : VMemberTemplatedWebControl
    {
        private HtmlAnchor aLink;
        private HtmlGenericControl divShowTotal;
        private Literal litTotal;
        private VshopTemplatedRepeater rptCartProducts;

        protected override void AttachChildControls()
        {
            this.rptCartProducts = (VshopTemplatedRepeater) this.FindControl("rptCartProducts");
            this.litTotal = (Literal) this.FindControl("litTotal");
            this.divShowTotal = (HtmlGenericControl) this.FindControl("divShowTotal");
            this.aLink = (HtmlAnchor) this.FindControl("aLink");
            this.Page.Session["stylestatus"] = "0";
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart != null)
            {
                this.rptCartProducts.DataSource = shoppingCart.LineItems;
                this.rptCartProducts.DataBind();
                this.litTotal.Text = shoppingCart.GetAmount().ToString("F2");
            }
            PageTitle.AddSiteNameTitle("购物车");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VShoppingCart.html";
            }
            base.OnInit(e);
        }
    }
}

