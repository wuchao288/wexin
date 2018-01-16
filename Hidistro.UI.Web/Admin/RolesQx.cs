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
    public class RolesQx : AdminPage
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Qx { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RoleId = int.Parse(Request.QueryString["role_id"]);
                RoleName = ManagerHelper.GetRole(RoleId).RoleName;
                IList<RoleQx> list = ManagerHelper.GetRoleQx(RoleId);

                for (int i = 0; i < list.Count; i++)
			    {
                    if (i == 0)
                    {
                        Qx = list[i].Qx;
                    }
                    else {
                        Qx += "," + list[i].Qx;
                    }
			    }
            }
        }

    }
}

