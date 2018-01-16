namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorInfo : VMemberTemplatedWebControl
    {
        private HtmlInputHidden hdbackimg;
        private HtmlInputHidden hdlogo;
        private HtmlImage imglogo;
        private Literal litBackImg;
        private HtmlTextArea txtdescription;
        private HtmlInputText txtstorename;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺消息");
            this.imglogo = (HtmlImage) this.FindControl("imglogo");
            this.litBackImg = (Literal) this.FindControl("litBackImg");
            this.hdbackimg = (HtmlInputHidden) this.FindControl("hdbackimg");
            this.hdlogo = (HtmlInputHidden) this.FindControl("hdlogo");
            this.txtstorename = (HtmlInputText) this.FindControl("txtstorename");
            this.txtdescription = (HtmlTextArea) this.FindControl("txtdescription");
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId());
            if (currentDistributors != null)
            {
                if (!string.IsNullOrEmpty(currentDistributors.Logo))
                {
                    this.imglogo.Src = currentDistributors.Logo;
                }
                this.hdbackimg.Value = currentDistributors.BackImage;
                this.txtstorename.Value = currentDistributors.StoreName;
                this.txtdescription.Value = currentDistributors.StoreDescription;
                if (this.litBackImg != null)
                {
                    List<string> list = SettingsManager.GetMasterSettings(false).DistributorBackgroundPic.Split(new char[] { '|' }).ToList<string>();
                    foreach (string str in list)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (this.hdbackimg.Value == str)
                            {
                                this.litBackImg.Text = this.litBackImg.Text + "<div class=\"disactive\"><span class=\"mark\"></span><img src=\"" + str + "\"/></div>";
                            }
                            else
                            {
                                this.litBackImg.Text = this.litBackImg.Text + "<div><span class=\"mark\"></span><img src=\"" + str + "\" /></div>";
                            }
                        }
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorInfo.html";
            }
            base.OnInit(e);
        }
    }
}

