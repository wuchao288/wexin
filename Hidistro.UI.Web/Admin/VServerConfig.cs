namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VServerConfig : AdminPage
    {
        protected Button btnAdd;
        protected ImageLinkButton btnPicDelete;
        protected Button btnUpoad;
        protected HtmlInputCheckBox chk_manyService;
        protected HtmlInputCheckBox chkIsValidationService;
        protected FileUpload fileUpload;
        protected FileUpload logoFile;
        protected HiImage imgPic;
        protected Literal litKeycode;
        protected HtmlGenericControl P1;
        protected TextBox txtAppId;
        protected TextBox txtAppSecret;
        protected TextBox txtShopIntroduction;
        protected HtmlGenericControl txtShopIntroductionTip;
        protected TextBox txtSiteName;
        protected HtmlGenericControl txtSiteNameTip;
        protected Literal txtToken;
        protected Literal txtUrl;
        protected TextBox txtWeixinLoginUrl;
        protected TextBox txtWeixinNumber;

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (this.logoFile.HasFile)
            {
                this.logoFile.SaveAs(Server.MapPath("/Utility/pics/headLogo.jpg"));
            }

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.WeixinAppId = this.txtAppId.Text;
            masterSettings.WeixinAppSecret = this.txtAppSecret.Text;
            masterSettings.IsValidationService = this.chkIsValidationService.Checked;
            masterSettings.WeixinNumber = this.txtWeixinNumber.Text;
            masterSettings.WeixinLoginUrl = this.txtWeixinLoginUrl.Text;
            masterSettings.SiteName = this.txtSiteName.Text;
            masterSettings.OpenManyService = this.chk_manyService.Checked;
            masterSettings.ShopIntroduction = this.txtShopIntroduction.Text.Trim();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (!string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl))
            {
                ResourcesHelper.DeleteImage(masterSettings.WeiXinCodeImageUrl);
                this.btnPicDelete.Visible = false;
                masterSettings.WeiXinCodeImageUrl = this.imgPic.ImageUrl = string.Empty;
                SettingsManager.Save(masterSettings);
            }
        }

        private void btnUpoad_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (this.fileUpload.HasFile)
            {
                try
                {
                    if (!string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl))
                    {
                        ResourcesHelper.DeleteImage(masterSettings.WeiXinCodeImageUrl);
                    }
                    this.imgPic.ImageUrl = masterSettings.WeiXinCodeImageUrl = VShopHelper.UploadWeiXinCodeImage(this.fileUpload.PostedFile);
                    this.btnPicDelete.Visible = true;
                    SettingsManager.Save(masterSettings);
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                }
            }
        }

        private string CreateKey(int len)
        {
            byte[] data = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", data[i]));
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnUpoad.Click += new EventHandler(this.btnUpoad_Click);
            this.btnPicDelete.Click += new EventHandler(this.btnPicDelete_Click);
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (string.IsNullOrEmpty(masterSettings.WeixinToken))
                {
                    masterSettings.WeixinToken = this.CreateKey(8);
                    SettingsManager.Save(masterSettings);
                }
                if (string.IsNullOrWhiteSpace(masterSettings.CheckCode))
                {
                    masterSettings.CheckCode = this.CreateKey(20);
                    SettingsManager.Save(masterSettings);
                }
                this.txtSiteName.Text = masterSettings.SiteName;
                this.txtAppId.Text = masterSettings.WeixinAppId;
                this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
                this.txtToken.Text = masterSettings.WeixinToken;
                this.chkIsValidationService.Checked = masterSettings.IsValidationService;
                this.imgPic.ImageUrl = masterSettings.WeiXinCodeImageUrl;
                this.txtWeixinNumber.Text = masterSettings.WeixinNumber;
                this.txtWeixinLoginUrl.Text = masterSettings.WeixinLoginUrl;
                this.btnPicDelete.Visible = !string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl);
                this.litKeycode.Text = masterSettings.CheckCode;
                this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text);
                this.chk_manyService.Checked = masterSettings.OpenManyService;
                this.txtShopIntroduction.Text = masterSettings.ShopIntroduction;
                
                
            }
        }
    }
}

