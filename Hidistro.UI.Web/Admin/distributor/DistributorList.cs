namespace Hidistro.UI.Web.Admin.distributor
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class DistributorList : AdminPage
    {
        protected Button btnSearchButton;
        private string CellPhone = "";
        protected DropDownList DrGrade;
        protected DropDownList DrStatus;
        private string Grade = "0";
        private string MicroSignal = "";
        protected Pager pager;
        private string RealName = "";
        protected Repeater reDistributor;
        private string Status = "0";
        private string StoreName = "";
        protected TextBox txtCellPhone;
        protected TextBox txtMicroSignal;
        protected TextBox txtRealName;

        private void BindData()
        {
            DistributorsQuery entity = new DistributorsQuery {
                GradeId = int.Parse(this.Grade),
                StoreName = this.StoreName,
                CellPhone = this.CellPhone,
                RealName = this.RealName,
                MicroSignal = this.MicroSignal,
                ReferralStatus = int.Parse(this.Status),
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "userid"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult distributors = VShopHelper.GetDistributors(entity);
            this.reDistributor.DataSource = distributors.Data;
            this.reDistributor.DataBind();
            this.pager.TotalRecords = distributors.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
    
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Grade"]))
                {
                    this.Grade = base.Server.UrlDecode(this.Page.Request.QueryString["Grade"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
                {
                    this.Status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
                {
                    this.RealName = base.Server.UrlDecode(this.Page.Request.QueryString["RealName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
                {
                    this.CellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["CellPhone"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MicroSignal"]))
                {
                    this.MicroSignal = base.Server.UrlDecode(this.Page.Request.QueryString["MicroSignal"]);
                }
                this.DrStatus.SelectedValue = this.Status;
                this.DrGrade.SelectedValue = this.Grade;
                this.txtCellPhone.Text = this.CellPhone;
                this.txtMicroSignal.Text = this.MicroSignal;
                this.txtRealName.Text = this.RealName;
            }
            else
            {
                this.Status = this.DrStatus.SelectedValue;
                this.Grade = this.DrGrade.SelectedValue;
                this.CellPhone = this.txtCellPhone.Text;
                this.RealName = this.txtRealName.Text;
                this.MicroSignal = this.txtMicroSignal.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
            queryStrings.Add("Grade", this.DrGrade.Text);
            queryStrings.Add("CellPhone", this.txtCellPhone.Text);
            queryStrings.Add("RealName", this.txtRealName.Text);
            queryStrings.Add("MicroSignal", this.txtMicroSignal.Text);
            queryStrings.Add("Status", this.DrStatus.SelectedValue);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

