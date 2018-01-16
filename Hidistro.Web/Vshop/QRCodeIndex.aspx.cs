using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.Web.Vshop
{
    public partial class QRCodeIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ReferralId = Request.QueryString["ReferralId"];

            if (!string.IsNullOrEmpty(ReferralId) && int.Parse(ReferralId) > 0)
            {
                Response.Redirect("/vshop/QRcode.aspx?ReferralId=" + ReferralId);
            }
        }
    }
}