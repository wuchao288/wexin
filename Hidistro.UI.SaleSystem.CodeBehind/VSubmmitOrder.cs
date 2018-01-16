namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VSubmmitOrder : VMemberTemplatedWebControl
    {
        private HtmlAnchor aLinkToShipping;
        private int buyAmount;
        private Common_CouponSelect dropCoupon;
        private Common_ShippingTypeSelect dropShippingType;
        private HtmlInputControl groupbuyHiddenBox;
        private int groupBuyId;
        private HtmlInputHidden hiddenCartTotal;
        private Literal litAddAddress;
        private Literal litAddress;
        private Literal litCellPhone;
        private Literal litOrderTotal;
        private Literal litProductTotalPrice;
        private Literal litShipTo;
        private string productSku;
        private HtmlInputHidden regionId;
        private VshopTemplatedRepeater rptAddress;
        private VshopTemplatedRepeater rptCartProducts;
        private HtmlInputHidden selectShipTo;

        protected override void AttachChildControls()
        {
            this.litShipTo = (Literal) this.FindControl("litShipTo");
            this.litCellPhone = (Literal) this.FindControl("litCellPhone");
            this.litAddress = (Literal) this.FindControl("litAddress");
            this.rptCartProducts = (VshopTemplatedRepeater) this.FindControl("rptCartProducts");
            this.dropShippingType = (Common_ShippingTypeSelect) this.FindControl("dropShippingType");
            this.dropCoupon = (Common_CouponSelect) this.FindControl("dropCoupon");
            this.litOrderTotal = (Literal) this.FindControl("litOrderTotal");
            this.hiddenCartTotal = (HtmlInputHidden) this.FindControl("hiddenCartTotal");
            this.aLinkToShipping = (HtmlAnchor) this.FindControl("aLinkToShipping");
            this.groupbuyHiddenBox = (HtmlInputControl) this.FindControl("groupbuyHiddenBox");
            this.rptAddress = (VshopTemplatedRepeater) this.FindControl("rptAddress");
            this.selectShipTo = (HtmlInputHidden) this.FindControl("selectShipTo");
            this.regionId = (HtmlInputHidden) this.FindControl("regionId");
            Literal literal = (Literal) this.FindControl("litProductTotalPrice");
            this.litAddAddress = (Literal) this.FindControl("litAddAddress");
            IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
            this.rptAddress.DataSource = from item in shippingAddresses
                orderby item.IsDefault
                select item;
            this.rptAddress.DataBind();
            ShippingAddressInfo info = shippingAddresses.FirstOrDefault<ShippingAddressInfo>(item => item.IsDefault);
            if (info == null)
            {
                info = (shippingAddresses.Count > 0) ? shippingAddresses[0] : null;
            }
            if (info != null)
            {
                this.litShipTo.Text = info.ShipTo;
                this.litCellPhone.Text = info.CellPhone;
                this.litAddress.Text = info.Address;
                this.selectShipTo.SetWhenIsNotNull(info.ShippingId.ToString());
                this.regionId.SetWhenIsNotNull(info.RegionId.ToString());
            }
            this.litAddAddress.Text = " href='/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "'";
            if ((shippingAddresses == null) || (shippingAddresses.Count == 0))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
            }
            else
            {
                this.aLinkToShipping.HRef = Globals.ApplicationPath + "/Vshop/ShippingAddresses.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString());
                ShoppingCartInfo shoppingCart = null;
                if (((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"])) && ((this.Page.Request.QueryString["from"] == "signBuy") || (this.Page.Request.QueryString["from"] == "groupBuy")))
                {
                    this.productSku = this.Page.Request.QueryString["productSku"];
                    if (int.TryParse(this.Page.Request.QueryString["groupbuyId"], out this.groupBuyId))
                    {
                        this.groupbuyHiddenBox.SetWhenIsNotNull(this.groupBuyId.ToString());
                        shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(this.groupBuyId, this.productSku, this.buyAmount);
                    }
                    else
                    {
                        shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
                    }
                }
                else
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                }
                if (shoppingCart != null)
                {
                    this.rptCartProducts.DataSource = shoppingCart.LineItems;
                    this.rptCartProducts.DataBind();
                    this.dropShippingType.ShoppingCartItemInfo = shoppingCart.LineItems;
                    this.dropShippingType.RegionId = 0;
                    this.dropShippingType.Weight = shoppingCart.Weight;
                    this.dropCoupon.CartTotal = shoppingCart.GetTotal();
                    this.hiddenCartTotal.Value = this.litOrderTotal.Text = literal.Text = shoppingCart.GetTotal().ToString("F2");
                }
                PageTitle.AddSiteNameTitle("订单确认");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VSubmmitOrder.html";
            }
            base.OnInit(e);
        }
    }
}

