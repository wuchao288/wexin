namespace Hidistro.UI.Web.Admin.tools
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class SendMessageTemplets : AdminPage
    {
        protected Button btnSaveSendSetting;
        protected Grid grdEmailTemplets;

        private void btnSaveSendSetting_Click(object sender, EventArgs e)
        {
            List<MessageTemplate> templates = new List<MessageTemplate>();
            foreach (GridViewRow row in this.grdEmailTemplets.Rows)
            {
                MessageTemplate item = new MessageTemplate();
                CheckBox box = (CheckBox) row.FindControl("chkWeixinMessage");
                item.SendWeixin = box.Checked;
                item.MessageType = (string) this.grdEmailTemplets.DataKeys[row.RowIndex].Value;
                templates.Add(item);
            }
            VShopHelper.UpdateSettings(templates);
            this.ShowMsg("保存设置成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveSendSetting.Click += new EventHandler(this.btnSaveSendSetting_Click);
            if (!this.Page.IsPostBack)
            {
                this.grdEmailTemplets.DataSource = VShopHelper.GetMessageTemplates();
                this.grdEmailTemplets.DataBind();
            }
        }
    }
}

