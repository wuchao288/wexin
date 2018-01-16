namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VLoginOut : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("Vshop-Member");
            if (cookie != null)
            {
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            HttpCookie cookie2 = HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId");
            if (cookie2 != null)
            {
                cookie2.Value = null;
                cookie2.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Set(cookie2);
            }
            this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/UserLogin.aspx");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VLogout.html";
            }
            base.OnInit(e);
        }
    }
}

