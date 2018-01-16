namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Web.API;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    /// <summary>
    /// 后台管理员登陆的逻辑，有授权验证
    /// </summary>
    public class Login : Page
    {
        protected Button btnAdminLogin;
        protected HtmlForm form1;
        protected HeadContainer HeadContainer1;
        protected SmallStatusMessage lblStatus;
        private readonly string licenseMsg = ("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的微分销系统未经官方授权，无法登录后台管理。请联系微分销官方购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "Vshop/\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2014 hishop.com.cn all Rights Reserved. 本产品资源均为 Hishop 版权所有</div>\r\n</div>\r\n</body>\r\n</html>");
        private readonly string noticeMsg = ("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的微分销系统已过授权有效期，无法登录后台管理。请续费。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "Vshop/\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2014 hishop.com.cn all Rights Reserved. 本产品资源均为 Hishop 版权所有</div>\r\n</div>\r\n</body>\r\n</html>");
        protected PageTitle PageTitle1;
        protected Panel Panel1;
        protected TextBox txtAdminName;
        protected TextBox txtAdminPassWord;
        protected TextBox txtCode;
        private string verifyCodeKey = "VerifyCode";

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            if (!Globals.CheckVerifyCode(this.txtCode.Text.Trim()))
            {
                this.ShowMessage("验证码不正确");
            }
            else
            {
                var c = HiCryptographer.Md5Encrypt(this.txtAdminPassWord.Text);
                ManagerInfo manager = ManagerHelper.GetManager(this.txtAdminName.Text);
                if (manager == null)
                {
                    this.ShowMessage("无效的用户信息");
                }
                else if (manager.Password != HiCryptographer.Md5Encrypt(this.txtAdminPassWord.Text))
                {
                    this.ShowMessage("密码不正确");
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("Vshop-Manager")
                    {
                        Value = manager.UserId.ToString(),
                        Expires = DateTime.Now.AddDays(1.0)
                    };

                    HttpCookie roleid = new HttpCookie("roleid")
                    {
                        Value = manager.RoleId.ToString(),
                        Expires = DateTime.Now.AddDays(1.0)
                    };
                    HttpContext.Current.Response.Cookies.Add(roleid);

                    HttpContext.Current.Response.Cookies.Add(cookie);
                    this.Page.Response.Redirect("Default.aspx", true);
                }
            }
        }

        private bool CheckVerifyCode(string verifyCode)
        {
            if (base.Request.Cookies[this.verifyCodeKey] == null)
            {
                return false;
            }
            return (string.Compare(HiCryptographer.Decrypt(base.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0);
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdminLogin.Click += new EventHandler(this.btnAdminLogin_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                string verifyCode = base.Request["code"];
                string str2 = "";
                if (!this.CheckVerifyCode(verifyCode))
                {
                    str2 = "0";
                }
                else
                {
                    str2 = "1";
                }
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{ ");
                base.Response.Write(string.Format("\"flag\":\"{0}\"", str2));
                base.Response.Write("}");
                base.Response.End();
            }
            if (!this.Page.IsPostBack)
            {
                Uri urlReferrer = this.Context.Request.UrlReferrer;
                if (urlReferrer != null)
                {
                    this.ReferralLink = urlReferrer.ToString();
                }
                this.txtAdminName.Focus();
                PageTitle.AddSiteNameTitle("后台登录");
            }
        }

        /// <summary>
        /// 修改：2015-4-15
        /// 去掉验证版权
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            //改下判断 绝对返回1就不限域名了 你想要给你客户限域名你就修改一下加上你自己的判断 
            //0是未通过验证，-1是过期

            #region  验证版权的
            //string str = APIHelper.PostData("http://ysc.kuaidiantong.cn/valid.ashx", "action=login&product=3&version=1.5&host=" + Globals.DomainName);
            //if (!string.IsNullOrEmpty(str))
            //{
            //    switch (Convert.ToInt32(str.Replace("{\"state\":\"", "").Replace("\"}", "")))
            //    {
            //        case 0:
            //            writer.Write(this.licenseMsg);
            //            return;

            //        case -1:
            //            writer.Write(this.noticeMsg);
            //            return;
            //    }
            //}
            //base.Render(writer);

            #endregion

           // string ss=  APIHelper.PostData("http://ysc.kuaidiantong.cn/valid.ashx", "action=login&product=3&version=1.5&host=" + Globals.DomainName);

            string str = "0";
            //if (Globals.DomainName.IndexOf("uweixin.cn")>=0 || Globals.DomainName == "localhost")
            //{
            //    str = "1";
            //}
            str = "1";
            if (!string.IsNullOrEmpty(str))
            {
                switch (Convert.ToInt32(str.Replace("{\"state\":\"", "").Replace("\"}", "")))
                {
                    case 0:
                        writer.Write(this.licenseMsg);
                        return;

                    case -1:
                        writer.Write(this.noticeMsg);
                        return;
                }
            }
            base.Render(writer);



        }

        private void ShowMessage(string msg)
        {
            this.lblStatus.Text = msg;
            this.lblStatus.Success = false;
            this.lblStatus.Visible = true;
        }

        private string ReferralLink
        {
            get
            {
                return (this.ViewState["ReferralLink"] as string);
            }
            set
            {
                this.ViewState["ReferralLink"] = value;
            }
        }
    }
}

