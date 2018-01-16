namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class AddManager : AdminPage
    {
        protected Button btnCreate;
        protected RoleDropDownList dropRole;
        protected TextBox txtEmail;
        protected TextBox txtPassword;
        protected TextBox txtPasswordagain;
        protected TextBox txtUserName;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
            {
                this.ShowMsg("请确保两次输入的密码相同", false);
            }
            else
            {
                ManagerInfo manager = new ManagerInfo {
                    RoleId = this.dropRole.SelectedValue,
                    UserName = this.txtUserName.Text.Trim(),
                    Email = this.txtEmail.Text.Trim(),
                    Password = HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim())
                };
                if (ManagerHelper.Create(manager))
                {
                    this.txtEmail.Text = string.Empty;
                    this.txtUserName.Text = string.Empty;
                    this.ShowMsg("成功添加了一个管理员", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropRole.DataBind();
            }
        }
    }
}

