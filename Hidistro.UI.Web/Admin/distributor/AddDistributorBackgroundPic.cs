using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Config;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.distributor
{
    public class AddDistributorBackgroundPic : AdminPage
    {
        protected Button btnSave;
        protected Button Button1;
        protected FileUpload logoFile;
        protected FileUpload FileUpload1;
        protected TextBox txtDefaultLink;
        protected TextBox txtGuanggaotuLink;
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.logoFile.HasFile)
            {
                this.logoFile.SaveAs(Server.MapPath("/Utility/pics/default.jpg"));
            }

            if (this.FileUpload1.HasFile)
            {
                this.FileUpload1.SaveAs(Server.MapPath("/templates/vshop/default/images/11.jpg"));
            }

            AppConfigModel model = new AppConfigModel() { 
                Id = 1,
                LogoLink = this.txtDefaultLink.Text.Trim(),
                IndexPicLink = this.txtGuanggaotuLink.Text.Trim()
            };

            AppConfigHelper.SaveConfigModel(model);

            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                AppConfigModel model = AppConfigHelper.GetAppConfig();
                txtDefaultLink.Text = model.LogoLink;
                txtGuanggaotuLink.Text = model.IndexPicLink;
            }
            
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.Button1.Click += new EventHandler(this.btnSave_Click);
            
        }
    }
}

