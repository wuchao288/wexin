namespace Hidistro.UI.Web.Admin.distributor
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BalanceDrawApplyList : AdminPage
    {
        protected Button btapply;
        protected Button btnSearchButton;
        protected HtmlInputHidden hdapplyid;
        protected HtmlInputHidden hdreferralblance;
        protected HtmlInputHidden hduserid;
        protected Pager pager;
        protected Repeater reBalanceDrawRequest;
        private string RequestEndTime = "";
        private string RequestStartTime = "";
        private string StoreName = "";
        protected HtmlTextArea txtcontent;
        protected WebCalendar txtRequestEndTime;
        protected WebCalendar txtRequestStartTime;
        protected TextBox txtStoreName;

        private void BindData()
        {
            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                RequestTime = "",
                CheckTime = "",
                StoreName = this.StoreName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "SerialID",
                RequestEndTime = this.RequestEndTime,
                RequestStartTime = this.RequestStartTime,
                IsCheck = "0",
                UserId = ""
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(entity);
            this.reBalanceDrawRequest.DataSource = balanceDrawRequest.Data;
            this.reBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            int id = int.Parse(this.hdapplyid.Value);
            string remark = this.txtcontent.Value;
            int userId = int.Parse(this.hduserid.Value);
            decimal referralRequestBalance = decimal.Parse(this.hdreferralblance.Value);
            if (VShopHelper.UpdateBalanceDrawRequest(id, remark))
            {
                if (VShopHelper.UpdateBalanceDistributors(userId, referralRequestBalance))
                {
                    this.ShowMsg("结算成功", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("结算失败", false);
                }
            }
            else
            {
                this.ShowMsg("结算失败", false);
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                }
                this.txtStoreName.Text = this.StoreName;
                this.txtRequestStartTime.Text = this.RequestStartTime;
                this.txtRequestEndTime.Text = this.RequestEndTime;
            }
            else
            {
                this.StoreName = this.txtStoreName.Text;
                this.RequestStartTime = this.txtRequestStartTime.Text;
                this.RequestEndTime = this.txtRequestEndTime.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btapply.Click += new EventHandler(this.btnApply_Click);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("RequestStartTime", this.txtRequestStartTime.Text);
            queryStrings.Add("RequestEndTime", this.txtRequestEndTime.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

