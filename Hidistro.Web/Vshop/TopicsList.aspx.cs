using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.Web.Vshop
{
    public partial class TopicsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                BasePage.AddSiteNameTitle("本站专题");

                this.Repeater1.DataSource = VShopHelper.GetHomeTopics();
                this.Repeater1.DataBind();
            }
        }
    }
}