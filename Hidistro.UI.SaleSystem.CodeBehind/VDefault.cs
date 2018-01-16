namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Config;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDefault : VshopTemplatedWebControl
    {
        private HtmlImage img;
        private HiImage imgback;
        private HiImage imglogo;
        private Literal litdescription;
        private Pager pager;
        private VshopTemplatedRepeater iconsRepeater;
        private VshopTemplatedRepeater grdBanner;

        //private Literal litReferralId;
        private Literal litSaleService;

        private Literal logoLinkStart;
        private Literal logoLinkEnd;

        //private VshopTemplatedRepeater rptCategories;

        
        protected override void AttachChildControls()
        {
            //this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.img = (HtmlImage) this.FindControl("imgDefaultBg");
            this.pager = (Pager) this.FindControl("pager");
            this.litdescription = (Literal) this.FindControl("litdescription");
            this.imgback = (HiImage) this.FindControl("imgback");
            this.imglogo = (HiImage) this.FindControl("imglogo");
            this.iconsRepeater = (VshopTemplatedRepeater)this.FindControl("iconsRepeater");
            this.grdBanner = (VshopTemplatedRepeater)this.FindControl("grdBanner");
            //this.litReferralId = (Literal) this.FindControl("litReferralId");
            this.litSaleService = (Literal)this.FindControl("litSaleService");

            this.logoLinkStart = (Literal)this.FindControl("logoLinkStart");
            this.logoLinkEnd = (Literal)this.FindControl("logoLinkEnd");

            AppConfigModel model = AppConfigHelper.GetAppConfig();


            this.Page.Session["stylestatus"] = "3";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            PageTitle.AddSiteNameTitle();
            if (base.referralId <= 0)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if (!((cookie == null) || string.IsNullOrEmpty(cookie.Value)))
                {
                    base.referralId = int.Parse(cookie.Value);
                }
            }

            //this.litReferralId.Text = base.referralId.ToString();

            DistributorsInfo userIdDistributors = new DistributorsInfo();
            userIdDistributors = DistributorsBrower.GetUserIdDistributors(base.referralId);

            if (this.img != null)
            {
                this.img.Src = new VTemplateHelper().GetDefaultBg();
            }

            MemberInfo member = MemberProcessor.GetMember(base.referralId);
            if (member != null) {
                this.litSaleService.Text = member.CellPhone;
                
            }

            //读取图标配置
            this.iconsRepeater.DataSource = VShopHelper.GetAllNavigate();
            this.iconsRepeater.DataBind();

            //幻灯片
            this.grdBanner.DataSource = VShopHelper.GetAllBanners();
            this.grdBanner.DataBind();


            //IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxMainCategories(100);
            //this.rptCategories.DataSource = maxSubCategories;
            //this.rptCategories.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDefault.html";
            }
            base.OnInit(e);
        }
    }
}

