namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class Roles : AdminPage
    {
        protected Button btnEditRoles;
        protected Button btnSubmitRoles;
        protected Grid grdGroupList;
        protected TextBox txtAddRoleName;
        protected TextBox txtEditRoleDesc;
        protected TextBox txtEditRoleName;
        protected TextBox txtRoleDesc;
        protected HtmlInputHidden txtRoleId;
        protected HtmlInputHidden txtRoleName;

        public void BindUserGroup()
        {
            this.grdGroupList.DataSource = ManagerHelper.GetRoles();
            this.grdGroupList.DataBind();
        }

        private void btnEditRoles_Click(object sender, EventArgs e)
        {
            RoleInfo target = new RoleInfo();
            if (string.IsNullOrEmpty(this.txtEditRoleName.Text.Trim()))
            {
                this.ShowMsg("部门名称不能为空，长度限制在60个字符以内", false);
            }
            else if ((string.Compare(this.txtRoleName.Value, this.txtEditRoleName.Text) == 0) || !ManagerHelper.RoleExists(this.txtEditRoleName.Text.Trim().Replace(",", "")))
            {
                target.RoleId = int.Parse(this.txtRoleId.Value);
                target.RoleName = Globals.HtmlEncode(this.txtEditRoleName.Text.Trim()).Replace(",", "");
                target.Description = Globals.HtmlEncode(this.txtEditRoleDesc.Text.Trim());
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<RoleInfo>(target, new string[] { "ValRoleInfo" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    this.ShowMsg(msg, false);
                }
                else
                {
                    ManagerHelper.UpdateRole(target);
                    this.BindUserGroup();
                }
            }
            else
            {
                this.ShowMsg("已经存在相同的部门名称", false);
            }
        }

        protected void btnSubmitRoles_Click(object sender, EventArgs e)
        {
            string str = Globals.HtmlEncode(this.txtAddRoleName.Text.Trim()).Replace(",", "");
            string str2 = Globals.HtmlEncode(this.txtRoleDesc.Text.Trim());
            if (string.IsNullOrEmpty(str) || (str.Length > 60))
            {
                this.ShowMsg("部门名称不能为空，长度限制在60个字符以内", false);
            }
            else if (!ManagerHelper.RoleExists(str))
            {
                RoleInfo role = new RoleInfo {
                    RoleName = str,
                    Description = str2
                };
                ManagerHelper.AddRole(role);
                this.BindUserGroup();
                this.ShowMsg("成功添加了一个部门", true);
            }
            else
            {
                this.ShowMsg("已经存在相同的部门名称", false);
            }
        }

        private void grdGroupList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ManagerHelper.DeleteRole((int) this.grdGroupList.DataKeys[e.RowIndex].Value))
            {
                this.BindUserGroup();
                this.ShowMsg("成功删除了选择的部门", true);
            }
            else
            {
                this.ShowMsg("删除失败，该部门下已有管理员", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmitRoles.Click += new EventHandler(this.btnSubmitRoles_Click);
            this.grdGroupList.RowDeleting += new GridViewDeleteEventHandler(this.grdGroupList_RowDeleting);
            this.btnEditRoles.Click += new EventHandler(this.btnEditRoles_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindUserGroup();
            }
        }

        private void Reset()
        {
            this.txtAddRoleName.Text = string.Empty;
            this.txtRoleDesc.Text = string.Empty;
        }
    }
}

