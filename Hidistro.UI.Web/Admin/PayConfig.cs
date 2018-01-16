namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class PayConfig : AdminPage
    {
        protected Button btnOK;
        protected YesNoRadioButtonList radEnableHtmRewrite;
        protected TextBox txtAppId;
        protected TextBox txtAppSecret;
        protected TextBox txtPartnerID;
        protected TextBox txtPartnerKey;
        protected TextBox txtPaySignKey;

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.WeixinAppId = this.txtAppId.Text;
            masterSettings.WeixinAppSecret = this.txtAppSecret.Text;
            masterSettings.WeixinPartnerID = this.txtPartnerID.Text;
            masterSettings.WeixinPartnerKey = this.txtPartnerKey.Text;
            masterSettings.WeixinPaySignKey = this.txtPaySignKey.Text;
            masterSettings.EnableWeiXinRequest = this.radEnableHtmRewrite.SelectedValue;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("设置成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtAppId.Text = masterSettings.WeixinAppId;
                this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
                this.txtPartnerID.Text = masterSettings.WeixinPartnerID;
                this.txtPartnerKey.Text = masterSettings.WeixinPartnerKey;
                this.txtPaySignKey.Text = masterSettings.WeixinPaySignKey;
                this.radEnableHtmRewrite.SelectedValue = masterSettings.EnableWeiXinRequest;
            }
        }
    }
}

