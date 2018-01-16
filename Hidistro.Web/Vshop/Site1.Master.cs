using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.Web
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        public string Title { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = SettingsManager.GetMasterSettings(true).SiteName;
            
        }
    }
}