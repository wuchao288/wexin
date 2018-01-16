namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class EditDistributor : AdminPage
    {
        protected Button btnSubmit;
        protected DropDownList DrStatus;
        protected TextBox txtStoreDescription;
        protected TextBox txtStoreName;
        private int userid;

        private void Bind()
        {
            DistributorsInfo userIdDistributors = VShopHelper.GetUserIdDistributors(int.Parse(this.Page.Request.QueryString["UserId"]));
            if (userIdDistributors != null)
            {
                this.txtStoreName.Text = userIdDistributors.StoreName;
                this.txtStoreDescription.Text = userIdDistributors.StoreDescription;
                this.DrStatus.SelectedValue = userIdDistributors.ReferralStatus.ToString();
            }
            else
            {
                this.ShowMsg("分销商不存在！", false);
            }
        }

        private void btn_Submint(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtStoreName.Text.Trim()))
            {
                this.ShowMsg("店铺店不能为空", false);
            }
            else
            {
                DistributorsInfo userIdDistributors = VShopHelper.GetUserIdDistributors(int.Parse(this.Page.Request.QueryString["UserId"]));
                if (userIdDistributors != null)
                {
                    userIdDistributors.StoreName = this.txtStoreName.Text.Trim();
                    userIdDistributors.StoreDescription = this.txtStoreDescription.Text.Trim();
                    userIdDistributors.ReferralStatus = int.Parse(this.DrStatus.SelectedValue);
                    if (DistributorsBrower.UpdateDistributor(userIdDistributors))
                    {
                        this.ShowMsg("店铺店修改成功", true);
                    }
                    else
                    {
                        this.ShowMsg("店铺店修改失败", false);
                    }
                }
                else
                {
                    this.ShowMsg("分销商不存在！", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmit.Click += new EventHandler(this.btn_Submint);
            if (!base.IsPostBack)
            {
                if (int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
                {
                    this.Bind();
                }
                else
                {
                    this.Page.Response.Redirect("DistributorList.aspx");
                }
            }
        }
    }
}

