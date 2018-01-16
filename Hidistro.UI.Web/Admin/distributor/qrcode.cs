using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.distributor
{
    public class qrcode : AdminPage
    {
        protected Button btnSave;
        protected Button Button1;
        protected FileUpload logoFile;
        protected FileUpload FileUpload1;
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.logoFile.HasFile)
            {
                this.logoFile.SaveAs(Server.MapPath("/Templates/vshop/default/images/qrcode/qrcode_01.png"));
            }

            if (this.FileUpload1.HasFile)
            {
                this.FileUpload1.SaveAs(Server.MapPath("/Templates/vshop/default/images/qrcode/qrcode_02.png"));
            }

            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.Button1.Click += new EventHandler(this.btnSave_Click);
            
        }
    }
}

