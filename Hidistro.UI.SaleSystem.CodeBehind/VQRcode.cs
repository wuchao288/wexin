namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VQRcode : VshopTemplatedWebControl
    {
        private Image litimgorcode;
        private Literal litstorename;
        private Literal liturl;
        private Literal litName;
        private Image imglogo;
        
        protected override void AttachChildControls()
        {
            this.litimgorcode = (Image) this.FindControl("litimgorcode");
            this.liturl = (Literal) this.FindControl("liturl");
            this.litstorename = (Literal)this.FindControl("litstorename");
            this.litName = (Literal)this.FindControl("litName");

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                cookie.Value = HttpContext.Current.Request.Cookies["Vshop-ReferralId"].ToString();
                HttpContext.Current.Response.Cookies.Add(cookie);

                string str = "http://s.jiathis.com/qrcode.php?url=" + Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/Default.aspx?ReferralId=" + this.Page.Request.QueryString["ReferralId"];
                this.litimgorcode.ImageUrl = str;
                this.liturl.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/Default.aspx?ReferralId=" + this.Page.Request.QueryString["ReferralId"];

                this.litstorename.Text = masterSettings.SiteName;

                MemberInfo currentMember = MemberProcessor.GetMember(int.Parse(this.Page.Request.QueryString["ReferralId"]));
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(base.referralId);


                string name = "";

                if (currentMember != null)
                {
                    if (!string.IsNullOrEmpty(currentMember.RealName))
                    {
                        name = currentMember.RealName;
                    }
                    else if (!string.IsNullOrEmpty(currentMember.UserName))
                    {
                        name = currentMember.UserName;
                    }
                    MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMember.GradeId);
                    name += "(" + memberGrade.Name + ")";
                }
                
                this.litName.Text = name;
            }
            PageTitle.AddSiteNameTitle("店铺二维码");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VQRcode.html";
            }
            base.OnInit(e);
        }
    }
}

