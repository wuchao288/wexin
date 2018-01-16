namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VRequestCommissions : VMemberTemplatedWebControl
    {
        private Literal litMaxMoney;
        private HtmlInputText txtaccount;
        private HtmlInputText txtmoney;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("申请提现");
            this.litMaxMoney = (Literal) this.FindControl("litMaxMoney");
            this.txtaccount = (HtmlInputText) this.FindControl("txtaccount");
            this.txtmoney = (HtmlInputText) this.FindControl("txtmoney");
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
            this.txtaccount.Value = userIdDistributors.RequestAccount;
            this.litMaxMoney.Text = userIdDistributors.ReferralBlance.ToString("F2");
            decimal result = 0M;
            if (decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out result) && (result > 0M))
            {
                this.litMaxMoney.Text = ((userIdDistributors.ReferralBlance / result) * result).ToString("F2");
                this.txtmoney.Attributes["placeholder"] = "请输入" + result + "的倍数金额";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-RequestCommissions.html";
            }
            base.OnInit(e);
        }
    }
}

