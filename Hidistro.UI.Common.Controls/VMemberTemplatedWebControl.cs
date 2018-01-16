namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Data;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class VMemberTemplatedWebControl : VshopTemplatedWebControl
    {
        protected VMemberTemplatedWebControl()
        {
            if (((MemberProcessor.GetCurrentMember() == null) || (this.Page.Session["userid"] == null)) || (this.Page.Session["userid"].ToString() != MemberProcessor.GetCurrentMember().UserId.ToString()))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                if (masterSettings.IsValidationService)
                {
                    string msg = this.Page.Request.QueryString["code"];
                    //Utils.LogWriter.SaveLog("msg code:" + msg);

                    if (!string.IsNullOrEmpty(msg))
                    {
                        string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&code=" + msg + "&grant_type=authorization_code");
                        //Utils.LogWriter.SaveLog("code responseResult:" + responseResult);


                        if (responseResult.Contains("access_token"))
                        {
                            //Utils.LogWriter.SaveLog("msg access_token:" + responseResult);
                            JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                            string str3 = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");

                            if (str3.Contains("nickname"))
                            {
                                JObject obj3 = JsonConvert.DeserializeObject(str3) as JObject;
                                string generateId = Globals.GetGenerateId();
                                MemberInfo member = new MemberInfo
                                {
                                    GradeId = MemberProcessor.GetDefaultMemberGrade(),
                                    UserName = Globals.UrlDecode(obj3["nickname"].ToString()),
                                    OpenId = obj3["openid"].ToString(),
                                    CreateDate = DateTime.Now,
                                    SessionId = generateId,
                                    SessionEndTime = DateTime.Now.AddYears(10)
                                };
                                if ((MemberProcessor.GetCurrentMember() != null) && string.IsNullOrEmpty(MemberProcessor.GetCurrentMember().OpenId))
                                {
                                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                                    currentMember.OpenId = member.OpenId;
                                    MemberProcessor.UpdateMember(currentMember);
                                }

                                //跳转前记录当前ID
                                //Utils.LogWriter.SaveLog("微信登录关联推荐人1：");
                                HttpCookie cookie3 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                                if ((cookie3 != null) && !string.IsNullOrEmpty(cookie3.Value))
                                {
                                    try
                                    {
                                        //Utils.LogWriter.SaveLog("微信登录关联推荐人2：" + cookie3.Value);
                                        if (int.Parse(cookie3.Value) > 0)
                                        {
                                            member.ReferralUserId = int.Parse(cookie3.Value);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }
                                }

                                MemberProcessor.CreateMember(member);
                                MemberProcessor.GetusernameMember(Globals.UrlDecode(obj3["nickname"].ToString()));
                                MemberInfo info3 = MemberProcessor.GetMember(generateId);
                                HttpCookie cookie = new HttpCookie("Vshop-Member") {
                                    Value = info3.UserId.ToString(),
                                    Expires = DateTime.Now.AddYears(10)
                                };
                                HttpContext.Current.Response.Cookies.Add(cookie);
                                this.Page.Session["userid"] = info3.UserId.ToString();
                                DistributorsInfo userIdDistributors = new DistributorsInfo();
                                userIdDistributors = DistributorsBrower.GetUserIdDistributors(info3.UserId);
                                if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                                {
                                    HttpCookie cookie2 = new HttpCookie("Vshop-ReferralId") {
                                        Value = userIdDistributors.UserId.ToString(),
                                        Expires = DateTime.Now.AddYears(1)
                                    };
                                    HttpContext.Current.Response.Cookies.Add(cookie2);
                                }
                                this.Page.Response.Redirect(HttpContext.Current.Request.Url.ToString());
                            }
                            else
                            {
                                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/Default.aspx");
                            }
                        }
                        else
                        {
                            this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/Default.aspx");
                        }
                    }
                    else if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
                    {
                        this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/Default.aspx");
                    }
                    else
                    {
                        string str5 = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + masterSettings.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
                        //Utils.LogWriter.SaveLog("授权跳转:" + str5);
                        this.Page.Response.Redirect(str5);
                    }
                }
                else if (this.Page.Request.Cookies["Vshop-Member"] == null)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/UserLogin.aspx");
                }
            }
        }

        private string GetResponseResult(string url)
        {
            using (HttpWebResponse response = (HttpWebResponse) WebRequest.Create(url).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        //public void WriteError(string msg, string OpenId)
        //{
        //    DataTable table = new DataTable {
        //        TableName = "wxlogin"
        //    };
        //    table.Columns.Add("OperTime");
        //    table.Columns.Add("ErrorMsg");
        //    table.Columns.Add("OpenId");
        //    table.Columns.Add("PageUrl");
        //    DataRow row = table.NewRow();
        //    row["OperTime"] = DateTime.Now;
        //    row["ErrorMsg"] = msg;
        //    row["OpenId"] = OpenId;
        //    row["PageUrl"] = HttpContext.Current.Request.Url;
        //    table.Rows.Add(row);
        //    table.WriteXml(HttpContext.Current.Request.MapPath("/wxlogin.xml"));
        //}
    }
}

