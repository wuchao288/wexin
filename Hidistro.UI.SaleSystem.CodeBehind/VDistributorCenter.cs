namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorCenter : VMemberTemplatedWebControl
    {
        private Literal litdistirbutors;
        //private DistributorGradeLitral litGrade;
        private Literal litQRcode;
        private Literal litStoreNum;
        private Literal litStroeName;
        private FormatedMoneyLabel refrraltotal;
        private FormatedMoneyLabel saletotal;
        private Literal litRefrralName;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("我是卖家");
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(MemberProcessor.GetCurrentMember().UserId);

            //Utils.LogWriter.SaveLog("UserId " + MemberProcessor.GetCurrentMember().UserId);
            //Utils.LogWriter.SaveLog("Status " + userIdDistributors.Status);
            if (userIdDistributors.Status == 1)
            {
                Utils.LogWriter.SaveLog("跳转");
                this.Page.Response.Redirect("/Vshop/DistributorDisable.aspx");
                return;
            }

            if (userIdDistributors != null)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

                //this.litGrade = (DistributorGradeLitral) this.FindControl("litGrade");
                this.litStroeName = (Literal) this.FindControl("litStroeName");
                this.saletotal = (FormatedMoneyLabel) this.FindControl("saletotal");
                this.refrraltotal = (FormatedMoneyLabel) this.FindControl("refrraltotal");
                this.litStoreNum = (Literal) this.FindControl("litStoreNum");
                this.litdistirbutors = (Literal) this.FindControl("litdistirbutors");
                this.litQRcode = (Literal) this.FindControl("litQRcode");
                this.litRefrralName = (Literal)this.FindControl("litRefrralName");

                this.litdistirbutors.Text = "<a href=\"ChirldrenDistributors.aspx\" class=\"list-group-item\">下级分店<span class=\"glyphicon glyphicon-chevron-right\"></span></a>";
                this.litQRcode.Text = "<a href=\"QRcode.aspx?ReferralId=" + userIdDistributors.UserId + "\" class=\"list-group-item\">本店二维码<span class=\"glyphicon glyphicon-chevron-right\"></span></a>";
                this.litStroeName.Text = masterSettings.SiteName + " - " + MemberProcessor.GetCurrentMember().RealName;
                this.refrraltotal.Money = userIdDistributors.ReferralBlance;
                this.saletotal.Money = userIdDistributors.OrdersTotal;
                //userIdDistributors.StoreName

                if (userIdDistributors.ReferralUserId > 0) {
                    MemberInfo mem = MemberProcessor.GetMember(userIdDistributors.ReferralUserId);

                    if (mem != null)
                    {
                        litRefrralName.Text = "推荐人：" + mem.RealName;
                    }
                }
                //this.litGrade.GradeId = userIdDistributors.DistributorGradeId;

                this.litStoreNum.Text = DistributorsBrower.GetDistributorNum().ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorCenter.html";
            }
            base.OnInit(e);
        }
    }
}

