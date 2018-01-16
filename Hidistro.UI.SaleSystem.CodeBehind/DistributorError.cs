namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class DistributorError : VMemberTemplatedWebControl
    {
        private Literal litBackImg;
        private Literal txtSiteName;
        private Literal txtApplySet;
        private Literal txtMoney;
        //private Literal litReferralUserId;
        
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("申请分销");
            this.litBackImg = (Literal)this.FindControl("litBackImg");
            this.txtSiteName = (Literal)this.FindControl("txtSiteName");
            this.txtApplySet = (Literal)this.FindControl("txtApplySet");
            //this.litReferralUserId = (Literal)this.FindControl("litReferralUserId");
            this.txtMoney = (Literal)this.FindControl("txtMoney");

            this.Page.Session["stylestatus"] = "2";
            MemberInfo member = MemberProcessor.GetCurrentMember();
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(member.UserId);

            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                this.Page.Response.Redirect("DistributorCenter.aspx", true);
            }
            if (this.litBackImg != null)
            {
                List<string> list = SettingsManager.GetMasterSettings(false).DistributorBackgroundPic.Split(new char[] { '|' }).ToList<string>();
                foreach (string str in list)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        this.litBackImg.Text = this.litBackImg.Text + "<div><span class=\"mark\"></span><img src=\"" + str + "\" /></div>";
                    }
                }
            }

            //获取配置信息
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            //商城名称
            this.txtSiteName.Text = masterSettings.SiteName;

            //分销策略
            this.txtApplySet.Text = masterSettings.DistributorDescription;

            //分销金额要求
            this.txtMoney.Text = masterSettings.FinishedOrderMoney.ToString();

            ////上级会员ID
            //MemberInfo m = MemberProcessor.GetMember(member.UserId);
            //if (m.ReferralUserId != 0)
            //{
            //    MemberInfo referralUser = MemberProcessor.GetMember(m.ReferralUserId);
            //    this.litReferralUserId.Text = string.Format("<p>推荐人：<span class='tuchu'>{0}</span></p>", referralUser.UserName);
            //}
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDistributorError.html";
            }
            base.OnInit(e);
        }
    }
}

