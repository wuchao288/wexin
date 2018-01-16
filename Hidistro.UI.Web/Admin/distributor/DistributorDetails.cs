namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class DistributorDetails : AdminPage
    {
        protected Button btnSubmit;
        protected Literal lblStoreName;
        protected Literal litCellPhone;
        protected Literal litCommission;
        protected Literal litDownGradeNum;
        protected Literal litGreade;
        protected Literal litMicroSignal;
        protected Literal litOrders;
        protected Literal litQQ;
        protected Literal litRealName;
        protected Literal litUpGrade;
        protected Literal litUserName;
        private int userid;

        private void Bind()
        {
            DistributorsQuery entity = new DistributorsQuery {
                UserId = int.Parse(this.Page.Request.QueryString["UserId"]),
                PageIndex = 1,
                PageSize = 1,
                SortOrder = SortAction.Desc,
                SortBy = "userid"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult distributors = VShopHelper.GetDistributors(entity);
            if (distributors.Data != null)
            {
                DataTable data = new DataTable();
                data = (DataTable) distributors.Data;
                this.litUserName.Text = data.Rows[0]["RealName"].ToString();
                this.lblStoreName.Text = data.Rows[0]["StoreName"].ToString();
                this.litRealName.Text = data.Rows[0]["RealName"].ToString();
                this.litCellPhone.Text = data.Rows[0]["CellPhone"].ToString();
                this.litQQ.Text = data.Rows[0]["QQ"].ToString();
                this.litMicroSignal.Text = data.Rows[0]["MicroSignal"].ToString();
                this.litGreade.Text = (data.Rows[0]["GradeId"].ToString() == "1") ? "一级" : ((data.Rows[0]["GradeId"].ToString() == "2") ? "二级" : "三级");
                this.litOrders.Text = "本站订单数：" + data.Rows[0]["ReferralOrders"].ToString() + ",所有下级分销商订单数：" + VShopHelper.GetDownDistributorNumReferralOrders(data.Rows[0]["UserId"].ToString()).ToString();
                this.litCommission.Text = data.Rows[0]["ReferralBlance"].ToString();
                DistributorsInfo userIdDistributors = VShopHelper.GetUserIdDistributors(int.Parse(data.Rows[0]["ReferralUserId"].ToString()));
                if (userIdDistributors != null)
                {
                    this.litUpGrade.Text = userIdDistributors.StoreName;
                }
                else
                {
                    this.litUpGrade.Text = "一级分销商";
                }
                this.litDownGradeNum.Text = VShopHelper.GetDownDistributorNum(data.Rows[0]["UserId"].ToString()).ToString();
            }
            else
            {
                this.ShowMsg("分销商信息不存在！", false);
            }
        }

        private void btn_Submint(object sender, EventArgs e)
        {
            this.Page.Response.Redirect("DistributorList.aspx");
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

