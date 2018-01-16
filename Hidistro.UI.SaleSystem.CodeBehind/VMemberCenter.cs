namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberCenter : VMemberTemplatedWebControl
    {
        private Literal litExpenditure;
        private Literal litMemberGrade;
        private Literal litUserName;
        private Literal litWaitForPayCount;
        private Literal litWaitForRecieveCount;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("我是买家");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                
                this.litUserName = (Literal)this.FindControl("litUserName");
                this.litExpenditure = (Literal) this.FindControl("litExpenditure");
                this.litExpenditure.SetWhenIsNotNull(currentMember.Expenditure.ToString("F2"));
                this.litMemberGrade = (Literal) this.FindControl("litMemberGrade");
                MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMember.GradeId);
                if (memberGrade != null)
                {
                    this.litMemberGrade.SetWhenIsNotNull(memberGrade.Name);
                }
                this.litUserName.Text = string.IsNullOrEmpty(currentMember.RealName) ? currentMember.UserName : currentMember.RealName;
                this.Page.Session["stylestatus"] = "1";
                this.litWaitForRecieveCount = (Literal) this.FindControl("litWaitForRecieveCount");
                this.litWaitForPayCount = (Literal) this.FindControl("litWaitForPayCount");
                OrderQuery query = new OrderQuery {
                    Status = OrderStatus.WaitBuyerPay
                };
                int userOrderCount = MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query);
                this.litWaitForPayCount.SetWhenIsNotNull(userOrderCount.ToString());
                query.Status = OrderStatus.SellerAlreadySent;
                this.litWaitForRecieveCount.SetWhenIsNotNull(MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query).ToString());
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberCenter.html";
            }
            base.OnInit(e);
        }
    }
}

