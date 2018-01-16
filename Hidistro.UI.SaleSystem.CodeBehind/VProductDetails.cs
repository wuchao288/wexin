namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;
    using Hidistro.Core;
    [ParseChildren(true)]
    public class VProductDetails : VshopTemplatedWebControl
    {
        private Common_ExpandAttributes expandAttr;
        private HyperLink linkDescription;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private HtmlInputHidden litHasCollected;
        private Literal litMarketPrice;
        private Literal litProdcutName;
        private Literal litReviewsCount;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litStock;
        private Literal litFenyong;
        private int productId;
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;
        private Panel fenyongPanel;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
            this.litProdcutName = (Literal)this.FindControl("litProdcutName");
            this.litSalePrice = (Literal)this.FindControl("litSalePrice");
            this.litMarketPrice = (Literal)this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal)this.FindControl("litShortDescription");
            this.litDescription = (Literal)this.FindControl("litDescription");
            this.litStock = (Literal)this.FindControl("litStock");
            this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
            this.linkDescription = (HyperLink)this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal)this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal)this.FindControl("litReviewsCount");
            this.litHasCollected = (HtmlInputHidden)this.FindControl("litHasCollected");
            this.litFenyong = (Literal)this.FindControl("litFenyong");
            this.fenyongPanel = (Panel)this.FindControl("fenyongPanel");

            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            if (product == null)
            {
                base.GotoResourceNotFound("此商品已不存在");
            }

            if (product.SaleStatus != ProductSaleStatus.OnSale)
            {
                base.GotoResourceNotFound("此商品已下架");
            }
            if (this.rptProductImages != null)
            {
                string locationUrl = "javascript:;";
                SlideImage[] imageArray = new SlideImage[] { new SlideImage(product.ImageUrl1, locationUrl), new SlideImage(product.ImageUrl2, locationUrl), new SlideImage(product.ImageUrl3, locationUrl), new SlideImage(product.ImageUrl4, locationUrl), new SlideImage(product.ImageUrl5, locationUrl) };
                this.rptProductImages.DataSource = from item in imageArray
                                                   where !string.IsNullOrWhiteSpace(item.ImageUrl)
                                                   select item;
                this.rptProductImages.DataBind();
            }
            this.litProdcutName.Text = product.ProductName;
            PageTitle.AddSiteNameTitle(product.ProductName);

            this.litSalePrice.Text = product.MinSalePrice.ToString("F2");
            if (product.MarketPrice.HasValue)
            {
                this.litMarketPrice.SetWhenIsNotNull(product.MarketPrice.GetValueOrDefault(0M).ToString("F2"));
            }
            this.litShortDescription.Text = product.ShortDescription;
            if (this.litDescription != null)
            {
                this.litDescription.Text = product.Description;
            }
            this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
            this.litStock.Text = product.Stock.ToString();
            this.skuSelector.ProductId = this.productId;
            if (this.expandAttr != null)
            {
                this.expandAttr.ProductId = this.productId;
            }
            if (this.linkDescription != null)
            {
                this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
            }
            this.litConsultationsCount.SetWhenIsNotNull(ProductBrowser.GetProductConsultationsCount(this.productId, false).ToString());
            this.litReviewsCount.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(this.productId).ToString());
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            bool flag = false;
            if (currentMember != null)
            {
                flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
            }
            this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");

            //显示佣金
            try
            {
                //是否分销商
                DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId());
                if (currentDistributors != null)
                {
                    decimal commission = decimal.Parse(CategoryBrowser.GetCategory(product.CategoryId).FirstCommission);
                    this.litFenyong.Text = (commission * (product.MinSalePrice - product.MinCostPrice) / 100).ToString("F2");
                }
                else
                {
                    this.fenyongPanel.Visible = false;
                }
            }
            catch (Exception)
            {

            }

            ProductBrowser.UpdateVisitCounts(this.productId);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductDetails.html";
            }
            base.OnInit(e);
        }
    }
}

