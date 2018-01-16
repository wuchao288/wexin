namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VChirldrenDistributors : VMemberTemplatedWebControl
    {
        private Panel onedistributor;
        private VshopTemplatedRepeater rpdistributor;
        private Panel twodistributor;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("下级分销商");
            this.rpdistributor = (VshopTemplatedRepeater) this.FindControl("rpdistributor");
            DistributorsQuery query = new DistributorsQuery {
                PageIndex = 1,
                PageSize = 0x2710
            };
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId());
            if (this.Page.Request.QueryString["gradeId"] == "3")
            {
                query.ReferralUserId2 = currentDistributors.UserId;
            }
            else
            {
                query.ReferralUserId = currentDistributors.UserId;
            }

            query.UserId = currentDistributors.UserId;
            
            this.rpdistributor.DataSource = DistributorsBrower.GetDistributorsCommission(query);
            this.rpdistributor.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VChirldrenDistributors.html";
            }
            base.OnInit(e);
        }
    }
}

