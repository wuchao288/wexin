﻿namespace Hidistro.UI.Web.Admin.sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageSendNote : AdminPage
    {
        protected Button btnSearchButton;
        protected DataList dlstSendNote;
        protected HtmlInputHidden hidOrderId;
        protected PageSize hrefPageSize;
        protected Label lblStatus;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtOrderId;

        private void BindSendNote()
        {
            RefundApplyQuery refundApplyQuery = this.GetRefundApplyQuery();
            DbQueryResult allSendNote = OrderHelper.GetAllSendNote(refundApplyQuery);
            this.dlstSendNote.DataSource = allSendNote.Data;
            this.dlstSendNote.DataBind();
            this.pager.TotalRecords = allSendNote.TotalRecords;
            this.pager1.TotalRecords = allSendNote.TotalRecords;
            this.txtOrderId.Text = refundApplyQuery.OrderId;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadSendNotes(true);
        }

        private RefundApplyQuery GetRefundApplyQuery()
        {
            RefundApplyQuery query = new RefundApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "ShippingDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要删除的发货单", false);
            }
            else
            {
                int num;
                OrderHelper.DelSendNote(str.Split(new char[] { ',' }), out num);
                this.BindSendNote();
                this.ShowMsg(string.Format("成功删除了{0}个发货单", num), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            if (!base.IsPostBack)
            {
                this.BindSendNote();
            }
        }

        private void ReloadSendNotes(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OrderId", this.txtOrderId.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                queryStrings.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            base.ReloadPage(queryStrings);
        }
    }
}

