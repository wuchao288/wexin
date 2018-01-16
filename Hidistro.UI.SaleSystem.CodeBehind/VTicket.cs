namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VTicket : VMemberTemplatedWebControl
    {
        private Literal litActivityDesc;
        private Literal litEndDate;
        private Common_PrizeNames litPrizeNames;
        private Literal litStartDate;

        protected override void AttachChildControls()
        {
            int num;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("id"), out num);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(num);
            if (lotteryTicket == null)
            {
                base.GotoResourceNotFound("");
            }
            this.litActivityDesc = (Literal) this.FindControl("litActivityDesc");
            this.litPrizeNames = (Common_PrizeNames) this.FindControl("litPrizeNames");
            this.litStartDate = (Literal) this.FindControl("litStartDate");
            this.litEndDate = (Literal) this.FindControl("litEndDate");
            this.litActivityDesc.Text = lotteryTicket.ActivityDesc;
            this.litPrizeNames.Activity = lotteryTicket;
            this.litStartDate.Text = lotteryTicket.OpenTime.ToString("yyyy年MM月dd日 HH:mm:ss");
            this.litEndDate.Text = lotteryTicket.EndTime.ToString("yyyy年MM月dd日 HH:mm:ss");
            PageTitle.AddSiteNameTitle("微抽奖");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vTicket.html";
            }
            base.OnInit(e);
        }
    }
}

