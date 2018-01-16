namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.MP;
    using Hishop.Weixin.MP.Api;
    using Hishop.Weixin.MP.Domain;
    using Hishop.Weixin.MP.Handler;
    using Hishop.Weixin.MP.Request;
    using Hishop.Weixin.MP.Request.Event;
    using Hishop.Weixin.MP.Response;
    using Hishop.Weixin.MP.Util;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Xml.Linq;
    using ThoughtWorks.QRCode.Codec;

    public class CustomMsgHandler : RequestHandler
    {
        public CustomMsgHandler(string requestDocument)
            : base(requestDocument)
        {

        }

        public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
        {
            ReplyInfo mismatchReply = ReplyHelper.GetMismatchReply();
            if ((mismatchReply == null) || this.IsOpenManyService())
            {
                return this.GotoManyCustomerService(requestMessage);
            }
            AbstractResponse response = this.GetResponse(mismatchReply, requestMessage.FromUserName);
            if (response == null)
            {
                return this.GotoManyCustomerService(requestMessage);
            }
            response.ToUserName = requestMessage.FromUserName;
            response.FromUserName = requestMessage.ToUserName;
            return response;
        }

        private AbstractResponse GetKeyResponse(string key, AbstractRequest request)
        {
            IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Topic);
            if ((replies != null) && (replies.Count > 0))
            {
                foreach (ReplyInfo info in replies)
                {
                    if (info.Keys == key)
                    {
                        TopicInfo topic = VShopHelper.Gettopic(info.ActivityId);
                        if (topic != null)
                        {
                            NewsResponse response = new NewsResponse
                            {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article item = new Article
                            {
                                Description = topic.Title,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, topic.IconUrl),
                                Title = topic.Title,
                                Url = string.Format("http://{0}/vshop/Topics.aspx?TopicId={1}", HttpContext.Current.Request.Url.Host, topic.TopicId)
                            };
                            response.Articles.Add(item);
                            return response;
                        }
                    }
                }
            }
            IList<ReplyInfo> list2 = ReplyHelper.GetReplies(ReplyType.Vote);
            if ((list2 != null) && (list2.Count > 0))
            {
                foreach (ReplyInfo info3 in list2)
                {
                    if (info3.Keys == key)
                    {
                        VoteInfo voteById = StoreHelper.GetVoteById((long)info3.ActivityId);
                        if ((voteById != null) && voteById.IsBackup)
                        {
                            NewsResponse response2 = new NewsResponse
                            {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article2 = new Article
                            {
                                Description = voteById.VoteName,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, voteById.ImageUrl),
                                Title = voteById.VoteName,
                                Url = string.Format("http://{0}/vshop/Vote.aspx?voteId={1}", HttpContext.Current.Request.Url.Host, voteById.VoteId)
                            };
                            response2.Articles.Add(article2);
                            return response2;
                        }
                    }
                }
            }
            IList<ReplyInfo> list3 = ReplyHelper.GetReplies(ReplyType.Wheel);
            if ((list3 != null) && (list3.Count > 0))
            {
                foreach (ReplyInfo info5 in list3)
                {
                    if (info5.Keys == key)
                    {
                        LotteryActivityInfo lotteryActivityInfo = VShopHelper.GetLotteryActivityInfo(info5.ActivityId);
                        if (lotteryActivityInfo != null)
                        {
                            NewsResponse response3 = new NewsResponse
                            {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article3 = new Article
                            {
                                Description = lotteryActivityInfo.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityPic),
                                Title = lotteryActivityInfo.ActivityName,
                                Url = string.Format("http://{0}/vshop/BigWheel.aspx?activityId={1}", HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityId)
                            };
                            response3.Articles.Add(article3);
                            return response3;
                        }
                    }
                }
            }
            IList<ReplyInfo> list4 = ReplyHelper.GetReplies(ReplyType.Scratch);
            if ((list4 != null) && (list4.Count > 0))
            {
                foreach (ReplyInfo info7 in list4)
                {
                    if (info7.Keys == key)
                    {
                        LotteryActivityInfo info8 = VShopHelper.GetLotteryActivityInfo(info7.ActivityId);
                        if (info8 != null)
                        {
                            NewsResponse response4 = new NewsResponse
                            {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article4 = new Article
                            {
                                Description = info8.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, info8.ActivityPic),
                                Title = info8.ActivityName,
                                Url = string.Format("http://{0}/vshop/Scratch.aspx?activityId={1}", HttpContext.Current.Request.Url.Host, info8.ActivityId)
                            };
                            response4.Articles.Add(article4);
                            return response4;
                        }
                    }
                }
            }
            IList<ReplyInfo> list5 = ReplyHelper.GetReplies(ReplyType.SmashEgg);
            if ((list5 != null) && (list5.Count > 0))
            {
                foreach (ReplyInfo info9 in list5)
                {
                    if (info9.Keys == key)
                    {
                        LotteryActivityInfo info10 = VShopHelper.GetLotteryActivityInfo(info9.ActivityId);
                        if (info10 != null)
                        {
                            NewsResponse response5 = new NewsResponse
                            {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article5 = new Article
                            {
                                Description = info10.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, info10.ActivityPic),
                                Title = info10.ActivityName,
                                Url = string.Format("http://{0}/vshop/SmashEgg.aspx?activityId={1}", HttpContext.Current.Request.Url.Host, info10.ActivityId)
                            };
                            response5.Articles.Add(article5);
                            return response5;
                        }
                    }
                }
            }
            IList<ReplyInfo> list6 = ReplyHelper.GetReplies(ReplyType.SignUp);
            if ((list6 != null) && (list6.Count > 0))
            {
                foreach (ReplyInfo info11 in list6)
                {
                    if (info11.Keys == key)
                    {
                        ActivityInfo activity = VShopHelper.GetActivity(info11.ActivityId);
                        if (activity != null)
                        {
                            NewsResponse response6 = new NewsResponse
                            {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article6 = new Article
                            {
                                Description = activity.Description,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, activity.PicUrl),
                                Title = activity.Name,
                                Url = string.Format("http://{0}/vshop/Activity.aspx?id={1}", HttpContext.Current.Request.Url.Host, activity.ActivityId)
                            };
                            response6.Articles.Add(article6);
                            return response6;
                        }
                    }
                }
            }
            IList<ReplyInfo> list7 = ReplyHelper.GetReplies(ReplyType.Ticket);
            if ((list7 != null) && (list7.Count > 0))
            {
                foreach (ReplyInfo info13 in list7)
                {
                    if (info13.Keys == key)
                    {
                        LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(info13.ActivityId);
                        if (lotteryTicket != null)
                        {
                            NewsResponse response7 = new NewsResponse
                            {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article7 = new Article
                            {
                                Description = lotteryTicket.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityPic),
                                Title = lotteryTicket.ActivityName,
                                Url = string.Format("http://{0}/vshop/SignUp.aspx?id={1}", HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityId)
                            };
                            response7.Articles.Add(article7);
                            return response7;
                        }
                    }
                }
            }
            return null;
        }

        //二维码

        public AbstractResponse GetQRCodeResponse(string openId)
        {
            //判断是否注册
            if (!MemberProcessor.IsExitOpenId(openId))
            {
                TextResponse response = new TextResponse
                {
                    CreateTime = DateTime.Now,
                    Content = "您购买后系统会自动生成您的推广二维码海报."
                };
                return response;
            }

            Hidistro.Entities.Members.MemberInfo member = MemberProcessor.GetMembers(openId);
            DistributorsInfo info = DistributorsBrower.GetUserIdDistributors(member.UserId);
            if (info == null)
            {
                TextResponse response = new TextResponse
                {
                    CreateTime = DateTime.Now,
                    Content = "您购买后系统会自动生成您的推广二维码海报."
                };
                return response;
            }

            ImageResponse imgResponse = new ImageResponse()
            {
                CreateTime = DateTime.Now,
                Image = new Hishop.Weixin.MP.Domain.Image()
            };

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

            //生成二维码图片
            string sApplicationPath = System.Web.HttpContext.Current.Request.MapPath("/Templates/vshop/default/images/qrcode/");

            string qr_code = sApplicationPath + @"/QRCodeBg.jpg";

            string qr_code_name = sApplicationPath + @"/tmp/QRCode_" + info.UserId + ".jpg";    //二维码图片
            string qr_code_bg = sApplicationPath + @"/tmp/QRCode_bg_" + info.UserId + ".jpg";   //拼接后的图片
            string link = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/vshop/Default.aspx?ReferralId=" + info.UserId;   //二维码链接

            if (!File.Exists(qr_code_name))
            {
                //生成二维码名片
                string code_url = TokenApi.CreateQRCode(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, info.UserId);

                //下载二维码
                Utils.HttpHelper helper = new Utils.HttpHelper();
                helper.DownloadFiles(code_url, qr_code_name);

                //头像图片
                string img_logo_src = System.Web.HttpContext.Current.Request.MapPath("/") + info.Logo;   //拼接后的图片
                Bitmap img_logo = new Bitmap(img_logo_src);

                //拼接
                File.Copy(qr_code, qr_code_bg, false);

                Bitmap newImg = new Bitmap(qr_code);

                Graphics g = Graphics.FromImage(newImg);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //下面这个也设成高质量 
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //下面这个设成High 
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                System.Drawing.Image erweima = new Bitmap(qr_code_name);
                g.DrawImage(erweima, 158, 430, 204, 204);   //画二维码

                g.DrawImage(img_logo, 35, 26, 77, 77);                              //画头像

                //我是...
                SolidBrush drawBrush = new SolidBrush(Color.Red);
                SolidBrush drawBrush2 = new SolidBrush(Color.Black);

                Font drawFont = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel);

                MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(member.GradeId);
                string str = member.UserName + " (" + memberGrade.Name + ")";
                string str2 = info.StoreName + " 代言";

                g.DrawString("我是 ", drawFont, drawBrush2, 140, 50);
                g.DrawString(str, drawFont, drawBrush, 180, 50);

                g.DrawString("我为 ", drawFont, drawBrush2, 140, 75);
                g.DrawString(str2, drawFont, drawBrush, 180, 75);

                g.Dispose();
                newImg.Save(qr_code_bg);
            }

            //Utils.LogWriter.SaveLog("素材图片：" + qr_code_bg);

            //上传素材
            imgResponse.Image.MediaId = TokenApi.AddNews(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, qr_code_bg);
            //Utils.LogWriter.SaveLog("上传素材ID：" + imgResponse.Image.MediaId);

            return imgResponse;
        }

        public AbstractResponse GetResponse(ReplyInfo reply, string openId)
        {
            if (reply.MessageType == MessageType.Text)
            {
                TextReplyInfo info = reply as TextReplyInfo;
                TextResponse response = new TextResponse
                {
                    CreateTime = DateTime.Now,
                    Content = info.Text
                };
                if (reply.Keys == "登录")
                {
                    string str = string.Format("http://{0}/Vshop/Login.aspx?SessionId={1}", HttpContext.Current.Request.Url.Host, openId);
                    response.Content = response.Content.Replace("$login$", string.Format("<a href=\"{0}\">一键登录</a>", str));
                }
                return response;
            }
            NewsResponse response2 = new NewsResponse
            {
                CreateTime = DateTime.Now,
                Articles = new List<Article>()
            };
            foreach (NewsMsgInfo info2 in (reply as NewsReplyInfo).NewsMsg)
            {
                Article item = new Article
                {
                    Description = info2.Description,
                    PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, info2.PicUrl),
                    Title = info2.Title,
                    Url = string.IsNullOrEmpty(info2.Url) ? string.Format("http://{0}/Vshop/ImageTextDetails.aspx?messageId={1}", HttpContext.Current.Request.Url.Host, info2.Id) : info2.Url
                };
                response2.Articles.Add(item);
            }
            return response2;
        }

        public AbstractResponse GotoManyCustomerService(AbstractRequest requestMessage)
        {
            if (!this.IsOpenManyService())
            {
                return null;
            }
            return new AbstractResponse { FromUserName = requestMessage.ToUserName, ToUserName = requestMessage.FromUserName, MsgType = ResponseMsgType.transfer_customer_service };
        }

        public bool IsOpenManyService()
        {
            return SettingsManager.GetMasterSettings(false).OpenManyService;
        }

        //扫描二维码事件
        public override AbstractResponse OnEvent_ScanRequest(ScanEventRequest scanEventRequest)
        {
            Utils.LogWriter.SaveLog("产生扫码事件:" + scanEventRequest.EventKey);
            if (!string.IsNullOrEmpty(scanEventRequest.EventKey))
            {
                /*
                 * 打开分享图片
                 * 获取用户信息
                 * 
                 * */

                string open_id = scanEventRequest.FromUserName;
                int ReferralUserId = int.Parse(scanEventRequest.EventKey);

                //判断是否会员
                if (MemberProcessor.IsExitOpenId(open_id))
                {
                    //Utils.LogWriter.SaveLog("已存在会员:" + open_id);
                    Hidistro.Entities.Members.MemberInfo m = MemberProcessor.GetMembers(open_id);
                    if (m.ReferralUserId == 0 && m.OpenId != open_id)
                    {
                        m.ReferralUserId = ReferralUserId;
                        MemberProcessor.UpdateMember(m);
                        Utils.LogWriter.SaveLog("扫码填补上级ID:" + ReferralUserId);
                        return null;
                    }

                    //Utils.LogWriter.SaveLog("返回提示:" + open_id);
                    TextResponse response = new TextResponse
                    {
                        CreateTime = DateTime.Now,
                        Content = "您已经有上级了哦",
                        ToUserName = scanEventRequest.FromUserName,
                        FromUserName = scanEventRequest.ToUserName
                    };
                    return response;
                }

                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

                JObject wx_user_info = TokenApi.GetUserInfo(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, open_id);
                Utils.LogWriter.SaveLog("wx_user_info:" + wx_user_info["nickname"].ToString());

                string generateId = Globals.GetGenerateId();
                Hidistro.Entities.Members.MemberInfo member = new Hidistro.Entities.Members.MemberInfo
                {
                    GradeId = MemberProcessor.GetDefaultMemberGrade(),
                    UserName = Globals.UrlDecode(wx_user_info["nickname"].ToString()),
                    OpenId = open_id,
                    CreateDate = DateTime.Now,
                    SessionId = generateId,
                    SessionEndTime = DateTime.Now.AddYears(10),
                    ReferralUserId = ReferralUserId
                };

                //Utils.LogWriter.SaveLog("创建客户资料");
                MemberProcessor.CreateMember(member);
            }

            return null;
        }

        public override AbstractResponse OnEvent_ClickRequest(ClickEventRequest clickEventRequest)
        {
            MenuInfo menu = VShopHelper.GetMenu(Convert.ToInt32(clickEventRequest.EventKey));
            if (menu == null)
            {
                return null;
            }
            //Utils.LogWriter.SaveLog("menu bind：" + menu.Bind);
            //我的二维码
            if (menu.Bind == 9)
            {

                AbstractResponse codeResponse = this.GetQRCodeResponse(clickEventRequest.FromUserName);
                codeResponse.ToUserName = clickEventRequest.FromUserName;
                codeResponse.FromUserName = clickEventRequest.ToUserName;

                //Utils.LogWriter.SaveLog("返回二维码图片");
                return codeResponse;
            }

            ReplyInfo reply = ReplyHelper.GetReply(menu.ReplyId);
            if (reply == null)
            {
                return null;
            }
            AbstractResponse keyResponse = this.GetKeyResponse(reply.Keys, clickEventRequest);
            if (keyResponse != null)
            {
                return keyResponse;
            }
            AbstractResponse response = this.GetResponse(reply, clickEventRequest.FromUserName);
            if (response == null)
            {
                this.GotoManyCustomerService(clickEventRequest);
            }
            response.ToUserName = clickEventRequest.FromUserName;
            response.FromUserName = clickEventRequest.ToUserName;
            return response;
        }

        //关注事件
        public override AbstractResponse OnEvent_SubscribeRequest(SubscribeEventRequest subscribeEventRequest)
        {
            Utils.LogWriter.SaveLog("产生关注事件:" + subscribeEventRequest.EventKey);
            string event_key = subscribeEventRequest.EventKey;
            if (!string.IsNullOrEmpty(event_key) && event_key.IndexOf("qrscene_") != -1)
            {
                //关联上级
                /*
                 * 打开分享图片
                 * 获取用户信息
                 * 
                 * */

                string open_id = subscribeEventRequest.FromUserName;
                Utils.LogWriter.SaveLog("产生关注事件step1:" + open_id);
                int ReferralUserId = int.Parse(subscribeEventRequest.EventKey.Replace("qrscene_", ""));
                Utils.LogWriter.SaveLog("产生关注事件step2:" + ReferralUserId);

                //判断是否会员
                if (MemberProcessor.IsExitOpenId(open_id))
                {
                    Utils.LogWriter.SaveLog("产生关注事件step3:已存在会员信息");
                    Hidistro.Entities.Members.MemberInfo m = MemberProcessor.GetMembers(open_id);
                    if (m.ReferralUserId == 0 && m.OpenId != open_id)
                    {
                        m.ReferralUserId = ReferralUserId;
                        MemberProcessor.UpdateMember(m);
                        //Utils.LogWriter.SaveLog("扫码填补上级ID:" + ReferralUserId);
                        return null;
                    }
                }
                else
                {
                    Utils.LogWriter.SaveLog("产生关注事件step3:不存在会员信息");
                    //关联上级并增加客户资料
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

                    JObject wx_user_info = TokenApi.GetUserInfo(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, open_id);
                    Utils.LogWriter.SaveLog("产生关注事件step4:" + wx_user_info["nickname"].ToString());

                    string generateId = Globals.GetGenerateId();
                    Hidistro.Entities.Members.MemberInfo member = new Hidistro.Entities.Members.MemberInfo
                    {
                        GradeId = MemberProcessor.GetDefaultMemberGrade(),
                        UserName = Globals.UrlDecode(wx_user_info["nickname"].ToString()),
                        OpenId = open_id,
                        CreateDate = DateTime.Now,
                        SessionId = generateId,
                        SessionEndTime = DateTime.Now.AddYears(10),
                        ReferralUserId = ReferralUserId
                    };

                    Utils.LogWriter.SaveLog("创建客户资料");
                    MemberProcessor.CreateMember(member);

                    //获取上级
                    Hidistro.Entities.Members.MemberInfo parentInfo = MemberProcessor.GetMember(ReferralUserId);

                    //获取第多少个会员
                    int count = MemberProcessor.GetMemberCount();

                    TextResponse r = new TextResponse
                    {
                        CreateTime = DateTime.Now,
                        Content = "恭喜您！您已通过【" + parentInfo.UserName + "】的推荐成为本站会员，您是本站第" + (10000 + count) 
                        + "个会员，点击右下方【创业良机】～【组建团队】进入财富倍增快通道。",
                        ToUserName = subscribeEventRequest.FromUserName,
                        FromUserName = subscribeEventRequest.ToUserName
                    };
                    return r;
                }
            }

            ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply();
            if (subscribeReply == null)
            {
                return null;
            }
            subscribeReply.Keys = "登录";
            AbstractResponse response = this.GetResponse(subscribeReply, subscribeEventRequest.FromUserName);
            if (response == null)
            {
                this.GotoManyCustomerService(subscribeEventRequest);
            }
            response.ToUserName = subscribeEventRequest.FromUserName;
            response.FromUserName = subscribeEventRequest.ToUserName;
            return response;
        }

        public override AbstractResponse OnTextRequest(TextRequest textRequest)
        {
            AbstractResponse keyResponse = this.GetKeyResponse(textRequest.Content, textRequest);
            if (keyResponse != null)
            {
                return keyResponse;
            }
            IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Keys);
            if ((replies == null) || ((replies.Count == 0) && this.IsOpenManyService()))
            {
                this.GotoManyCustomerService(textRequest);
            }
            foreach (ReplyInfo info in replies)
            {
                if ((info.MatchType == MatchType.Equal) && (info.Keys == textRequest.Content))
                {
                    AbstractResponse response = this.GetResponse(info, textRequest.FromUserName);
                    response.ToUserName = textRequest.FromUserName;
                    response.FromUserName = textRequest.ToUserName;
                    return response;
                }
                if ((info.MatchType == MatchType.Like) && info.Keys.Contains(textRequest.Content))
                {
                    AbstractResponse response3 = this.GetResponse(info, textRequest.FromUserName);
                    response3.ToUserName = textRequest.FromUserName;
                    response3.FromUserName = textRequest.ToUserName;
                    return response3;
                }
            }
            return this.DefaultResponse(textRequest);
        }
    }
}

