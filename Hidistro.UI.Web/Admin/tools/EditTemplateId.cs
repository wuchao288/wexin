namespace Hidistro.UI.Web.Admin.tools
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class EditTemplateId : AdminPage
    {
        protected Button btnSaveEmailTemplet;
        protected TextBox txtTemplateId;

        private void btnSaveEmailTemplet_Click(object sender, EventArgs e)
        {
            string text = this.txtTemplateId.Text;
            string messageType = base.Request["MessageType"];
            MessageTemplate messageTemplate = VShopHelper.GetMessageTemplate(messageType);
            messageTemplate.WeixinTemplateId = text;
            try
            {
                VShopHelper.UpdateTemplate(messageTemplate);
                this.ShowMsg("保存模板Id成功", true);
            }
            catch
            {
            }
        }

        private void InitShow()
        {
            string messageType = base.Request["MessageType"];
            MessageTemplate messageTemplate = VShopHelper.GetMessageTemplate(messageType);
            this.txtTemplateId.Text = messageTemplate.WeixinTemplateId;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSaveEmailTemplet.Click += new EventHandler(this.btnSaveEmailTemplet_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.InitShow();
            }
        }
    }
}

